using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.FastballGetOffersMsgData.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballGetOffersMsgData.Impl
{
  public class FastballGetOffersMsgDataRequest : IRequest
  {
    private const string URL_FORMAT = "http://{0}{1}";
    
    private const string ACTION_URL_ID = "actionUrl";
    private const string OFFER_IMG_ID = "offerImage";
    private const string LANDING_PAGE_URL = "landingPageUrl";

    private const string CAPTION = "caption";
    private const string CI_CODE = "ciCode";
    private const string NAME = "name";
    private const string VALUE = "val";
    private const string VIEW = "view";
    private const string DOMAIN = "domain";
    private const string SOURCE = "src";
    private const string PRODUCT = "productKey";
    private const string MOBILE_WEB_URL = "mobileweburl";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result;

      try
      {
        FastballGetOffersMsgDataRequestData requestData = (FastballGetOffersMsgDataRequestData)oRequestData;
        OffersAPIWS.Service offersWs = new OffersAPIWS.Service();
        offersWs.Url = ((WsConfigElement)oConfig).WSURL;
        offersWs.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
       
        List<FastBallBannerAd> adList = new List<FastBallBannerAd>();
        int count = 0;
        var offersResponse = offersWs.GetOffersAndMessageData(requestData.ChannelRequestXml, requestData.CandidateRequestXml);
        if( offersResponse.ResultCode != 0 || offersResponse.SelectedOffers == null)
        {
          throw new Exception(string.Format("GetOffersAndMessageData call failed. Status:{0}, ChanneRequestlXml:{1}, CandidateRequestXml:{2}", offersResponse.ResultCode, requestData.ChannelRequestXml, requestData.CandidateRequestXml));
        }

        foreach (OffersAPIWS.Offer offer in offersResponse.SelectedOffers)
        {
          FastBallBannerAd newAd = new FastBallBannerAd();
          newAd.FastBallOfferId = offer.fbiOfferID;
          newAd.FastballDiscount = offer.fastballDiscount;
          newAd.FastballOrderDiscount = offer.fastballOrderDiscount;

          string imageUrl = null;
          string view = null;
          string clickUrl = null;
          string name = null;
          string caption = null;
          string ciCode = null;
          string clickDomain = null;
          string imageDomain = null;
          string product = null;

          foreach (OffersAPIWS.OfferMessageDataItem dataItem in offer.MessageData.DataItems)
          {
            foreach (OffersAPIWS.OfferMessageDataItemAttribute attribute in dataItem.Attributes)
            {
              string attVal = attribute.Values.First();

              switch (dataItem.ID)
              {
                case ACTION_URL_ID:
                  switch (attribute.key)
                  {
                    case VIEW:
                      view = attVal;
                      break;
                    case VALUE:
                      clickUrl = attVal;
                      break;
                    case NAME:
                      name = attVal;
                      break;
                    case CAPTION:
                      caption = attVal;
                      break;
                    case CI_CODE:
                      ciCode = attVal;
                      break;
                    case DOMAIN:
                      clickDomain = attVal;
                      break;
                  }
                  break;

                case OFFER_IMG_ID:
                  switch (attribute.key)
                  {
                    case SOURCE:
                      imageUrl = attVal;
                      break;
                    case DOMAIN:
                      imageDomain = attVal;
                      break;
                  }
                  break;
                
                case LANDING_PAGE_URL:
                  switch (attribute.key)
                  {
                    case PRODUCT:
                      product = attVal;
                      break;
                    case MOBILE_WEB_URL:
                      clickUrl = attVal;
                      break;
                    case CI_CODE:
                      ciCode = attVal;
                      break;
                  }
                  break;
              }
            }            
          }

          newAd.ViewType = view;
          
          newAd.Name = name;
          newAd.Caption = caption;
          newAd.CICode = ciCode;
          newAd.Order = count.ToString();
          newAd.Product = product;

          // Do not remove this check, added for backward compatability
          if (!string.IsNullOrEmpty(clickDomain) && !Uri.IsWellFormedUriString(clickUrl, UriKind.Absolute))
          {
            newAd.ClickUrl = string.Format(URL_FORMAT, clickDomain, clickUrl);
          }
          else
          {
            newAd.ClickUrl = clickUrl;
          }

          // Do not remove this check, added for backward compatability
          if(!string.IsNullOrEmpty(imageDomain) && !Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
          {
            newAd.ImageUrl = string.Format(URL_FORMAT, imageDomain, imageUrl);
          }
          else
          {
            newAd.ImageUrl = imageUrl;
          }

          if (HttpContext.Current != null &&
             HttpContext.Current.Request.IsSecureConnection &&
             newAd.ImageUrl.StartsWith("http:", true, null))
          {
            newAd.ImageUrl = newAd.ImageUrl.ToLower().Replace("http:", "https:");
          }

          adList.Add(newAd);
          
          count++;
        }
        
        result = new FastballGetOffersMsgDataResponseData
                   {
                     FastBallAds = adList,
                     IsSuccess = true,
        
                   };


      }
      catch (Exception ex)
      {
        result = new FastballGetOffersMsgDataResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
