using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract]
  public class HCCAvailDomainsResponse : IHCCResponseMessage
  {
    [DataMember(Name = "xml")]
    string _xml = string.Empty;

    [DataMember(Name = "msg")]
    string _responseMessage;

    [DataMember(Name = "status")]
    string _responseStatus;

    [DataMember(Name = "code")]
    int _responseStatusCode;

    public HCCAvailDomainsResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    string[] _domains;
    [DataMember(Name = "domains")]
    public string[] Domains
    {
      get
      {
        return _domains ?? new string[0];
      }

      set { _domains = value; }
    }

    [DataMember(Name = "page")]
    public int Page { get; set; }

    [DataMember(Name = "pagesz")]
    public int PageSize { get; set; }

    [DataMember(Name = "domcount")]
    public int TotalDomains { get; set; }



    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {

        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           new XElement("page", Page),
           new XElement("pagesize", PageSize),
           new XElement("totaldomains", TotalDomains),
           new XElement("domains",
             from domain in Domains
             where !string.IsNullOrEmpty(domain)
             select new XElement("domain", domain)
           )
        );

        _xml = response.ToString();
      }

      return _xml;
    }

    #region IHCCResponseMessage Interface
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
