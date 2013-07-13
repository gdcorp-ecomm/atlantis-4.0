using System;
using Atlantis.Framework.ShopperValidator.Interface.LanguageResources;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public abstract class ValidationRule
  {
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }
    public string ItemToValidate { get; set; }
    public string FieldName { get; set; }

    private string _culture = "en";
    public string Culture
    {
      get { return _culture; } set { _culture = value; }
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

    public ValidationRule()
    {
      ErrorMessage = string.Empty;
      ItemToValidate = string.Empty;
      FieldName = string.Empty;
    }

    public ValidationRule(string culture)
      : this()
    {
      Culture = culture;
    }


    public virtual void Validate()
    {
      throw new NotImplementedException("Must provide a custom implementation of Validate.");
    }
  }
}
