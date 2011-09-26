using System;
using System.Xml.Linq;
using Atlantis.Framework.AuctionSearch.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionSearchGetSaved.Interface
{
  public class AuctionSearchGetSavedRequestData : RequestData
  {
    TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);

    public string RequestXml
    {
      get { return BuildXmlRequest(); }
    }

    /// <summary>
    /// The requestorInformation and shopperId are required and 
    /// </summary>
    /// <param name="requestorInformation"></param>
    /// <param name="shopperId"></param>
    /// <param name="sourceUrl"></param>
    /// <param name="orderId"></param>
    /// <param name="pathway"></param>
    /// <param name="pageCount"></param>
    public AuctionSearchGetSavedRequestData(RequestorInformation requestorInformation, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestorInformation = requestorInformation;
      RequestTimeout = _requestTimeout;
    }

    public RequestorInformation RequestorInformation { get; private set; }

    string BuildXmlRequest()
    {
      XElement savedSearchRoot;
      if (IsRequestValid())
      {
        savedSearchRoot = new XElement("GetMemberSavedSearches");
        savedSearchRoot.SetAttributeValue("ExternalIPAddress", RequestorInformation.ExternalIpAddress);
        savedSearchRoot.SetAttributeValue("RequestingServerIP", RequestorInformation.RequestingServerIp);
        savedSearchRoot.SetAttributeValue("RequestingServerName", RequestorInformation.RequestingServerName);
        savedSearchRoot.SetAttributeValue("SourceSystemId", RequestorInformation.SourceSystemId);
        savedSearchRoot.SetAttributeValue("shopperId", ShopperID);
      }
      else
      {
        throw new ArgumentException("Request Validation Exception", "RequestInformation property or one it's values are not valid.");
      }

      return savedSearchRoot.ToString();
    }

    bool IsRequestValid()
    {
      var isvalid = true;

      if (RequestorInformation != null)
      {
        if (string.IsNullOrEmpty(ShopperID))
        {
          isvalid = false;
        }

        if (string.IsNullOrEmpty(RequestorInformation.ExternalIpAddress))
        {
          isvalid = false;
        }

        if (string.IsNullOrEmpty(RequestorInformation.RequestingServerIp))
        {
          isvalid = false;
        }

        if (string.IsNullOrEmpty(RequestorInformation.RequestingServerName))
        {
          isvalid = false;
        }
      }
      else
      {
        isvalid = false;
      }

      return isvalid;
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in AuctionSearchGetSavedRequestData");
    }

  }
}
