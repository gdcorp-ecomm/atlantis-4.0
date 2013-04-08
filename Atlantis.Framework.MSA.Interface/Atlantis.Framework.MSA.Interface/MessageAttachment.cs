using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Atlantis.Framework.MSA.Interface
{
  public class MessageAttachment
  {
    [DataMember(Name = "filename")]
    [XmlElement(ElementName = "filename")]
    public string Filename { get; set; }

    [DataMember(Name = "part")]
    [XmlElement(ElementName = "part")]
    public string Part { get; set; }

    [DataMember(Name = "tnef_part")]
    [XmlIgnore]
    public string TnefPart { get; set; }

    [DataMember(Name = "encoding")]
    public string Encoding { get; set; }

    [DataMember(Name = "size")]
    public string Size { get; set; }

    // TODO: Change this back to bool when webmail fixes their side
    [DataMember(Name = "encrypted")]
    public object Encrypted { get; set; }
  }
}
