using Atlantis.Framework.ShopperValidator.Interface.Validator;

namespace Atlantis.Framework.ShopperValidator.Interface.ShopperValidation
{
  public class ShopperProperty
  {
    public string Value { get; set; }
    public RuleContainer RuleContainer { get; set; }

    private bool? _hasValidationRules;
    public bool HasValidationRules
    {
      get
      {
        if (!_hasValidationRules.HasValue)
        {
          _hasValidationRules = RuleContainer != null;
        }

        return _hasValidationRules.Value;
      }
    }

    public ShopperProperty()
    { }
  }
}
