using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.FastballProduct.Interface;
using System.Collections.Generic;
using Atlantis.Framework.FastballProduct.Impl.offersApiWS;

namespace Atlantis.Framework.FastballProduct.Impl
{
  public class FastballProductRequest : IRequest
  {
    private static DateTime _donotCallUntil;
    private static TimeSpan _tenMinutes;
    private static Dictionary<string, string> _emptyResult;

    static FastballProductRequest()
    {
      _donotCallUntil = DateTime.MinValue;
      _tenMinutes = TimeSpan.FromMinutes(10.0);
      _emptyResult = new Dictionary<string, string>();
    }

    private bool MasterSwitchOn
    {
      get 
      {
        string fastballProductOn = DataCache.DataCache.GetAppSetting("ATLANTIS_FRAMEWORK_FASTBALLPRODUCT_ON");
        return ("true".Equals(fastballProductOn, StringComparison.InvariantCultureIgnoreCase));
      }
    }

    private bool DelayIsInEffect
    {
      get { return DateTime.Now < _donotCallUntil; }
    }

    private void DelayAllCalls(FastballProductRequestData request)
    {
      // only set delay if its not a spoof call
      if (string.IsNullOrEmpty(request.SpoofOfferId))
      {
        _donotCallUntil = DateTime.Now.Add(_tenMinutes);
      }
    }

    private bool CallCanBeMade(FastballProductRequestData request, out int status)
    {
      if (!string.IsNullOrEmpty(request.SpoofOfferId))
      {
        status = FastballProductStatus.Valid;
        return true;
      }

      if (!MasterSwitchOn)
      {
        status = FastballProductStatus.AppSettingOff;
        return false;
      }

      if (DelayIsInEffect)
      {
        status = FastballProductStatus.DelayedOff;
        return false;
      }

      status = FastballProductStatus.Valid;
      return true;
    }

    /// <summary>
    /// This request data is designed to not fail, and always return something that can be cached into the users session
    /// 0. If appsetting is not 'true' we don't call at all.
    /// 1. If the webservice fails, then we return empty values so the consuming application will just use defaults
    /// 2. If the webservice returns a "no testing active" then we set a static DateTime and do not make the call.
    ///    We do this without locking because a few extra calls will not hurt us.
    /// </summary>
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      FastballProductResponseData result = new FastballProductResponseData(_emptyResult, FastballProductStatus.Failed);
      string placement = string.Empty;

      try
      {
        FastballProductRequestData request = (FastballProductRequestData)requestData;
        int status;
        if (!CallCanBeMade(request, out status))
        {
          result.Status = status;
        }
        else // make the call
        {
          // Validate request
          placement = request.Placement;

          if (string.IsNullOrEmpty(placement))
          {
            throw new ArgumentException("Placement is empty or null.");
          }

          using (offersApiWS.Service service = new offersApiWS.Service())
          {
            service.Url = ((WsConfigElement)config).WSURL;
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            var offerResult = service.GetOffersAndMessageData(request.GetChannelRequestXml(), request.GetCandidateRequestXml());

            string offerId;
            OfferMessageDataItem offerMessageDataItem = GetFirstDataItemFromFirstOffer(offerResult, out offerId);
            if (offerMessageDataItem == null)
            {
              DelayAllCalls(request);
            }
            else
            {
              if ((offerMessageDataItem.Attributes != null) && (offerMessageDataItem.Attributes.Length > 0))
              {
                Dictionary<string, string> messageData = new Dictionary<string, string>(offerMessageDataItem.Attributes.Length);
                {
                  foreach(var attribute in offerMessageDataItem.Attributes)
                  {
                    if ((attribute.Values != null) && (attribute.Values.Length > 0))
                    {
                      messageData[attribute.key] = attribute.Values[0];
                    }
                  }
                }
                result = new FastballProductResponseData(messageData, FastballProductStatus.Valid, offerId);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        result = new FastballProductResponseData(_emptyResult, FastballProductStatus.Failed);
        try
        {
          string data = "Placement" + (placement ?? "null");
          AtlantisException aex = new AtlantisException(requestData, "FastballProduct.RequestHandler", ex.Message + ex.StackTrace, data);
          Engine.Engine.LogAtlantisException(aex);
        }
        catch { }
      }

      return result;
    }

    private OfferMessageDataItem GetFirstDataItemFromFirstOffer(OfferResult offerResult, out string offerId)
    {
      offerId = string.Empty;      
      OfferMessageDataItem result = null;

      if ((offerResult.SelectedOffers != null) && (offerResult.SelectedOffers.Length > 0))
      {
        Offer offer = offerResult.SelectedOffers[0];
        if ((offer != null) && (offer.MessageData != null) && (offer.MessageData.DataItems != null) && (offer.MessageData.DataItems.Length > 0))
        {
          offerId = offer.fbiOfferID;
          result = offer.MessageData.DataItems[0];
        }
      }

      return result;
    }

  }
}
