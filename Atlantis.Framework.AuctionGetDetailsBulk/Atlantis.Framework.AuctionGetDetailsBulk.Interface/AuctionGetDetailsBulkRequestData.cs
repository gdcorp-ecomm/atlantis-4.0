using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionGetDetailsBulk.Interface
{
  public class AuctionGetDetailsBulkRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public string RequestXml { get; private set; }
    
    public AuctionGetDetailsBulkRequestData(string shopperId, 
                                            string sourceURL, 
                                            string orderId, 
                                            string pathway, 
                                            int pageCount,
                                            string externalIpAddress,
                                            string requestingServerIp,
                                            string requestingServerName,
                                            IEnumerable<string> auctionsToGet) 
                                            : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestXml = BuildXmlRequest(externalIpAddress, requestingServerIp, requestingServerName, auctionsToGet);
      RequestTimeout = _requestTimeout;
    }

    private string BuildXmlRequest(string externalIpAddress, string requestingServerIp, string requestingServerName, IEnumerable<string> auctionsToGet)
    {
      var sb = new StringBuilder();

      string extIp = string.Format("ExternalIPAddress='{0}'", externalIpAddress);
      string serverIp = string.Format("RequestingServerIP='{0}'", requestingServerIp);
      string serverName = string.Format("RequestingServerName='{0}'", requestingServerName);

      sb.AppendFormat("<AuctionDetailBulk {0}  {1}  {2}>", extIp, serverIp, serverName);

      if (auctionsToGet != null)
      {
        string ShopperIDString = string.IsNullOrEmpty(ShopperID)
                                   ? string.Empty
                                   : string.Format("ShopperId='{0}'", ShopperID);

        foreach (string auction in auctionsToGet)
        {
          sb.AppendFormat("<AuctionDetail auctionId='{0}' {1} />", auction, ShopperIDString);
        }
      }

      sb.AppendFormat("</AuctionDetailBulk>");

      return sb.ToString();
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("AuctionGetDetailsBulk is not a cacheable request.");
    }

    #endregion
  }
}
