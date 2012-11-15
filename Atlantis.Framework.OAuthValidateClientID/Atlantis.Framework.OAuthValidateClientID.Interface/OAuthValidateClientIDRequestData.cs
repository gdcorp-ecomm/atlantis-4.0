using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OAuthValidateClientId.Interface
{
  public class OAuthValidateClientIdRequestData: RequestData
  {
    public string ClientId { get; private set; }
    public string ApplicationId { get; private set; }
    public string IpAddress { get; private set; }
    public string Host { get; private set; }

    public OAuthValidateClientIdRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, 
      string clientId, string applicationId, string ipAddress, string host):
      base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ClientId = clientId;
      ApplicationId = applicationId;
      IpAddress = ipAddress;
      Host = host;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
