using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.FastballPromoBanners.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballPromoBanners.Impl
{
  public class FastballPromoBannersRequest : IRequest
  {
    private const string BANNER_TEXT_KEY = "bannerText";
    private const string LANDING_PAGE_URL_KEY = "landingPageUrl";

    private const string BANNER_TEXT_VAL = "val";
    private const string PRODUCT_KEY = "productKey";
    private const string CI_CODE_KEY = "ciCode";

    private static readonly HashSet<int> _validResultCodes = new HashSet<int> { 0, -83 };

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      try
      {
        FastballPromoBannersRequestData fastballPromoBannersRequestData = (FastballPromoBannersRequestData)requestData;
        OffersAPIWS.Service offersWs = new OffersAPIWS.Service();
        offersWs.Url = ((WsConfigElement)config).WSURL;
        offersWs.Timeout = (int)fastballPromoBannersRequestData.RequestTimeout.TotalMilliseconds;

        var offersResponse = offersWs.GetOffersAndMessageData(fastballPromoBannersRequestData.ChannelRequestXml,
                                                              fastballPromoBannersRequestData.CandidateRequestXml);

        if (ResultCodes.IsResultException(offersResponse.ResultCode))
        {
          throw new Exception(string.Format("GetOffersAndMessageData call failed. Status:{0}, ChanneRequestlXml:{1}, CandidateRequestXml:{2}", offersResponse.ResultCode, fastballPromoBannersRequestData.ChannelRequestXml, fastballPromoBannersRequestData.CandidateRequestXml));
        }

        
        IList<FastballPromoBanner> bannerList = new List<FastballPromoBanner>(4);

        if (offersResponse.SelectedOffers != null)
        {
          foreach (OffersAPIWS.Offer bannerOffer in offersResponse.SelectedOffers)
          {
            DateTime startDate;
            DateTime endDate;
            DateTime now = DateTime.Now;

            if (DateTime.TryParse(bannerOffer.TargetDate, out startDate) &&
               DateTime.TryParse(bannerOffer.EndDate, out endDate) &&
               now > startDate &&
               now < endDate)
            {
              FastballPromoBanner fastballPromoBanner = new FastballPromoBanner();
              fastballPromoBanner.OfferId = bannerOffer.fbiOfferID;
              fastballPromoBanner.Discount = bannerOffer.fastballDiscount;
              fastballPromoBanner.OrderDiscount = bannerOffer.fastballOrderDiscount;
              fastballPromoBanner.Isc = fastballPromoBannersRequestData.Isc;
              fastballPromoBanner.StartDate = startDate;
              fastballPromoBanner.EndDate = endDate;

              string bannerText = null;
              string ciCode = null;
              string product = null;

              foreach (OffersAPIWS.OfferMessageDataItem dataItem in bannerOffer.MessageData.DataItems)
              {
                foreach (OffersAPIWS.OfferMessageDataItemAttribute attribute in dataItem.Attributes)
                {
                  string attVal = attribute.Values.First();

                  switch (dataItem.ID)
                  {
                    case BANNER_TEXT_KEY:
                      switch (attribute.key)
                      {
                        case BANNER_TEXT_VAL:
                          bannerText = attVal;
                          break;
                      }
                      break;
                    case LANDING_PAGE_URL_KEY:
                      switch (attribute.key)
                      {
                        case CI_CODE_KEY:
                          ciCode = attVal;
                          break;
                        case PRODUCT_KEY:
                          product = attVal;
                          break;
                      }
                      break;
                  }
                }
              }

              fastballPromoBanner.BannerText = bannerText;
              fastballPromoBanner.CiCode = ciCode;
              fastballPromoBanner.Product = product;

              if (!string.IsNullOrEmpty(fastballPromoBanner.BannerText))
              {
                bannerList.Add(fastballPromoBanner);
              }
            }
          }
        }

        responseData = new FastballPromoBannersResponseData(bannerList);
      }
      catch (Exception ex)
      {
        responseData = new FastballPromoBannersResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
