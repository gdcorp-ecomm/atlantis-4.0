using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Atlantis.Framework.MSA.Interface
{
  [DataContract]
  public class MailApiResponseState // encaspulates the "state" portion of mailapi responses
  {
    [DataMember(Name = "session")]
    [XmlElement(ElementName="session")]
    public string Session { get; set; }

  }
}