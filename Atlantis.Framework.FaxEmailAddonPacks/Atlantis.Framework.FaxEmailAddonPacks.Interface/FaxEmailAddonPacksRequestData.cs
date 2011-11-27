using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FaxEmailAddonPacks.Interface
{
  public class FaxEmailAddonPacksRequestData : RequestData
  {
    public int FteResourceId { get; private set; }

    public FaxEmailAddonPacksRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int fteResourceId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      FteResourceId = fteResourceId;
      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in FaxEmailAddonPacksRequestData");
    }
  }
}
