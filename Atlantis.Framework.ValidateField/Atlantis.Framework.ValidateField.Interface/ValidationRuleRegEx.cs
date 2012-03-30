using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.ValidateField.Interface
{
  public class ValidationRuleRegEx : ValidationRuleElementBase<string>
  {
    public bool MatchIsValid { get; private set; }
    public bool IgnoreCase { get; private set; }
    public string ServerPattern { get; private set; }
    public string ClientPattern { get; private set; }

    Regex _validationExpression = null;

    public ValidationRuleRegEx(XElement ruleElement)
      : base(ruleElement)
    {
      MatchIsValid = true;
      IgnoreCase = false;

      XAttribute matchModeAttribute = ruleElement.Attribute("matchisvalid");
      if (matchModeAttribute != null)
      {
        MatchIsValid = matchModeAttribute.Value != "false";
      }

      XAttribute ignoreCaseAttribute = ruleElement.Attribute("ignorecase");
      if (ignoreCaseAttribute != null)
      {
        IgnoreCase = ignoreCaseAttribute.Value == "true";
      }

      XElement serverElement = ruleElement.Descendants("server").FirstOrDefault();
      if (serverElement != null)
      {
        ServerPattern = serverElement.Value;

        RegexOptions options = RegexOptions.Compiled;
        if (IgnoreCase)
        {
          options = options | RegexOptions.IgnoreCase;
        }

        _validationExpression = new Regex(ServerPattern, options);
      }
      else
      {
        ServerPattern = null;
      }

      XElement clientElement = ruleElement.Descendants("client").FirstOrDefault();
      if (clientElement != null)
      {
        ClientPattern = clientElement.Value;
      }
      else
      {
        ClientPattern = null;
      }
    }

    public override bool IsValid(string itemToValidate)
    {
      bool result = false;

      if (_validationExpression == null)
      {
        result = true;
      }
      else if (itemToValidate != null)
      {
        result = bool.Equals(_validationExpression.IsMatch(itemToValidate), MatchIsValid);
      }

      return result;
    }
  }
}
