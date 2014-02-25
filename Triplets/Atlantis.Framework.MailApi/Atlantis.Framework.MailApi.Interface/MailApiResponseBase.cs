using Atlantis.Framework.Interface;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class MailApiResponseBase : IResponseData, IMailApiResponse // Framework providers should implement a corresponding interface
  {
    [XmlElement(ElementName = "ResultCode")]
    public int ResultCode { get; set; }

    // JSOAP_FAULT (the bool, the message, and the detail)
    // DataMembers defined so we can deserialize from mailapi, but we won't return in MSA response
    [DataMember(Name = "jsoap_fault")]
    [XmlIgnore]
    public bool IsJsoapFault { set; get; }

    [DataMember(Name = "message")]
    [XmlIgnore]
    public string JsoapMessage { get; set; }

    [DataMember(Name = "detail")]
    [XmlIgnore]
    public string JsoapDetail { get; set; }
    
    public string GetJsoapFaultMessage()
    {
      return JsoapMessage;
    }

    public string ToXML()
    {
      return new XElement((XName)this.GetType().FullName).ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return (AtlantisException)null;
    }
  }
}
