using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCGenerateContentRequestData : PCRequestDataBase
  {
    string _cacheKey;

    public PCGenerateContentRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string cacheKey)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      if (string.IsNullOrEmpty(cacheKey))
      {
        throw new ArgumentNullException("CacheKey cannot be null or empty. Please use cache key returned by PCDetermineCacheKeyRequestData");
      }

      _cacheKey = cacheKey;
    }

    public override IResponseData CreateResponse(PCResponse responseData)
    {
      return new PCGenerateContentResponseData(responseData);
    }

    public override IResponseData CreateResponse(AtlantisException ex)
    {
      return new PCGenerateContentResponseData(ex);
    }

    public override string GetCacheMD5()
    {
      return _cacheKey;
    }
  }
}
