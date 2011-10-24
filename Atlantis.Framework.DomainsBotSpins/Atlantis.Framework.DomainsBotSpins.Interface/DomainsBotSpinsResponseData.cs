using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsBotSpins.Interface
{
  public class DomainsBotSpinsResponseData : IResponseData
  {
    AtlantisException _ex = null;

    private Dictionary<string, List<KeyValuePair<string, string>>> _domainNames = new Dictionary<string, List<KeyValuePair<string, string>>>();
    public Dictionary<string, List<KeyValuePair<string, string>>> DomainNames
    {
      get
      {
        return _domainNames;
      }
      set
      {
        _domainNames = value;
      }
    }
    private int _resultsCount = 0;
    public int ResultsCount
    {
      get
      {
        return _resultsCount;
      }
      set
      {
        _resultsCount = value;
      }
    }

    public DomainsBotSpinsResponseData(int domainResultLength, Dictionary<string, List<KeyValuePair<string, string>>> domains)
    {
      _resultsCount = domainResultLength;
      _domainNames = domains;
    }

    public DomainsBotSpinsResponseData(AtlantisException atlEx)
    {
      _ex = atlEx;
    }

    public DomainsBotSpinsResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException(oRequestData,
                                   "DomainsBotSpinsResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }
    
    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _ex;
    }

    public string ToXML()
    {
      StringBuilder result = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(result));

      xtwResult.WriteStartElement("DomainList");

      if (_domainNames != null)
      {
        foreach (KeyValuePair<string, List<KeyValuePair<string, string>>> oPair in _domainNames)
        {
          xtwResult.WriteStartElement("Domain");
          xtwResult.WriteAttributeString("Name", oPair.Key);

          foreach (KeyValuePair<string, string> item in oPair.Value)
          {
            xtwResult.WriteAttributeString(item.Key, item.Value);      
          }                    
          xtwResult.WriteEndElement();
        }
      }

      xtwResult.WriteEndElement();

      return result.ToString();
    }

    #endregion
  }
}
