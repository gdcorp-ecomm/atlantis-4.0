using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CMSCreditAccounts.Interface
{
  public class CMSCreditAccountsResponseData : IResponseData
  {

    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseXml = string.Empty;
    //dictionary by product group, then by domainname to find CreditAccountInfo
    public Dictionary<int, Dictionary<string, CreditAccountInfo>> CreditResults = new Dictionary<int, Dictionary<string, CreditAccountInfo>>();
    public Dictionary<int, Dictionary<string, CreditAccountInfo>> AccountResults = new Dictionary<int, Dictionary<string, CreditAccountInfo>>();

    public CMSCreditAccountsResponseData(string responseXml)
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
            string resultType = currentGroup.Attributes["listType"].Value;
            int prdGroup = 0;
            int.TryParse(productGroup, out prdGroup);
            Dictionary<string, CreditAccountInfo> _domainSet = new Dictionary<string, CreditAccountInfo>();
            foreach (XmlNode attrValues in currentGroup.ChildNodes)
            {
              CreditAccountInfo currentDomain = new CreditAccountInfo();
              string domainName = string.Empty;
              foreach (XmlAttribute currentAttribute in attrValues.Attributes)
              {
                switch (resultType)
                {
                  case "credits":
                    if (currentAttribute.Name == "name")
                    {
                      domainName = currentAttribute.Value;
                    }
                    break;
                  case "accounts":
                    if (currentAttribute.Name == "orionGuid")
                    {
                      domainName = currentAttribute.Value;
                    }
                    break;
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
            switch (resultType)
            {
              case "credits":
                if (!CreditResults.ContainsKey(prdGroup))
                {
                  CreditResults.Add(prdGroup, _domainSet);
                }
                break;
              case "accounts":
                if (!AccountResults.ContainsKey(prdGroup))
                {
                  AccountResults.Add(prdGroup, _domainSet);
                }
                break;
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

    public CMSCreditAccountsResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public CMSCreditAccountsResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "CMSCreditAccountsResponseData", ex.Message, ex.StackTrace);
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
