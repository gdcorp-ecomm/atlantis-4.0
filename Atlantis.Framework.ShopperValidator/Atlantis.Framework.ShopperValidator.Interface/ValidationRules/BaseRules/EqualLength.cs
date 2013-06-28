using Atlantis.Framework.ShopperValidator.Interface.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  class EqualLength: ValidationRule
  {
    public int ExactLength { get; private set; }
    public bool AlphaNumeric { get; set; }

    public EqualLength(string fieldName, string textToValidate, int exactLength = 0,bool alphaNumeric=false)
      : base()
    {
      ExactLength = exactLength;
      base.ItemToValidate = textToValidate;
      if (alphaNumeric)
      {
        base.ErrorMessage = string.Concat(fieldName, " must be ", ExactLength.ToString(), " characters.");
      }
      else
      {
        base.ErrorMessage = string.Concat(fieldName, " must be ", ExactLength.ToString(), " digits.");
      }
    }

    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null)
      {
        base.IsValid = base.ItemToValidate.Length >= ExactLength;
      }
    }
  }
}
