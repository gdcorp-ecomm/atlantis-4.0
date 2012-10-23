using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public abstract class SingleValueRuleContainer : RuleContainer
  {
    public string Value { get; set; }
    public string FieldName { get; set; }
    public bool IsRequired { get; set; }

    public SingleValueRuleContainer(string value, string fieldName, bool isRequired)
      : base()
    {
      Value = value;
      FieldName = fieldName;
      IsRequired = isRequired;

      if(IsRequired)
      {
        base.RulesToValidate.Add(new RequiredRule(FieldName, Value));
      }
    }
  }
}
