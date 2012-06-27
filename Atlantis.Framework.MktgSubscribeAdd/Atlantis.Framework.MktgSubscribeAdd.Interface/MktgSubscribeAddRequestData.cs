using System;
using System.Net;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSubscribeAdd.Interface
{
  public class MktgSubscribeAddRequestData : RequestData
  {
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(8);

    public string Email { get; private set; }

    public int PublicationId { get; private set; }

    public int PrivateLabelId { get; private set; }

    public int EmailType { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string RequestedBy { get; private set; }

    public bool Confirmed { get; private set; }

    public string IpAddress { get; private set; }

    [Obsolete("Use the constructor that passes in the ip address as a parameter.")]
    public MktgSubscribeAddRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string email, int publicationId, int privateLabelId, int emailType, string firstName, string lastName, bool confirmed, string requestedBy)
                                : this(shopperId, sourceUrl, orderId, pathway, pageCount, email, publicationId, privateLabelId, emailType, firstName, lastName, confirmed, GetLocalAddress(), requestedBy)
    {
    }

    public MktgSubscribeAddRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string email, int publicationId, int privateLabelId, int emailType, string firstName, string lastName, bool confirmed, string ipAddress, string requestedBy)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Email = email;
      PublicationId = publicationId;
      PrivateLabelId = privateLabelId;
      EmailType = emailType;
      FirstName = firstName;
      LastName = lastName;
      Confirmed = confirmed;
      RequestedBy = requestedBy;
      IpAddress = ipAddress;
      RequestTimeout = _defaultTimeout;
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
      throw new NotImplementedException("MktgSubscriberAdd is not a cacheable request.");
    }
  }
}
