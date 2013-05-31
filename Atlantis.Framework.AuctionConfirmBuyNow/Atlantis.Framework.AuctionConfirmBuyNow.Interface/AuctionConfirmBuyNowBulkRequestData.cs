using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionConfirmBuyNow.Interface
{
  public class AuctionConfirmBuyNowBulkRequestData : RequestData
  {
    public AuctionConfirmBuyNowBulkRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      int sourceSystemId, string externalIpAddress, string requestingServerIp, string requestingServerName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      SourceSystemId = sourceSystemId;
      ExternalIpAddress = externalIpAddress;
      RequestingServerIp = requestingServerIp;
      RequestingServerName = requestingServerName;
      RequestTimeout = _requestTimeout;
      _buyNowDomains = new List<ConfirmBuyNow>();
    }

    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    public int SourceSystemId { get; set; }
    public string ExternalIpAddress { get; set; }
    public string RequestingServerIp { get; set; }
    public string RequestingServerName { get; set; }
    private List<ConfirmBuyNow> _buyNowDomains { get; set; }

    /// <summary>
    /// Add Buy Now auctions to the list that will be added to the Cart
    /// </summary>
    /// <param name="auctionId">If known.  Is used over the domainName.</param>
    /// <param name="domainName">auctionId or domainName must be passed.</param>
    /// <param name="bidderShopperId">ShopperId of the buyer - required.</param>
    /// <param name="bidAmount">Whole dollar amount of the domain being purchased - required.</param>
    /// <param name="comments">Optional - can be empty string.</param>
    /// <param name="isc">Optional - can be empty string.</param>
    /// <param name="itc">Optional - can be empty string.</param>
    public void AddBuyNowDomain(string auctionId, string domainName, string bidderShopperId, string comments, string isc, string itc)
    {
      _buyNowDomains.Add(new ConfirmBuyNow(auctionId, domainName, bidderShopperId, comments, isc, itc));
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("AuctionConfirmBuyNowBulkRequestData is not a cachable Request");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("BulkBuyNow");
      xtwRequest.WriteAttributeString("SourceSystemId", this.SourceSystemId.ToString());
      xtwRequest.WriteAttributeString("ExternalIpAddress", this.ExternalIpAddress);
      xtwRequest.WriteAttributeString("RequestingServerIp", this.RequestingServerIp);
      xtwRequest.WriteAttributeString("RequestingServerName", this.RequestingServerName);

      if (_buyNowDomains.Count > 0)
      {
        foreach (ConfirmBuyNow item in _buyNowDomains)
        {
          xtwRequest.WriteStartElement("ConfirmBuyNow");
          if (string.IsNullOrEmpty(item.AuctionId) && !string.IsNullOrEmpty(item.DomainName))
          {
            xtwRequest.WriteAttributeString("DomainName", item.DomainName);
          }
          else
          {
            xtwRequest.WriteAttributeString("AuctionId", item.AuctionId);
          }
          xtwRequest.WriteAttributeString("BidderShopperId", item.BidderShopperId);
          if (!string.IsNullOrEmpty(item.Comments))
          {
            xtwRequest.WriteAttributeString("Comments", item.Comments);
          }
          if (!string.IsNullOrEmpty(item.Isc))
          {
            xtwRequest.WriteAttributeString("Isc", item.Isc);
          }
          if (!string.IsNullOrEmpty(item.Itc))
          {
            xtwRequest.WriteAttributeString("Itc", item.Itc);
          }
          xtwRequest.WriteEndElement();
        }
      }

      xtwRequest.WriteEndElement(); //BulkBuyNow

      return sbRequest.ToString();
    }
  }
}