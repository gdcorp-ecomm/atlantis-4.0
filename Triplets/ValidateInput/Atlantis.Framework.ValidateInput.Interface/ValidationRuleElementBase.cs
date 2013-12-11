using System.Xml.Linq;
using Atlantis.Framework.Providers.ValidateInput.Interface.ErrorCodes;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public abstract class ValidationRuleElementBase<T>
  {
    public int FailureCode { get; private set; }
    public abstract bool IsValid(T itemToValidate);

    protected ValidationRuleElementBase(XElement ruleElement)
    {
      var failureCodeAttribute = ruleElement.Attribute("failurecode");
      int failureCode;
      if ((failureCodeAttribute == null) || (!int.TryParse(failureCodeAttribute.Value, out failureCode)))
      {
        failureCode = PasswordErrorCodes.UnknownError;
      }

      FailureCode = failureCode;
    }
  }
}