using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Atlantis.Framework.EcommBillingSync.Impl.Helper_Classes
{
  public class CartResponse
  {
    private readonly XmlDocument _responseDoc;
    private List<KeyValuePair<string, string>> _errors;

    public CartResponse(string responseXml)
    {
      if (!string.IsNullOrEmpty(responseXml))
      {
        _responseDoc = new XmlDocument();
        try
        {
          _responseDoc.LoadXml(responseXml);
        }
        catch (Exception)
        {
          _responseDoc = null;
        }
      }
    }

    public CartResponse(string errorNumber, string errorMessage)
    {
      _errors = new List<KeyValuePair<string, string>> {new KeyValuePair<string, string>(errorNumber, errorMessage)};
    }

    public bool Success
    {
      get
      {
        var result = false;
        if (_responseDoc != null)
        {
          var messageNode = _responseDoc.SelectSingleNode("//MESSAGE");
          if (messageNode != null)
          {
            result = (String.Compare(messageNode.InnerText, "SUCCESS", StringComparison.OrdinalIgnoreCase) == 0);
          }
        }
        return result;
      }
    }

    public string ErrorMessage
    {
      get
      {
        var result = "Unknown error.";
        if (Errors.Count > 0)
        {
          result = Errors[0].Key + " : " + Errors[0].Value;
        }
        return result;
      }
    }

    public bool ContainsErrorMessage(string message)
    {
      return Errors.Any(error => String.Compare(error.Value, message, StringComparison.OrdinalIgnoreCase) == 0);
    }

    public bool ContainsErrorNumber(string number)
    {
      return Errors.Any(error => String.Compare(error.Key, number, StringComparison.OrdinalIgnoreCase) == 0);
    }

    public List<KeyValuePair<string, string>> Errors
    {
      get
      {
        if (_errors == null)
        {
          _errors = new List<KeyValuePair<string, string>>();
          if (_responseDoc != null)
          {
            var errorNodes = _responseDoc.SelectNodes("//ERRORS/ERROR");
            if (errorNodes != null)
            {
              foreach (XmlNode errorNode in errorNodes)
              {
                var numberNode = errorNode.SelectSingleNode("./NUMBER");
                if ((numberNode != null) && (!String.IsNullOrEmpty(numberNode.InnerText)))
                {
                  var errorDesc = string.Empty;
                  var descNode = errorNode.SelectSingleNode("./DESC");
                  if (descNode != null)
                  {
                    errorDesc = descNode.InnerText;
                  }

                  _errors.Add(new KeyValuePair<string, string>(numberNode.InnerText, errorDesc));
                }
              }
            }
          }
          else
          {
            _errors.Add(new KeyValuePair<string, string>("0", "Cart Xml response was not valid Xml."));
          }
        }
        return _errors;
      }
    }
  }
}
