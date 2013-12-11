using System.Xml.Linq;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidationRuleLength : ValidationRuleElementBase<string>
  {
    public int MinimumLength { get; private set; }
    public int MaximumLength { get; private set; }

    public ValidationRuleLength(XElement ruleElement) : base(ruleElement)
    {
      MinimumLength = 0;
      MaximumLength = int.MaxValue;

      var minAttribue = ruleElement.Attribute("min");
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
      var result = false;
      if (itemToValidate != null)
      {
        result = (itemToValidate.Length >= MinimumLength) && (itemToValidate.Length <= MaximumLength);
      }
      return result;
    }
  }
}