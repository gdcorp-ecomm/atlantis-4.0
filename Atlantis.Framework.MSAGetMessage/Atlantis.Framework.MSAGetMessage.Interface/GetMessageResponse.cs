using System.Runtime.Serialization;
using System.Xml.Serialization;
using Atlantis.Framework.MSA.Interface;

namespace Atlantis.Framework.MSAGetMessage.Interface
{
  [DataContract]
  public class GetMessageResponse : MailApiResponse
  {
    [DataMember(Name = "response")]
    public Message Message { get; set; }

    // BEGIN common response components
    // couldn't include these in superclass for various reasons
    // 1) there's kludge required in deserialization annotations
    // 2) you can't set the Order of the items in superclass (which exposes a bug in GDAndroid2.3.2 GetCalendarLink parsing)
    [XmlElement(ElementName = "DisplayError")]
    public virtual DisplayError displayError { get; set; }

    // Unsure of usefulness of this.. at least jsoap_fault is better indicator of bad session
    [DataMember(Name = "state")]
    [XmlElement(ElementName = "state")]
    public MailApiResponseState State { get; set; }

    [XmlElement(ElementName = "ResultCode")]
    public int ResultCode { get; set; }

    // JSOAP_FAULT (the bool, the message, and the detail)
    // DataMembers defined so we can deserialize from mailapi, but we won't return in MSA response
    [DataMember(Name = "jsoap_fault")]
    [XmlIgnore]
    public bool JsoapFault { set; get; }

    [DataMember(Name = "message")]
    [XmlIgnore]
    public string JsoapMessage { get; set; }

    [DataMember(Name = "detail")]
    [XmlIgnore]
    public string JsoapDetail { get; set; }
    // END common response components

    public bool isJsoapFault()
    {
      return JsoapFault;
    }

    public string getJsoapFaultMessage()
    {
      return JsoapMessage;
    }
  }
}
