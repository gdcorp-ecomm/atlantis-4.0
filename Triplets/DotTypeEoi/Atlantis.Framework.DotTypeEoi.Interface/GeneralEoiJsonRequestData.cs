using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class GeneralEoiJsonRequestData : RequestData
  {
    public GeneralEoiJsonRequestData()
    {
    }

    public override string GetCacheMD5()
    {
      return "GetGeneralEoiJsonRequest";
    }
  }
}
