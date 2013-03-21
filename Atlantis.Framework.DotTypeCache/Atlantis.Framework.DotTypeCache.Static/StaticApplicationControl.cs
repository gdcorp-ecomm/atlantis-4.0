using System.Collections.Generic;
using System.IO;
using System.Xml;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public class StaticApplicationControl : ITLDApplicationControl
  {
    private readonly StaticDotType _staticDotType;
    public StaticApplicationControl(StaticDotType staticDotType)
    {
      _staticDotType = staticDotType;
    }

    public string DotTypeDescription
    {
      get
      {
        return StaticDotTypes.GetAdditionalInfoValue(_staticDotType.DotType, "Description");
      }
    }

    public string LandingPageUrl
    {
      get
      {
        return StaticDotTypes.GetAdditionalInfoValue(_staticDotType.DotType, "LandingPageUrl");
      }
    }

    public bool IsMultiRegistry
    {
      get
      {
        return StaticDotTypes.IsDotTypeMultiRegistry(_staticDotType.DotType);
      } 
    }
  }
}
