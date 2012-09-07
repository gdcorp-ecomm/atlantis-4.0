using System.Collections.Generic;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public abstract class RuleContainer
  {
    public HashSet<ValidationRule> RulesToValidate {get;set;}
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }

    public RuleContainer()
    {
      ErrorMessage = string.Empty;
      RulesToValidate = new HashSet<ValidationRule>();
    }

    public void Validate()
    {
      ShopperRuleValidator validator = new ShopperRuleValidator();
      validator.ValidateOneRule(this);
    }
  }
}
