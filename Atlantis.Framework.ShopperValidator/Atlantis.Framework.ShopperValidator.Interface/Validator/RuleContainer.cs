using System.Collections.Generic;
using Atlantis.Framework.ShopperValidator.Interface.LanguageResources;
using Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public abstract class RuleContainer
  {
    public HashSet<ValidationRule> RulesToValidate {get;set;}
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }
    public string RuleName { get; set; }

    private string _value = string.Empty;
    public string Value { get { return _value; } set { _value = value; } }

    private string _defaultCulture = "en";
    private string _culture;
    public string Culture
    {
      get
      {
        if (string.IsNullOrEmpty(_culture))
        {
          _culture = _defaultCulture;
        }

        return _culture;
      }
      set { _culture = value; }
    }

    private FetchResource _fetchResource;
    protected FetchResource FetchResource
    {
      get
      {
        if (_fetchResource == null)
        {
          _fetchResource = new FetchResource(ResourceNamespace.ShopperValidator, Culture);
        }

        return _fetchResource;
      }
    }

    protected RuleContainer(string value, string culture = "")
    {
      Value = value;
      _culture = culture;
      ErrorMessage = string.Empty;
      RuleName = string.Empty;
      RulesToValidate = new HashSet<ValidationRule>();
    }


    protected RuleContainer(string culture = "") : this(string.Empty, culture)
    {
      Value = Value;
    }

    public void Validate()
    {
      ShopperRuleValidator validator = new ShopperRuleValidator();
      validator.ValidateOneRule(this);
    }
    
    public void AddIsRequiredRule(string value, string fieldName,bool isRequired)
    {
      if (isRequired)
      {
        RulesToValidate.Add(new RequiredRule(fieldName, value, Culture));
      }
    }
  }
}
