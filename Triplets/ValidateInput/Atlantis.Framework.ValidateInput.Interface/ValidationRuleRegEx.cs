using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Atlantis.Framework.ValidateInput.Interface
{
  public class ValidationRuleRegEx : ValidationRuleElementBase<string>
  {
    public bool MatchIsValid { get; private set; }
    public bool IgnoreCase { get; private set; }
    public string ServerPattern { get; private set; }
    public string ClientPattern { get; private set; }

    private readonly Regex _validationExpression;

    public ValidationRuleRegEx(XElement ruleElement) : base(ruleElement)
    {
      MatchIsValid = true;
      IgnoreCase = false;

      var matchModeAttribute = ruleElement.Attribute("matchisvalid");
      if (matchModeAttribute != null)
      {
        MatchIsValid = matchModeAttribute.Value != "false";
      }

      var ignoreCaseAttribute = ruleElement.Attribute("ignorecase");
      if (ignoreCaseAttribute != null)
      {
        IgnoreCase = ignoreCaseAttribute.Value == "true";
      }

      var serverElement = ruleElement.Descendants("server").FirstOrDefault();
      if (serverElement != null)
      {
        ServerPattern = serverElement.Value;

        var options = RegexOptions.Compiled;
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

      var clientElement = ruleElement.Descendants("client").FirstOrDefault();
      ClientPattern = clientElement != null ? clientElement.Value : null;
    }

    public override bool IsValid(string itemToValidate)
    {
      var result = false;

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