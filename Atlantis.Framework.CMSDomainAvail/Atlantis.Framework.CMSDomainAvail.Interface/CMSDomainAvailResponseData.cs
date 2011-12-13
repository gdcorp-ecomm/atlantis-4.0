using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;

namespace Atlantis.Framework.CMSDomainAvail.Interface
{
  public class CMSDomainAvailResponseData : IResponseData
  {

    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseXml = string.Empty;
    //dictionary by product group, then by domainname to find domainListInfo
    public Dictionary<string, CMSDomainInfo> DomainResults = new Dictionary<string, CMSDomainInfo>();
    public CMSDomainAvailResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isSuccess = ParseResponse();
    }

    private string GetAttribute(XmlNode currentNode, string attributeName,string defaultValue)
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
        if (!string.IsNullOrEmpty(_responseXml))
        {
          XmlDocument _xmlDoc = new XmlDocument();
          _xmlDoc.LoadXml(_responseXml);
          XmlNodeList _productGroups = _xmlDoc.SelectNodes("//ServiceResponse/DomainStatus/Domain");
          XmlNodeList _errorNode = _xmlDoc.SelectNodes("//ServiceResponse/DomainStatus/Errors/Error");
          if (_errorNode.Count != 0)
          {
            result = false;
          }
          foreach (XmlNode currentGroup in _productGroups)
          {
           
            string domainName = GetAttribute(currentGroup,"name",string.Empty);
            string isValidText = GetAttribute(currentGroup, "isvalid", "false");
            string associatedProduct = GetAttribute(currentGroup, "associatedproduct", string.Empty);
            bool isValid=false;
            bool.TryParse(isValidText,out isValid);
            if (!DomainResults.ContainsKey(domainName))
            {
              CMSDomainInfo oDomainInfo = new CMSDomainInfo();
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

    public CMSDomainAvailResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public CMSDomainAvailResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "CMSCreditDomainListResponseData", ex.Message, ex.StackTrace);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
