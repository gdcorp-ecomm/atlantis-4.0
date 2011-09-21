using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Atlantis.Framework.HCC.Interface.DomainSettings;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract(Name = "hccresp")]
  public class HCCSetupOptionsResponse : IHCCResponseMessage
  {
    [DataMember(Name = "xml")]
    string _xml = string.Empty;

    [DataMember(Name = "msg")]
    string _responseMessage;

    [DataMember(Name = "status")]
    string _responseStatus;

    [DataMember(Name = "code")]
    int _responseStatusCode;

    public HCCSetupOptionsResponse(string responseMessage, string responseStatus, int responseStatusCode)
    {
      _responseMessage = responseMessage;
      _responseStatus = responseStatus;
      _responseStatusCode = responseStatusCode;
    }

    //string[] _errors;
    //[DataMember(Name = "errors")]
    //public string[] Errors
    //{
    //  get
    //  {
    //    return _errors ?? new string[0];
    //  }

    //  set
    //  {
    //    _errors = value;
    //  }
    //}

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
    
    
    ReadOnlyCollection<HCCOptionGroup> _optionGroups;
    [DataMember(Name = "optgrps")]
    public ReadOnlyCollection<HCCOptionGroup> OptionGroups
    {
      get
      {
        return _optionGroups;
      }
      private set
      {
        _optionGroups = value;
      }
    }
    public void SetHCCOptionGroups(ReadOnlyCollection<HCCOptionGroup> optionGroups)
    {
      _optionGroups = optionGroups;
    }

    public string ToXML()
    {
      if (string.IsNullOrEmpty(_xml))
      {
        XElement response = new XElement("response",
           new XElement("message", _responseMessage),
           new XElement("status", _responseStatus),
           new XElement("statuscode", _responseStatusCode),
           GetOptionGroups()
        );

        _xml = response.ToString();
      }

      return _xml;
    }

    XElement GetOptionGroups()
    {
      XElement groups = new XElement("optiongroups",
        from optionGroup in OptionGroups
        select new XElement("optiongroup",
          new XAttribute("helparticleid", optionGroup.HelpArticleID),
          new XAttribute("listtype", optionGroup.ListType),
          new XAttribute("text", optionGroup.Text ?? string.Empty),
          new XAttribute("title", optionGroup.Title ?? string.Empty),
          new XElement("optionlistitems",
            from item in optionGroup.HCCOptionListItems
            select new XElement("optionlistitem",
              new XAttribute("selected", item.Selected),
              new XAttribute("text", item.Text ?? string.Empty),
              new XAttribute("value", item.Value.ToString() ?? string.Empty)
            )
          )
        )
      );

      return groups;
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
