using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.AuctionSearch.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionPlaceBidBulk.Interface
{
  public class AuctionPlaceBidBulkRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public string RequestXml { get; private set; }

    public AuctionPlaceBidBulkRequestData(RequestorInformation requestorInformation,
                                          List<AuctionPlaceBidItem> bidsToPlace,
                                          string shopperId,
                                          string sourceUrl,
                                          string orderId,
                                          string pathway,
                                          int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestXml = BuildXmlRequest(requestorInformation.SourceSystemId, requestorInformation.ExternalIpAddress, requestorInformation.RequestingServerIp, requestorInformation.RequestingServerName, bidsToPlace);
      RequestTimeout = _requestTimeout;
    }

    private static string BuildXmlRequest(int sourceSystem, string externalIpAddress, string requestingServerIp, string requestingServerName, List<AuctionPlaceBidItem> bidsToPlace)
    {
      var sb = new StringBuilder();

      string sourceSys = string.Format("SourceSystemId='{0}'", sourceSystem);
      string extIp = string.Format("ExternalIPAddress='{0}'", externalIpAddress);
      string serverIp = string.Format("RequestingServerIP='{0}'", requestingServerIp);
      string serverName = string.Format("RequestingServerName='{0}'", requestingServerName);

      sb.AppendFormat("<BulkBids {0}  {1}  {2}  {3} >", sourceSys, extIp, serverIp, serverName);

      if (bidsToPlace != null)
      {
        foreach (AuctionPlaceBidItem bidItem in bidsToPlace)
        {
          string auctionId = string.Format("AuctionId='{0}'", bidItem.AuctionItemId);
          string bidderShopperId = string.Format("BidderShopperId='{0}'", bidItem.BidderShopperId);
          string bidAmount = string.Format("bidAmount='{0}'", bidItem.BidAmount);
          string comments = string.Format("comments='{0}'", bidItem.Comments);

          sb.AppendFormat("<Bid {0}  {1}  {2}  {3} />", auctionId, bidderShopperId, bidAmount, comments);
        }
      }

      sb.AppendFormat("</BulkBids>");

      return sb.ToString();
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionPlaceBidBulk is not a cacheable request.");
    }

    #endregion
  }
}
