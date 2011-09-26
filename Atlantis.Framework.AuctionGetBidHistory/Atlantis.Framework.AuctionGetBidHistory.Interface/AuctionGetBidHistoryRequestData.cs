using System;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionGetBidHistory.Interface
{
  public class AuctionGetBidHistoryRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    
    public string RequestXml { get; private set; }

    public string AuctionItemId { get; private set; }

    public bool IsMemberArea { get; private set; }
    
    public AuctionGetBidHistoryRequestData(string shopperId, 
                                           string sourceURL, 
                                           string orderId, 
                                           string pathway, 
                                           int pageCount, 
                                           string externalIpAddress, 
                                           string requestingServerIp, 
                                           string requestingServerName, 
                                           string auctionItemId, 
                                           bool isMemberArea) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestXml = BuildXmlRequest(externalIpAddress, requestingServerIp, requestingServerName, auctionItemId, isMemberArea, shopperId);
      AuctionItemId = auctionItemId;
      IsMemberArea = isMemberArea;
      RequestTimeout = _requestTimeout;
    }

    private string BuildXmlRequest(string externalIpAddress, string requestingServerIp, string requestingServerName, string auctionItemId, bool isMemberArea, string shopperId)
    {
      var sb = new StringBuilder();
      
      string extIp = string.Format("ExternalIPAddress='{0}'", externalIpAddress);
      string serverIp = string.Format("RequestingServerIP='{0}'", requestingServerIp);
      string serverName = string.Format("RequestingServerName='{0}'", requestingServerName);
      string fAuctionId = string.Format("AuctionId='{0}'", auctionItemId);
      string fIsMemberArea = string.Format("IsMemberArea='{0}'", isMemberArea);
      string fShopperId = string.Format("ShopperId='{0}'", shopperId);

      sb.AppendFormat("<GetBidHistory {0}  {1}  {2}  {3}  {4}  {5}>", extIp, serverIp, serverName, fAuctionId, fIsMemberArea, fShopperId);
      
      sb.AppendFormat("</GetBidHistory>");

      return sb.ToString();
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionGetBidHistory is not a cacheable request.");
    }

    #endregion
  }
}
