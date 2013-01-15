using Atlantis.Framework.BazaarVoiceAPI.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.BazaarVoiceAPI.Impl
{
  public class ReviewsAPIRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData result = null;

      ReviewsAPIRequestData reviewsAPIRequestData = (ReviewsAPIRequestData)requestData;
      WsConfigElement wsConfig = (WsConfigElement)configElement;
      string apiversion = wsConfig.GetConfigValue("ApiVersion");
      string passkey = wsConfig.GetConfigValue("PassKey");
      string queryString = reviewsAPIRequestData.GetQuery();
      string reqeustUrl = wsConfig.WSURL + "?apiversion=" + apiversion + "&passkey=" + passkey + "&" + queryString;

      try
      {
        XNamespace dataApiQuery = "http://www.bazaarvoice.com/xs/DataApiQuery/" + apiversion;
        XDocument reviewData = XDocument.Load(reqeustUrl);
        List<Review> reviewList = new List<Review>(100);

        var xElement = reviewData.Descendants(dataApiQuery + "Review");
        if (xElement != null)
        {
          var reviews = reviewData.Descendants(dataApiQuery + "Review");
          foreach (XElement reviewItem in reviews)
          {
            Review newReview = Review.FromBazaarApiElement(reviewItem, dataApiQuery, requestData);
            if (newReview.IsValid)
            {
              reviewList.Add(newReview);
            }
          }
        }

        if (reviewList.Count == 0)
        {
          reviewList = null;
        }
         
        result = new ReviewsAPIResponseData(reviewList);

      }
      catch (Exception ex)
      {
        result = new ReviewsAPIResponseData(requestData, ex);
      }

      return result;
    }
  }
}
