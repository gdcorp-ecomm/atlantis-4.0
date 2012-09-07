using System;

namespace Atlantis.Framework.ShopperValidator.Interface.Validator
{
  public abstract class ValidationRule
  {
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }
    public string ItemToValidate { get; set; }
    public string FieldName { get; set; }

    public ValidationRule()
    {
      ErrorMessage = string.Empty;
      ItemToValidate = string.Empty;
      FieldName = string.Empty;
    }


    public virtual void Validate()
    {
      throw new NotImplementedException("Must provide a custom implementation of Validate.");
    }
  }
}
