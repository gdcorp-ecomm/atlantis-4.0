using System.Collections.Generic;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class ShopperRuleValidator
  {
    private IEnumerable<RuleContainer> _validationRules;

    public ShopperRuleValidator(IEnumerable<RuleContainer> validationRules)
    {
      _validationRules = validationRules;
    }

    public ShopperRuleValidator(){}

    public void ValidateAllRules()
    {
      foreach (RuleContainer ruleContainer in _validationRules)
      {
        ValidateRuleContainer(ruleContainer);
      }
    }

    public void ValidateOneRule(RuleContainer ruleContainer)
    {
      ValidateRuleContainer(ruleContainer);
    }

    private void ValidateRuleContainer(RuleContainer ruleContainer)
    {
      foreach (ValidationRule rule in ruleContainer.RulesToValidate)
      {
        rule.Validate();
        ruleContainer.IsValid = rule.IsValid;
        ruleContainer.RuleName = rule.RuleName;
        if (!ruleContainer.IsValid)
        {
          ruleContainer.ErrorMessage = rule.ErrorMessage;
          break;
        }
      }
    }
  }
}
