﻿using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class XssRule: RegexRule
  {
    private const bool _IS_VALID_IF_REGEX_DOESNT_MATCH = true;

    public XssRule(string fieldName, string textToValidate)
      :base(fieldName, textToValidate, RegexConstants.InvalidXssTags, _IS_VALID_IF_REGEX_DOESNT_MATCH)
    {
      base.ErrorMessage = string.Concat(fieldName, " contains invalid characters");
    }

    public override void Validate()
    {
      base.Validate();
      this.IsValid = base.IsValid;
    }
  }

}
