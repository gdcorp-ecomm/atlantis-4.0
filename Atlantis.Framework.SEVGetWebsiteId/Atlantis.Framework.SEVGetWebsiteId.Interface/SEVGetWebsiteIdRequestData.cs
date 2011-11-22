using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SEVGetWebsiteId.Interface
{
  public class SEVGetWebsiteIdRequestData : RequestData
  {

    public SEVGetWebsiteIdRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in SEVGetWebsiteIdRequestData");     
    }
  }
}
