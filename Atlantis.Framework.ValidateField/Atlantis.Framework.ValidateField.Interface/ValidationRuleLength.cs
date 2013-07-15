﻿using System.Xml.Linq;

namespace Atlantis.Framework.ValidateField.Interface
{
  public class ValidationRuleLength : ValidationRuleElementBase<string>
  {
    public int MinimumLength { get; private set; }
    public int MaximumLength { get; private set; }

    public ValidationRuleLength(XElement ruleElement, string culture = "")
      : base(ruleElement, culture)
    {
      MinimumLength = 0;
      MaximumLength = int.MaxValue;

      XAttribute minAttribue = ruleElement.Attribute("min");
      int minValue;
      if ((minAttribue != null) && (int.TryParse(minAttribue.Value, out minValue)))
      {
        MinimumLength = minValue;
      }

      XAttribute maxAttribue = ruleElement.Attribute("max");
      int maxValue;
      if ((maxAttribue != null) && (int.TryParse(maxAttribue.Value, out maxValue)))
      {
        MaximumLength = maxValue;
      }

    }

    public override bool IsValid(string itemToValidate)
    {
      bool result = false;
      if (itemToValidate != null)
      {
        result = (itemToValidate.Length >= MinimumLength) && (itemToValidate.Length <= MaximumLength);
      }
      return result;
    }
  }
}
