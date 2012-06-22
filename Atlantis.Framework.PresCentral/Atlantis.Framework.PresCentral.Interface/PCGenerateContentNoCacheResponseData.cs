using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCGenerateContentNoCacheResponseData : PCResponseDataBase
  {
    public PCGenerateContentNoCacheResponseData(AtlantisException ex)
      : base(ex)
    {
    }

    public PCGenerateContentNoCacheResponseData(PCResponse responseData)
      : base(responseData)
    {
    }
  }
}
