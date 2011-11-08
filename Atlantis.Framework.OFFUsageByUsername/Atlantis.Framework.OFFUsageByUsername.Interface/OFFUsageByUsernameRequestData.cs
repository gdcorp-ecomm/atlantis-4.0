using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OFFUsageByUsername.Interface
{
  public class OFFUsageByUsernameRequestData : RequestData
  {
    public string Username { get; private set; }

    public OFFUsageByUsernameRequestData(string shopperId, string sourceUrl, string orderId,
                                         string pathway, int pageCount, string username)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Username = username;
      RequestTimeout = TimeSpan.FromSeconds(15d);
    }

    public override string GetCacheMD5()
    {
      return string.Empty;
    }
  }
}
