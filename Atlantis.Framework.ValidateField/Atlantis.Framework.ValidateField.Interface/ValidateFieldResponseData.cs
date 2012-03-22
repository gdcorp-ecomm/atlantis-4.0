using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.ValidateField.Interface
{
  public class ValidateFieldResponseData : IResponseData
  {
    AtlantisException _exception = null;
    XDocument _fieldValidationDoc;

    public bool HasLengthRules { get; private set; }
    public bool HasExpressionRules { get; private set; }
    public string FieldType { get; private set; }
    public int MinLength { get; private set; }
    public int MaxLenght { get; private set; }

    private ValidateFieldResponseData()
    {
      HasLengthRules = false;
      HasExpressionRules = false;
    }

    public ValidateFieldResponseData(string fieldValidationXml) : this()
    {
      if (string.IsNullOrEmpty(fieldValidationXml))
      {
        _fieldValidationDoc = XDocument.Parse(fieldValidationXml);
      }
    }

    public ValidateFieldResponseData(RequestData requestData, Exception ex) : this()
    {
      _exception = new AtlantisException(requestData, "ValidateFieldResponse", ex.Message, ex.StackTrace, ex);
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
