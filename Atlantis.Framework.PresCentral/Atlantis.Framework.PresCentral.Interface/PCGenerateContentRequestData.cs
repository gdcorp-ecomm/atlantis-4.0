using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCGenerateContentRequestData : PCRequestDataBase
  {
    readonly string _cacheKey;

    public PCGenerateContentRequestData(string cacheKey)
    {
      if (string.IsNullOrEmpty(cacheKey))
      {
        throw new ArgumentException("cacheKey cannot be null or empty. Please use cacheKey returned by PCDetermineCacheKeyRequestData");
      }

      _cacheKey = cacheKey;
    }

    public override IResponseData CreateResponse(PCResponse responseData)
    {
      return new PCGenerateContentResponseData(responseData);
    }

    public override string GetCacheMD5()
    {
      return _cacheKey;
    }
  }
}
