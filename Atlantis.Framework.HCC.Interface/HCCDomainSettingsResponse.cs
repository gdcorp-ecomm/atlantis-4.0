using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Atlantis.Framework.HCC.Interface.DomainSettings
{
  [DataContract(Name = "hccresp")]
  public class HCCDomainSettingsResponse : IHCCResponseMessage
  {
    [DataMember(Name = "xml")]
    string _xml = string.Empty;

    [DataMember(Name = "msg")]
    string _responseMessage;

    [DataMember(Name = "status")]
    string _responseStatus;

    [DataMember(Name = "code")]
    int _responseStatusCode;

    public HCCDomainSettingsResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    string[] _errors;
    [DataMember(Name = "errors")]
    public string[] Errors
    {
      get
      {
        return _errors ?? new string[0];
      }

      set
      {
        _errors = value;
      }
    }

    string[] _texts;
    [DataMember(Name = "text")]
    public string[] Texts
    {
      get
      {
        return _texts ?? new string[0];
      }

      set
      {
        _texts = value;
      }
    }

    [DataMember]
    HCCOptionGroup _optionGroup;

    [DataMember(Name = "optgrp")]
    public HCCOptionGroup OptionGroup 
    {
      get
      {
        return _optionGroup ?? new HCCOptionGroup();
      }
      set
      {
        _optionGroup = value;
      }
    }


    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           GetOptionGroup(),
           new XElement("errors",
            from error in Errors
            select new XElement("error", error)
           )
        );

        _xml = response.ToString();
      }

      return _xml;
    }

    XElement GetOptionGroup()
    {
      XElement group = new XElement("optiongroup",
        new XAttribute("helparticleid", OptionGroup.HelpArticleID),
        new XAttribute("listtype", OptionGroup.ListType),
        new XAttribute("text", OptionGroup.Text ?? string.Empty),
        new XAttribute("title", OptionGroup.Title ?? string.Empty),
        new XElement("optionlistitems",
          from item in OptionGroup.HCCOptionListItems
          select new XElement("optionlistitem",
            new XAttribute("selected", item.Selected),
            new XAttribute("text", item.Text ?? string.Empty),
            new XAttribute("value", item.Value.ToString() ?? string.Empty)
          )
        )
      );

      return group;
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
