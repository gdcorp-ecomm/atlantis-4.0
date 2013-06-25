using System.Collections.Generic;
using System.IO;
using System.Xml;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public class StaticTld : ITLDTld
  {
    private readonly StaticDotType _staticDotType;
    public StaticTld(StaticDotType staticDotType)
    {
      _staticDotType = staticDotType;
    }

    public int RenewProhibitedPeriodForExpiration
    {
      get
      {
        return 0;               //This is not be used since MaxRenewalMonthsOut is used for statics
      }
    }

    public string RenewProhibitedPeriodForExpirationUnit
    {
      get
      {
        return string.Empty;    //This is not be used since MaxRenewalMonthsOut is used for statics
      }
    }

    public bool IsGtld 
    {
      get
      {
        return false;
      }
    }
  }
}
