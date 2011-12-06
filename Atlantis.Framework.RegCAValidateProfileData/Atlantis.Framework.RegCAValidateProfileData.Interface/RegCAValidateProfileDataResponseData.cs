using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegCAValidateProfileData.Interface
{
  public class RegCAValidateProfileDataResponseData : IResponseData
  {
    private string _responseXML;
    private AtlantisException _atlException;

    private List<string> _errors;

    public RegCAValidateProfileDataResponseData(string responseXML)
    {
      _responseXML = responseXML;
    }

    public RegCAValidateProfileDataResponseData(AtlantisException exAtlantis)
    {
      _responseXML = "";
      _atlException = exAtlantis;
    }

    public RegCAValidateProfileDataResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _responseXML = sResponseXML;
      _atlException = new AtlantisException(oRequestData, "RegCAValidateProfileDataResponseData", ex.Message, string.Empty);
    }

    public List<string> Errors
    {
      get
      {
        if (_errors == null)
        {
          _errors = new List<string>();

          XmlDocument responseDoc = new XmlDocument();
          responseDoc.LoadXml(_responseXML);
          XmlNode successNode = responseDoc.SelectSingleNode("//results/success/profile");
          if (successNode != null)
          {
            if (successNode.Attributes["validforregistration"].Value == "0")
            {
              if (successNode.Attributes["match"].Value == "0")
              {
                _errors.Add("Your Registrant Name/ID for your .CA domain(s) is incorrect or invalid.");
              }
              else
              {
                _errors.Add(successNode.Attributes["ciraerror"].Value);
              }
            }
          }
          else
          {

            XmlNodeList errorNodes = responseDoc.SelectNodes("//results/errors/error");
            if (errorNodes.Count == 0)
            {
              XmlNode errorsNode = responseDoc.SelectSingleNode("//results/errors");
              if (errorsNode != null)
              {
                _errors.Add("Your Registrant Name/ID for your .CA domain(s) is incorrect or invalid.");
              }
            }
            else
            {
              _errors.Add("Your Registrant Name/ID for your .CA domain(s) is incorrect or invalid.");
            }
          }
        }
        return _errors;
      }
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
