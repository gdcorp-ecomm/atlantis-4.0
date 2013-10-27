using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCDetermineCacheKeyRequestData : PCRequestDataBase
  {
    public override IResponseData CreateResponse(PCResponse responseData)
    {
      return new PCDetermineCacheKeyResponseData(responseData);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(GetQuery());
    }
  }
}
