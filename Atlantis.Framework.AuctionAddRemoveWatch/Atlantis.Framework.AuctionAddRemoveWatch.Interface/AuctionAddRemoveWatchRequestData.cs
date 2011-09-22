using System;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionAddRemoveWatch.Interface
{
  public class AuctionAddRemoveWatchRequestData: RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public string RequestXml { get; private set; }

    
    public AuctionAddRemoveWatchRequestData(string shopperId, 
                                        string sourceURL, 
                                        string orderId, 
                                        string pathway, 
                                        int pageCount,
                                        string externalIpAddress,
                                        string requestingServerIp,
                                        string requestingServerName, 
                                        string auctionId,
                                        bool isAddWatch,
                                        string watchType) 
                                        : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestXml = BuildXmlRequest(externalIpAddress, requestingServerIp, requestingServerName, shopperId, auctionId, isAddWatch, watchType);
      RequestTimeout = _requestTimeout;
    }

    private string BuildXmlRequest(string externalIpAddress, string requestingServerIp, string requestingServerName, string shopperId, string auctionId, bool isAddWatch, string watchType)
    {
      var sb = new StringBuilder();

      externalIpAddress = string.Format("ExternalIPAddress='{0}'", externalIpAddress);
      requestingServerIp = string.Format("RequestingServerIP='{0}'", requestingServerIp);
      requestingServerName = string.Format("RequestingServerName='{0}'", requestingServerName);
      shopperId = string.Format("ShopperId='{0}'", shopperId);
      auctionId = string.Format("AuctionId='{0}'", auctionId);
      string addWatch = string.Format("AddWatch='{0}'", isAddWatch.ToString().ToLower());
      watchType = string.Format("WatchType='{0}'", watchType);

      sb.AppendFormat("<AddRemoveWatch {0}  {1}  {2} {3} {4} {5} {6} />", externalIpAddress, requestingServerIp, requestingServerName, shopperId, auctionId, addWatch, watchType);

      return sb.ToString();
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionAddRemoveWatch is not a cacheable request.");
    }

    #endregion
  }
}
