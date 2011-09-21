using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract(Name = "hccresp")]
  public class HCCAvailableSSLResponse : IHCCResponseMessage
  {
    [DataMember(Name = "xml")]
    string _xml = string.Empty;

    [DataMember(Name = "msg")]
    string _responseMessage;

    [DataMember(Name = "status")]
    string _responseStatus;

    [DataMember(Name = "code")]
    int _responseStatusCode;

    public HCCAvailableSSLResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }
    
    [DataMember(Name = "enab")]
    public bool Enabled { get; set; }

    [DataMember(Name = "hlpid")]
    public int HelpArticleID {get; set; }

    [DataMember(Name = "instrtxt")]
    public string InstructionText { get; set; }

    [DataMember(Name = "subagreurl")]
    public string SubscriberAgreementUrl { get; set; }

    [DataMember(Name = "sslitemlist")]
    ReadOnlyCollection<HCCSSLItem> _sslItemList;
    [DataMember(Name = "sslitem")]
    public ReadOnlyCollection<HCCSSLItem> HCCSSLItemList
    {
      get
      {
        return _sslItemList ?? new List<HCCSSLItem>().AsReadOnly();
      }
      set
      {
        value = _sslItemList;
      }
    }
    public void SetHCCOptionGroups(ReadOnlyCollection<HCCSSLItem> sslItemList)
    {
      _sslItemList = sslItemList;
    }

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           new XElement("enabled", Enabled),
           new XElement("helparticleid", HelpArticleID),
           new XElement("instructiontext", InstructionText),
           new XElement("subscriberagreementurl", SubscriberAgreementUrl),
           GetSSLItemList()
        );

        _xml = response.ToString();
      }

      return _xml;
    }

    XElement GetSSLItemList()
    {
      XElement sslItemList = new XElement("hccsslitems",
        from item in HCCSSLItemList
        select new XElement("hccsslitem",
          new XAttribute("displaytext", item.DisplayText ?? string.Empty),
          new XAttribute("value", item.Value ?? string.Empty)
        )
      );

      return sslItemList;
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
