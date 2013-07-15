using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.ValidateField.Interface
{
  public class ValidateFieldResponseData : IResponseData
  {
    AtlantisException _exception = null;
    XDocument _fieldValidationDoc;

    public ValidationRuleLength LengthRule { get; private set; }
    public ValidationRuleRegEx ExpressionRule { get; private set; }
    public string FieldType { get; private set; }
    public string Culuture { get; private set; }

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
    }

    public ValidateFieldResponseData(string fieldValidationXml, string culture = "")
    {
      _culture = culture;
      FieldType = "none";
      LengthRule = null;
      ExpressionRule = null;

      if (!string.IsNullOrEmpty(fieldValidationXml))
      {
        _fieldValidationDoc = XDocument.Parse(fieldValidationXml);

        XAttribute fieldTypeAttribute = _fieldValidationDoc.Root.Attribute("type");
        if (fieldTypeAttribute != null)
        {
          FieldType = fieldTypeAttribute.Value;
        }

        XElement lengthRuleElement = _fieldValidationDoc.Root.Descendants("length").FirstOrDefault();
        if (lengthRuleElement != null)
        {
          LengthRule = new ValidationRuleLength(lengthRuleElement, Culture);
        }

        XElement expressionRuleElement = _fieldValidationDoc.Root.Descendants("regex").FirstOrDefault();
        if (expressionRuleElement != null)
        {
          ExpressionRule = new ValidationRuleRegEx(expressionRuleElement, Culture);
        }
      }
      else
      {
        _fieldValidationDoc = XDocument.Parse("<validatefield type=\"none\" />");
      }
    }

    public ValidateFieldResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "ValidateFieldResponse", ex.Message, ex.StackTrace, ex);
    }

    public bool ValidateStringField(string valueToValidate, out List<ValidationFailure> failures)
    {
      failures = new List<ValidationFailure>();

      if ((LengthRule != null) && (!LengthRule.IsValid(valueToValidate)))
      {
        failures.Add(LengthRule.FailureInfo);
      }

      if ((ExpressionRule != null) && (!ExpressionRule.IsValid(valueToValidate)))
      {
        failures.Add(ExpressionRule.FailureInfo);
      }

      return (failures.Count == 0);
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _fieldValidationDoc.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
