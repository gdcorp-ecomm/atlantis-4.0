using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CMSIsDomainUsed.Interface
{
  public class CMSIsDomainUsedResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    //dictionary by product group, then by domainname to find domainListInfo
    public Dictionary<string, DomainAvailability> DomainResults = new Dictionary<string, DomainAvailability>();

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public CMSIsDomainUsedResponseData(string xml)
    {
      _resultXML = xml;
      _success = ParseResponse();
    }

    private string GetAttribute(XmlNode currentNode, string attributeName, string defaultValue)
    {
      string result = defaultValue;
      try
      {
        if (currentNode.Attributes[attributeName] != null)
        {
          result = currentNode.Attributes[attributeName].Value;
        }
      }
      catch { }
      return result;
    }

    private bool ParseResponse()
    {
      bool result = true;
      try
      {
        //<ServiceResponse resultCode="0">
        //   <DomainStatus shopperId="858346">
        //      <Domain name="NateTest01.com" isvalid="true" />
        //      <Domain name="NateTest02.com" isvalid="true" />
        //   </DomainStatus>
        //</ServiceResponse>
        if (!string.IsNullOrEmpty(_resultXML))
        {
          XmlDocument _xmlDoc = new XmlDocument();
          _xmlDoc.LoadXml(_resultXML);
          XmlNodeList _productGroups = _xmlDoc.SelectNodes("//ServiceResponse/DomainStatus/Domain");
          XmlNodeList _errorNode = _xmlDoc.SelectNodes("//ServiceResponse/DomainStatus/Errors/Error");
          if (_errorNode.Count != 0)
          {
            result = false;
          }
          foreach (XmlNode currentGroup in _productGroups)
          {

            string domainName = GetAttribute(currentGroup, "name", string.Empty);
            string isValidText = GetAttribute(currentGroup, "isvalid", "false");
            string associatedProduct = GetAttribute(currentGroup, "associatedproduct", string.Empty);
            bool isValid = false;
            bool.TryParse(isValidText, out isValid);
            if (!DomainResults.ContainsKey(domainName))
            {
              DomainAvailability oDomainInfo = new DomainAvailability();
              oDomainInfo.DomainName = domainName;
              oDomainInfo.AssociatedProduct = associatedProduct;
              oDomainInfo.IsValid = isValid;
              DomainResults.Add(domainName, oDomainInfo);
            }
          }
        }
      }
      catch (System.Exception ex)
      {
        result = false;
        _exception = new AtlantisException("ParseResults", string.Empty, "0", "ErrorParsingResults", ex.Message + Environment.NewLine + ex.StackTrace, string.Empty, string.Empty, string.Empty, string.Empty, 0);
      }
      return result;
    }

     public CMSIsDomainUsedResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public CMSIsDomainUsedResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "CMSIsDomainUsedResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
