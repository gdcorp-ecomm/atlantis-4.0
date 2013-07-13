using Atlantis.Framework.ShopperValidator.Interface.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ShopperValidator.Interface.ValidationRules.BaseRules
{
  public class EqualLength : ValidationRule
  {
    public int ExactLength { get; private set; }
    public bool AlphaNumeric { get; set; }

    public EqualLength(string fieldName, string textToValidate, int exactLength = 0, bool alphaNumeric = false, string culture = "")
      : base(culture)
    {
      ExactLength = exactLength;
      base.ItemToValidate = textToValidate;

      string errorMessage = alphaNumeric ? FetchResource.GetString("equalLengthAlpha") : FetchResource.GetString("equalLengthNumeric");
      base.ErrorMessage = string.Format(errorMessage, fieldName, ExactLength.ToString());
    }

    public EqualLength(string culture, string fieldName, string textToValidate, int exactLength = 0, bool alphaNumeric = false)
      : this(fieldName, textToValidate, exactLength, alphaNumeric, culture)
    {}
    
    public override void Validate()
    {
      base.IsValid = false;
      if (base.ItemToValidate != null)
      {
        base.IsValid = base.ItemToValidate.Length == ExactLength;
      }
    }
  }
}
