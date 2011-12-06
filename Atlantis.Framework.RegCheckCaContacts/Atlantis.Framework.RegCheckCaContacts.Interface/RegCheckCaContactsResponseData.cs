using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegCheckCaContacts.Interface
{
  public class RegCheckCaContactsResponseData : IResponseData
  {
    private string _responseXML;
    private AtlantisException _atlException;

    private Dictionary<string, string> _errors;
    public Dictionary<string, string> Errors
    {
      get
      {
        if (_errors == null)
        {
          _errors = GetResponseErrors();
        }

        return _errors;
      }
    }

    private bool? _isValid;
    public bool IsValid
    {
      get
      {
        if (!_isValid.HasValue)
        {
          _isValid = (Errors.Count == 0);
        }

        return _isValid.Value;
      }
    }

    public RegCheckCaContactsResponseData(string responseXML)
    {
      _responseXML = responseXML;
    }

    public RegCheckCaContactsResponseData(AtlantisException exAtlantis)
    {
      _responseXML = "";
      _atlException = exAtlantis;
    }

    public RegCheckCaContactsResponseData(string sResponseXML,
      RequestData oRequestData, Exception ex)
    {
      _responseXML = sResponseXML;
      _atlException = new AtlantisException(oRequestData,
        "RegCheckCaContactsResponseData", ex.Message, string.Empty);
    }

    private Dictionary<string, string> GetResponseErrors()
    {
      Dictionary<string, string> errors = new Dictionary<string, string>();
      XmlDocument responseDoc = new XmlDocument();
      responseDoc.LoadXml(_responseXML);

      XmlNodeList errorNodes = responseDoc.SelectNodes("//results/errors/error");
      if (errorNodes.Count == 0)
      {
        XmlNode errorsNode = responseDoc.SelectSingleNode("//results/errors");

        if (errorsNode != null)
        {
          errors["0"] = errorsNode.InnerText;
        }
      }
      else
      {
        foreach (XmlNode errorNode in errorNodes)
        {

          errors[errorNode.Attributes["code"].Value] = errorNode.Attributes["msg"].Value;
        }
      }

      return errors;
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return _responseXML;
    }

    #endregion
  }
}
