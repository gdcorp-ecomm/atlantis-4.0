using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;

namespace Atlantis.Framework.CMSCreditDomainList.Interface
{
  public class CMSCreditDomainListResponseData : IResponseData
  {

    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseXml = string.Empty;
    //dictionary by product group, then by domainname to find domainListInfo
    public Dictionary<int, Dictionary<string, DomainListInfo>> DomainResults = new Dictionary<int, Dictionary<string, DomainListInfo>>();

    public CMSCreditDomainListResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isSuccess = ParseResponse();
    }

    private bool ParseResponse()
    {
      bool result = true;
      try
      {
        if (!string.IsNullOrEmpty(_responseXml))
        {
          XmlDocument _xmlDoc = new XmlDocument();
          _xmlDoc.LoadXml(_responseXml);
          XmlNodeList _productGroups= _xmlDoc.SelectNodes("//DomainList");
          foreach (XmlNode currentGroup in _productGroups)
          {
            string productGroup = currentGroup.Attributes["productGroup"].Value;
            int prdGroup = 0;
            int.TryParse(productGroup, out prdGroup);
            Dictionary<string, DomainListInfo> _domainSet = new Dictionary<string, DomainListInfo>();
            foreach (XmlNode attrValues in currentGroup.ChildNodes)
            {
              DomainListInfo currentDomain = new DomainListInfo();
              string domainName = string.Empty;
              foreach (XmlAttribute currentAttribute in attrValues.Attributes)
              {
                if (currentAttribute.Name == "name")
                {
                  domainName = currentAttribute.Value;
                }
                currentDomain[currentAttribute.Name] = currentAttribute.Value;
              }
              if (!string.IsNullOrEmpty(domainName))
              {
                if (!_domainSet.ContainsKey(domainName))
                {
                  _domainSet.Add(domainName, currentDomain);
                }
              }
            }
            if (!DomainResults.ContainsKey(prdGroup))
            {
              DomainResults.Add(prdGroup, _domainSet);
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

    public CMSCreditDomainListResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public CMSCreditDomainListResponseData(RequestData requestData, Exception ex)
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
