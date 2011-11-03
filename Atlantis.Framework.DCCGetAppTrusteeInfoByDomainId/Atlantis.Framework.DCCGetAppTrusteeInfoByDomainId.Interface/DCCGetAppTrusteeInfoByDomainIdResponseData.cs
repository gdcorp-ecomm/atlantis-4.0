using System;
using Atlantis.Framework.Interface;
using System.Xml;
using System.Collections.Generic;

namespace Atlantis.Framework.DCCGetAppTrusteeInfoByDomainId.Interface
{
  public class DCCGetAppTrusteeInfoByDomainIdResponseData : IResponseData
  {
    private bool _isSuccess = false;
    private string _responseXml;
    private AtlantisException _exception = null;

    private List<string> _failedDomainIds;
    private Dictionary<string, string> _validatedDomainIds;

    public DCCGetAppTrusteeInfoByDomainIdResponseData(string responseXml, RequestData requestData)
    {
      _responseXml = responseXml;
      _isSuccess = ParseResponse(requestData);
    }

    public DCCGetAppTrusteeInfoByDomainIdResponseData(string responseXml, AtlantisException ex)
    {
      _responseXml = responseXml;
      _exception = ex;
      _isSuccess = false;
    }

    public DCCGetAppTrusteeInfoByDomainIdResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(requestData, "DCCGetAppTrusteeInfoByDomainIdResponseData", ex.Message + Environment.NewLine + ex.StackTrace, _responseXml);
      _isSuccess = false;
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public Dictionary<string, string> ValidatedDomainIds
    {
      get
      {
        return _validatedDomainIds;
      }
    }

    public List<string> FailedDomainIds
    {
      get
      {
        return _failedDomainIds;
      }
    }

    #region Private Methods

    private bool ParseResponse(RequestData requestData)
    {
      bool result = false;

      if (!string.IsNullOrEmpty(_responseXml))
      {
        try
        {
          XmlDocument resultDoc = new XmlDocument();
          resultDoc.LoadXml(_responseXml);

          XmlNode successNode = resultDoc.SelectSingleNode("//success");
          if (successNode != null)
          {
            string successValue = successNode.InnerText;
            if (successValue == "1")
            {
              XmlNodeList domainNodes = resultDoc.SelectNodes("//domains/domain");
              foreach (XmlElement domain in domainNodes)
              {
                XmlAttribute processingAttr = domain.Attributes["processing"];
                if (null != processingAttr)
                {
                  if (processingAttr.Value.Equals("success"))
                  {
                    if (null == _validatedDomainIds)
                    {
                      _validatedDomainIds = new Dictionary<string, string>();
                    }
                    _validatedDomainIds[domain.Attributes["domainname"].Value] = domain.Attributes["trusteebillingresourceid"].Value;
                  }
                  else
                  {
                    if (null == _failedDomainIds)
                    {
                      _failedDomainIds = new List<string>();
                    }
                    _failedDomainIds.Add(domain.Attributes["domainid"].Value);
                  }
                }
              }
              result = true;
            }
            else // error
            {
              string message = null;
              string description = null;

              XmlNode errorMessageNode = resultDoc.SelectSingleNode("//error/message");
              if (errorMessageNode != null)
              {
                message = errorMessageNode.InnerText;
              }

              XmlNode errorDescriptionNode = resultDoc.SelectSingleNode("//error/description");
              if (errorDescriptionNode != null)
              {
                description = errorDescriptionNode.InnerText;
              }

              if (string.IsNullOrEmpty(message))
              {
                message = "No error message provided.";
              }

              if (string.IsNullOrEmpty(description))
              {
                description = "No error description provided.";
              }

              _exception = new AtlantisException(requestData, "DCCGetAppTrusteeInfoByDomainIdResponseData.ParseResponse", message + Environment.NewLine + description, _responseXml);
            }
          }
          else
          {
            _exception = new AtlantisException(requestData, "DCCGetAppTrusteeInfoByDomainIdResponseData.ParseResponse", "Xml result missing success node.", _responseXml);
          }

        }
        catch (Exception ex)
        {
          _exception = new AtlantisException(requestData, "DCCGetAppTrusteeInfoByDomainIdResponseData.ParseResponse", ex.Message + Environment.NewLine + ex.StackTrace, string.Empty);
        }
      }

      return result;
    }

    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
