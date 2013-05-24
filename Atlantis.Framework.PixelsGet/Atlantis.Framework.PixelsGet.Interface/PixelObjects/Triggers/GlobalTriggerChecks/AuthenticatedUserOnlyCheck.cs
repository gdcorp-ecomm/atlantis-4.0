using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.PixelsGet.Interface.Constants;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers.GlobalTriggerChecks
{
  public class AuthenticatedUserOnlyCheck : GlobalTriggerCheck
  {
    private readonly bool _isAuthenticated;

    public AuthenticatedUserOnlyCheck(bool isAuthenticated)
    {
      this._isAuthenticated = isAuthenticated;
    }

    public override bool CeaseFiringTrigger(string actualValue)
    {
      bool returnValue = false;

      bool parsed = false;
      if (bool.TryParse(actualValue, out parsed))
      {
        bool isMatchOrActualValueFalse = _isAuthenticated == parsed || parsed == false;

        if (!isMatchOrActualValueFalse)
        {
          returnValue = true;
        }
      }

      return returnValue;
    }

    public override string PixelXmlName
    {
      get { return PixelXmlNames.TriggerConditionAuthenticatedOnly; }
    }
  }
}
