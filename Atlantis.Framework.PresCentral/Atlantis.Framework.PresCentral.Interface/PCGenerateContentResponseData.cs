using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PresCentral.Interface
{
  public class PCGenerateContentResponseData : PCResponseDataBase
  {
    public PCGenerateContentResponseData(AtlantisException ex)
      : base(ex)
    {
    }

    public PCGenerateContentResponseData(PCResponse responseData)
      : base(responseData)
    {
    }
  }
}
