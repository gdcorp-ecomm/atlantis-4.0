using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCGenerateContentNoCacheRequestData : PCRequestDataBase
  {
    public override IResponseData CreateResponse(PCResponse responseData)
    {
      return new PCGenerateContentNoCacheResponseData(responseData);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("PCGenerateContentNoCache is not a cacheable request.");
    }
  }
}
