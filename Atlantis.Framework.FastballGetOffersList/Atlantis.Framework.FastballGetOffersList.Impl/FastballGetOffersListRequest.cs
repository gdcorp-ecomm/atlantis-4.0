using Atlantis.Framework.Interface;
using System.Collections.Generic;
using Atlantis.Framework.FastballGetOffersList.Interface;
using Atlantis.Framework.FastballGetOffersList.Impl.OffersAPI;
using System.ServiceModel;
using System;

namespace Atlantis.Framework.FastballGetOffersList.Impl
{
  public class FastballGetOffersListRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;

      FastballGetOffersListRequestData offersRequest = (FastballGetOffersListRequestData)requestData;
      WsConfigElement wsConfig = (WsConfigElement)config;

      if (AreParamsValid(offersRequest))
      {
        OffersAPI.LocationResult locationResult = null;
        try
        {
          using (OffersAPI.Service service = new OffersAPI.Service())
          {
            service.Url = ((WsConfigElement)config).WSURL;
            service.Timeout = (int)offersRequest.RequestTimeout.TotalMilliseconds;
            locationResult = service.GetOffersAndMessageDataWithSubLocations(offersRequest.ChannelRequestXml, offersRequest.CandidateRequestXml);
          }
        }
        catch (Exception ex)
        {
          Engine.Engine.LogAtlantisException(new AtlantisException(requestData, this.GetType().ToString() + "RequestHandler()", "An exception happend when calling the web method GetOffersAndMessageDataWithSubLocations.", string.Empty, ex));
          responseData = new FastballGetOffersListResponse(requestData, ex);
        }

        if (locationResult != null)
        {
          Interface.LocationResult responseLocationResult = ConvertLocationResult(locationResult);
          responseData = new FastballGetOffersListResponse(responseLocationResult);
        }
        else
        {
          Engine.Engine.LogAtlantisException(new AtlantisException(requestData, this.GetType().ToString() + "RequestHandler()", "The webmethod GetOffersAndMessageDataWithSubLocations returned a null response.", string.Empty));
        }
      }

      return responseData;
    }

    #endregion

    #region Private Methods
    private Interface.LocationResult ConvertLocationResult(OffersAPI.LocationResult locationResult)
    {
      Interface.LocationResult responseLocationResult = null;

      responseLocationResult = new Interface.LocationResult() { 
        //TODO - This xml fragment needs to be parsed and returned as an easily consumable object.  Waiting on OffersAPI to tell us the different types of xml fragments that can come back in this field so that we know what to expect so that we can parse the xml.
        CandidateAttributeXml = locationResult.CandidateAttributeXml, 
        RequestUID = locationResult.RequestUID, 
        ResultCode = locationResult.ResultCode, 
        SubLocations = ConvertSubLocations(locationResult.SubLocations) };
      
      return responseLocationResult;
    }

    private List<Interface.SubLocation> ConvertSubLocations(OffersAPI.SubLocation[] subLocations)
    {
      List<Interface.SubLocation> responseSubLocations = new List<Interface.SubLocation>(subLocations.Length);

      foreach (OffersAPI.SubLocation subLocation in subLocations)
      {
        Interface.SubLocation responseSubLocation = new Interface.SubLocation() { 
          Name = subLocation.SubLocationName, 
          SelectedOffers = ConvertSelectedOffers(subLocation.SelectedOffers) };
      }

      return responseSubLocations;
    }

    private List<Interface.SelectedOfferLite> ConvertSelectedOffers(OffersAPI.Offer[] offers)
    {
      List<Interface.SelectedOfferLite> responseOffers = new List<SelectedOfferLite>(offers.Length);

      foreach (OffersAPI.Offer offer in offers)
      {
        DateTime targetDate, endDate;

        if (DateTime.TryParse(offer.TargetDate, out targetDate))
        {
          if (DateTime.TryParse(offer.EndDate, out endDate))
          {
            if (DateTime.Now >= targetDate && DateTime.Now <= endDate)
            {
              Interface.SelectedOfferLite responseOffer = new SelectedOfferLite()
              {
                fbiOfferId = offer.fbiOfferID,
                ProductGroupCode = offer.productGroupCode,
                fbiOfferUId = offer.fbiOfferUID,
                FastballDiscount = offer.fastballDiscount,
                FastballOrderDiscount = offer.fastballOrderDiscount,
                DiscountType = offer.discountType,
                TargetDate = targetDate,
                EndDate = endDate,
                Packages = GetPackages(offer.Packages),
                PromoId = offer.promoID
              };
            }
          }
        }
      }

      return responseOffers;
    }

    private List<KeyValuePair<int, string>> GetPackages(OffersAPI.OfferPackage[] packages)
    {
      List<KeyValuePair<int, string>> responsePackages = new List<KeyValuePair<int, string>>(packages.Length);

      foreach (OffersAPI.OfferPackage package in packages)
      {
        responsePackages.Add(new KeyValuePair<int, string>(package.pkgid, package.pkgType));
      }

      return responsePackages;
    }

    private bool AreParamsValid(FastballGetOffersListRequestData requestData)
    {
      bool isValid = false;

      if (string.IsNullOrEmpty(requestData.OAPIParams.Placement))
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(requestData, this.GetType().ToString() + "AreParamsValid()", "Input validation.  Placement string cannot be empty.", string.Empty));
      }
      else
      {
        if (string.IsNullOrEmpty(requestData.OAPIParams.ShopperId))
        {
          Engine.Engine.LogAtlantisException(new AtlantisException(requestData, this.GetType().ToString() + "AreParamsValid()", "Input validation.  ShopperId cannot be empty.", string.Empty));
        }
        else
        {
          isValid = true;
        }
      }

      return isValid;
    }
    #endregion
  }
}
