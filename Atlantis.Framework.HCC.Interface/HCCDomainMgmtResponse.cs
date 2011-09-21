using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract]
  public class HCCDomainMgmtResponse : IHCCResponseMessage
  {
    string _responseMessage;
    string _responseStatus;
    int _responseStatusCode;
    string _xml = string.Empty;

    public HCCDomainMgmtResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    [DataMember]
    public string AccountDomainsXml { get; set; }

    [DataMember]
    public string[] Warnings { get; set; }

    [DataMember]
    public bool DomainNameChangeAllowed { get; set; }

    [DataMember]
    public string[] ModifiedDomains { get; set; }
    
    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        var warnings = new XElement("warnings");
        if (Warnings != null)
        {
          foreach (var warning in Warnings)
          {
            warnings.Add(
              new XElement("warning", warning)
              );
          }
        }

        var modifiedDomains = new XElement("modifieddomains");
        if (ModifiedDomains != null)
        {
          foreach (var domain in ModifiedDomains)
          {
            modifiedDomains.Add(
              new XElement("domain", domain)
              );
          }
        }

        var accountDomains = new XElement("domains");
        if (AccountDomainsXml != null)
        {
          if (!string.IsNullOrEmpty(AccountDomainsXml))
          {
            accountDomains = XElement.Parse(AccountDomainsXml);
          }
        }

        var response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           new XElement("domainnamechangeallowed", DomainNameChangeAllowed),
           accountDomains,
           warnings
        );

        _xml = response.ToString();
      }

      return _xml;
    }

    #region HCCMessage Interface
    public string GetResponseMessage()
    {
      return _responseMessage;
    }

    public string GetResponseStatus()
    {
      return _responseStatus;
    }

    public int GetResponseStatusCode()
    {
      return _responseStatusCode;
    }
    #endregion
  }
}
