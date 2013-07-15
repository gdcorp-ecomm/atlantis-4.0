using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.ValidateField.Interface.LanguageResources;

namespace Atlantis.Framework.ValidateField.Interface
{
  public abstract class ValidationRuleElementBase<T>
  {
    public ValidationFailure FailureInfo { get; private set; }
    public abstract bool IsValid(T itemToValidate);

    public ValidationRuleElementBase(XElement ruleElement, string culture = "")
    {
      XAttribute failureCodeAttribute = ruleElement.Attribute("failurecode");
      int failureCode;
      if ((failureCodeAttribute == null) || (!int.TryParse(failureCodeAttribute.Value, out failureCode)))
      {
        failureCode = -1;
      }

      string description;
      using (var fetchResource = new FetchResource("Atlantis.Framework.ValidateField.Interface.LanguageResources.ValidateField", culture))
      {
        XElement descriptionElement = ruleElement.Descendants("description").FirstOrDefault();
        if ((descriptionElement != null) && (!string.IsNullOrEmpty(descriptionElement.Value)))
        {
          description = fetchResource.GetString(descriptionElement.Value);
        }
        else
        {
          description = fetchResource.GetString("unknownFailure");
        }
      }

      FailureInfo = new ValidationFailure(failureCode, description);
    }

  }
}
