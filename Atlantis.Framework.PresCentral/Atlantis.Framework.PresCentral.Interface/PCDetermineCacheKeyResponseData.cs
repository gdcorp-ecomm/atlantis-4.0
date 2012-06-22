using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCDetermineCacheKeyResponseData : PCResponseDataBase
  {
    public PCDetermineCacheKeyResponseData(AtlantisException ex)
      : base(ex)
    {
    }

    public PCDetermineCacheKeyResponseData(PCResponse responseData)
      : base(responseData)
    {
    }
  }
}
