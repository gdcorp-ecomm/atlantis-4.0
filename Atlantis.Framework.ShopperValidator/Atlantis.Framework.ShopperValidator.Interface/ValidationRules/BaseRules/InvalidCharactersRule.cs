﻿using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class InvalidCharactersRule : RegexRule
  {
    private const bool _IS_VALID_IF_REGEX_DOESNT_MATCH = true;

    public InvalidCharactersRule(string fieldName, string textToValidate, string culture = "")
      : base(fieldName, textToValidate, RegexConstants.InvalidCharacters, _IS_VALID_IF_REGEX_DOESNT_MATCH, culture)
    {
      base.ErrorMessage = string.Format(FetchResource.GetString("invalidCharacters"), fieldName);
    }
    
    public override void Validate()
    {
      base.Validate();
      this.IsValid = base.IsValid;
      base.RuleName = "InvalidCharactersRule";
    }
  }

}
