﻿using System.Collections.Generic;
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
      get { throw new System.NotImplementedException(); }
    }

    public string RenewProhibitedPeriodForExpirationUnit
    {
      get { throw new System.NotImplementedException(); }
    }
  }
}
