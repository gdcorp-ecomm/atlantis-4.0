
using System;
using Atlantis.Framework.PixelsGet.Interface.Constants;

namespace Atlantis.Framework.PixelsGet.Interface.PixelObjects.Triggers.GlobalTriggerChecks
{
  class FirstTimeOnlyShopperCheck : GlobalTriggerCheck
  {
    private readonly bool _isRequestFirstTimeShopper;
    public FirstTimeOnlyShopperCheck(bool isRequestFirstTimeShopper)
    {
      _isRequestFirstTimeShopper = isRequestFirstTimeShopper;
    }

    public override bool CeaseFiringTrigger(string attributeValue)
    {
      bool ceaseFire = false;
      bool parsedAttributeValue;

      if (bool.TryParse(attributeValue, out parsedAttributeValue))
      {
        bool isMatchOrActualValueFalse = _isRequestFirstTimeShopper == parsedAttributeValue || parsedAttributeValue == false;

        if (!isMatchOrActualValueFalse)
        {
          ceaseFire = true;
        }

      }

      return ceaseFire;
    }

    public override string PixelXmlName
    {
      get { return PixelXmlNames.TriggerConditionFirstTime; }
    }
  }
}
