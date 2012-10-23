using Atlantis.Framework.ShopperValidator.Interface.RuleConstants;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;
using System;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public class CallInPinRule : SingleValueRuleContainer
  {
    private string _callInPin;
    private string _fieldName;

    public CallInPinRule(string value, string fieldName = FieldNames.CallInPin, bool isRequired = false)
      : base(value, fieldName, isRequired)
    {
      _callInPin = value;
      _fieldName = fieldName;

      base.RulesToValidate.Add(new MaxLengthRule(fieldName, value, LengthConstants.CallInPinMaxLength));
      base.RulesToValidate.Add(new MinLengthRule(fieldName, value, LengthConstants.CallInPinMinLength));
      base.RulesToValidate.Add(new NumericRule(fieldName, value));

      BuildCustomRules();
    }

    private void BuildCustomRules()
    {
      int callInPin = 0;
      if (int.TryParse(_callInPin, out callInPin))
      {
        string forward = "0123456789012";
        string backward = "0987654321098";
        int firstNumber = Convert.ToInt16(_callInPin.Substring(0, 1));
        
        if (firstNumber == 0) { firstNumber = 1; }  //prevents division by zero

        //cannot be same four digits or in any numerically ascending/descending order 
        bool isCallInPinSameFour = callInPin == 0 || (callInPin / firstNumber == 1111);
        bool isCallInPinSequential = forward.Contains(_callInPin) || backward.Contains(_callInPin);

        if (isCallInPinSequential || isCallInPinSameFour)
        {
          base.RulesToValidate.Add(new BlankRule(false, string.Concat(_fieldName, " cannot be any straight numerical sequence of digits.")));
        }
      }
    }
  }
}
