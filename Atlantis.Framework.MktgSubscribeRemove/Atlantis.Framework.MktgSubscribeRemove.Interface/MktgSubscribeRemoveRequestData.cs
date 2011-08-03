using System;
using System.Net;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSubscribeRemove.Interface
{
  public class MktgSubscribeRemoveRequestData : RequestData
  {
    public string Email { get; private set; }
    public int PublicationId { get; private set; }
    public int PrivateLabelId { get; private set; }
    public string RequestedBy { get; private set; }
    string m_sIPAddress;

    public MktgSubscribeRemoveRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string email, int publicationId, int privateLabelId, string sRequestedBy)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Email = email;
      PublicationId = publicationId;
      PrivateLabelId = privateLabelId;
      RequestedBy = sRequestedBy;
      this.IPAddress = string.Empty;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public string IPAddress
    {
      get { return GetLocalAddress(); }
      set
      {
        m_sIPAddress = "";
        IPAddress address;
        if (System.Net.IPAddress.TryParse(value, out address))
          m_sIPAddress = address.ToString();
      }
    }

    string GetLocalAddress()
    {
      if (m_sIPAddress.Length == 0)
      {
        IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

        if (addresses.Length > 0)
          m_sIPAddress = addresses[0].ToString();
      }

      return m_sIPAddress;
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MktgSubscriberRemove is not a cacheable request.");
    }
  }
}
