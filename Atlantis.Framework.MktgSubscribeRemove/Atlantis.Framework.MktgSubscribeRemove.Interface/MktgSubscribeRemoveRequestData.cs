using System;
using System.Net;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSubscribeRemove.Interface
{
  public class MktgSubscribeRemoveRequestData : RequestData
  {
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(6);

    public string Email { get; private set; }

    public int PublicationId { get; private set; }

    public int PrivateLabelId { get; private set; }

    public string RequestedBy { get; private set; }

    public string IpAddress { get; private set; }

    public MktgSubscribeRemoveRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string email, int publicationId, int privateLabelId, string ipAddress, string requestedBy) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Email = email;
      PublicationId = publicationId;
      PrivateLabelId = privateLabelId;
      RequestedBy = requestedBy;
      IpAddress = ipAddress;
      RequestTimeout = _defaultTimeout;
    }

    [Obsolete("Use the constructor that has ip address as a parameter")]
    public MktgSubscribeRemoveRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string email, int publicationId, int privateLabelId, string requestedBy)
                                   : this(shopperId, sourceUrl, orderId, pathway, pageCount, email, publicationId, privateLabelId, GetLocalAddress(), requestedBy)
    {
      
    }

    private static string GetLocalAddress()
    {
      string ipAddress = "127.0.0.1";

      IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

      if (addresses.Length > 0)
      {
        ipAddress = addresses[0].ToString();
      }

      return ipAddress;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MktgSubscriberRemove is not a cacheable request.");
    }
  }
}
