using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BPSubscribeRemove.Interface
{
  public class BPSubscribeRemoveRequestData : RequestData
  {
    public string Email { get; private set; }

    public BPSubscribeRemoveRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string email)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Email = email;
    }

    public override string GetCacheMD5()
    {
      // The number of email address vs how many will be re-requested would
      // make caching this just take up memory (not a lot of cache-hits)
      throw new NotImplementedException("BPSubscribeRemove is not a cacheable request.");
    }
  }
}
