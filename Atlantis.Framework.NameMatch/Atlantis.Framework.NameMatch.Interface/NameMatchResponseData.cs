using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.NameMatch.Interface
{
  public class NameMatchResponseData : IResponseData
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

    /// <summary>
    /// Only filled when calling as 1.0 service
    /// </summary>
    public AvailableDomain[] AvailableDomains;

    private int _resultsCount;
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

    public bool IsSuccess { get; set; }

    public NameMatchResponseData(int domainResultLength, Dictionary<string, List<KeyValuePair<string, string>>> domains, AvailableDomain[] oResponse)
    {
      ResultsCount = domainResultLength;
      DomainNames = domains;
      AvailableDomains = oResponse;
      IsSuccess = (domains.Count > 0);
    }

    public NameMatchResponseData(AtlantisException atlEx)
    {
      _ex = atlEx;
    }

    public NameMatchResponseData(RequestData oRequestData, Exception ex)
    {
      _ex = new AtlantisException(oRequestData,
                                   "NameMatchResponseData",
                                   ex.Message,
                                   oRequestData.ToXML());
    }

    public NameMatchResponseData(bool isSuccess)
    {
      IsSuccess = isSuccess;
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
