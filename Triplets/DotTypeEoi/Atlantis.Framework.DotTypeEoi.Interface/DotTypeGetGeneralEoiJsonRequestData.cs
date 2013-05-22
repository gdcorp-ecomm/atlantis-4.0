using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class DotTypeGetGeneralEoiJsonRequestData : RequestData
  {
    public DotTypeGetGeneralEoiJsonRequestData()
    {
    }

    public override string GetCacheMD5()
    {
      return "GetGeneralEoiJsonRequest";
    }
  }
}
