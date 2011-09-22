using System.Runtime.Serialization;

namespace Atlantis.Framework.AuctionSearch.Interface
{
  [DataContract]
  public class RequestorInformation
  {
    /// <summary>
    /// – REQUIRED. Don’t pass in the IP address of the server making the request.  We limit how many queries are made per IP address to limit scripting.
    /// </summary>
    [DataMember]
    public string ExternalIpAddress { get; set; }

    /// <summary>
    /// – REQUIRED.  IP address of the server sending the request.
    /// </summary>
    [DataMember]
    public string RequestingServerIp { get; set; }

    /// <summary>
    /// – REQUIRED. IP address of the server sending the request.
    /// </summary>
    [DataMember]
    public string RequestingServerName { get; set; }

    /// <summary>
    /// - REQUIRED. Id of the source system making the request. Contact the auctions team for a source system id.
    /// </summary>
    [DataMember]
    public int SourceSystemId { get; set; }

    /// <summary>
    /// - REQUIRED. Determines if we will pass a shopper id to the service call
    /// </summary>
    public bool HasAuctionAccount { get; set; }
  }
}
