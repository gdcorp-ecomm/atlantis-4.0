using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GoogleClientAuth.Interface
{
  public class GoogleClientAuthRequestData : RequestData
  {
    public string ServiceType { get; private set; }

    public string AccountType { get; private set; }

    public string SourceApplication { get; private set; }

    public GoogleClientAuthRequestData(string serviceType, string accountType, string sourceApplication, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ServiceType = serviceType;
      AccountType = accountType;
      SourceApplication = sourceApplication;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("GoogleClientAuth is not a cacheable request");
    }
  }
}
