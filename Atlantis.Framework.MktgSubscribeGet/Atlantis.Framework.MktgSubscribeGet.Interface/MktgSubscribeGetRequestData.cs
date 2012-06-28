using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSubscribeGet.Interface
{
  public class MktgSubscribeGetRequestData : RequestData
  {
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(4);

    public string Email { get; private set; }

    public int PrivateLabelId { get; private set; }

    public MktgSubscribeGetRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string email, int privateLabelId) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Email = email;
      PrivateLabelId = privateLabelId;

      RequestTimeout = _defaultTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MktgSubscribeGet is not a cacheable request.");
    }
  }
}
