using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class GeneralEoiJsonRequestData : RequestData
  {
    public GeneralEoiJsonRequestData()
    {
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string GetCacheMD5()
    {
      return "GeneralEoiJsonRequest";
    }
  }
}
