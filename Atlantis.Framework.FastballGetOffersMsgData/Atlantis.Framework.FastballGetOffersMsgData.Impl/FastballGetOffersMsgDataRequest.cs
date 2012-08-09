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
    private const string UrlFormat = "http://{0}{1}";
    
    private const string ActionUrlId = "actionUrl";
    private const string OfferImgId = "offerImage";
    private const string LandingPageUrl = "landingPageUrl";

    private const string Caption = "caption";
    private const string CiCode = "ciCode";
    private const string Name = "name";
    private const string Value = "val";
    private const string View = "view";
    private const string Launch = "launch";
    private const string Domain = "domain";
    private const string Source = "src";
    private const string Product = "productKey";
    private const string MobileWebUrl = "mobileweburl";

    private const string InternalLaunch = "internal"; // default

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
          string launchType = InternalLaunch;

          foreach (OffersAPIWS.OfferMessageDataItem dataItem in offer.MessageData.DataItems)
          {
            foreach (OffersAPIWS.OfferMessageDataItemAttribute attribute in dataItem.Attributes)
            {
              string attVal = attribute.Values.First();

              switch (dataItem.ID)
              {
                case ActionUrlId:
                  switch (attribute.key)
                  {
                    case View:
                      view = attVal;
                      break;
                    case Value:
                      clickUrl = attVal;
                      break;
                    case Name:
                      name = attVal;
                      break;
                    case Caption:
                      caption = attVal;
                      break;
                    case CiCode:
                      ciCode = attVal;
                      break;
                    case Domain:
                      clickDomain = attVal;
                      break;
                    case Launch:
                      launchType = attVal;
                      break;

                  }
                  break;

                case OfferImgId:
                  switch (attribute.key)
                  {
                    case Source:
                      imageUrl = attVal;
                      break;
                    case Domain:
                      imageDomain = attVal;
                      break;
                  }
                  break;
                
                case LandingPageUrl:
                  switch (attribute.key)
                  {
                    case Product:
                      product = attVal;
                      break;
                    case MobileWebUrl:
                      clickUrl = attVal;
                      break;
                    case CiCode:
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
          newAd.LaunchType = launchType;

          // Do not remove this check, added for backward compatability
          if (!string.IsNullOrEmpty(clickDomain) && !Uri.IsWellFormedUriString(clickUrl, UriKind.Absolute))
          {
            newAd.ClickUrl = string.Format(UrlFormat, clickDomain, clickUrl);
          }
          else
          {
            newAd.ClickUrl = clickUrl;
          }

          // Do not remove this check, added for backward compatability
          if(!string.IsNullOrEmpty(imageDomain) && !Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
          {
            newAd.ImageUrl = string.Format(UrlFormat, imageDomain, imageUrl);
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
