using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.EcommInstoreAccept.Interface
{
  public class EcommInstoreAcceptRequestData : RequestData
  {
    public EcommInstoreAcceptRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("EcommInstoreAccept is not a cacheable request.");
    }
  }
}
