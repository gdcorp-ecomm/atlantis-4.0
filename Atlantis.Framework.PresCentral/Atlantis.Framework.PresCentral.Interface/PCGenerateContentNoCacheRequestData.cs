using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCGenerateContentNoCacheRequestData : PCRequestDataBase
  {
    public PCGenerateContentNoCacheRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    { }

    public override IResponseData CreateResponse(PCResponse responseData)
    {
      return new PCGenerateContentNoCacheResponseData(responseData);
    }

    public override IResponseData CreateResponse(AtlantisException ex)
    {
      return new PCGenerateContentNoCacheResponseData(ex);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("PCGenerateContentNoCache is not a cacheable request.");
    }
  }
}
