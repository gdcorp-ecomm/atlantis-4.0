using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.ValidateField.Interface
{
  public abstract class ValidationRuleElementBase<T>
  {
    public ValidationFailure FailureInfo { get; private set; }
    public abstract bool IsValid(T itemToValidate);

    public ValidationRuleElementBase(XElement ruleElement)
    {
      XAttribute failureCodeAttribute = ruleElement.Attribute("failurecode");
      int failureCode;
      if ((failureCodeAttribute == null) || (!int.TryParse(failureCodeAttribute.Value, out failureCode)))
      {
        failureCode = -1;
      }

      string description = "Unknown failure.";
      XElement descriptionElement = ruleElement.Descendants("description").FirstOrDefault();
      if ((descriptionElement != null) && (!string.IsNullOrEmpty(descriptionElement.Value)))
      {
        description = descriptionElement.Value;
      }

      FailureInfo = new ValidationFailure(failureCode, description);
    }
   
  }
}
