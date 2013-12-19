using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Atlantis.Framework.Iris.Interface.Objects
{
  [DataContract(Name = "Item")]
  [XmlRoot(ElementName = "Item")]
  [XmlSerializerFormat]
  public class Note
  {
    [DataMember, XmlAttribute]
    public long IncidentId { get; set; }

    [DataMember, XmlAttribute]
    public int IncidentNoteId { get; set; }

    [DataMember, XmlAttribute]
    public string CreateDate { get; set; }

    [DataMember(Name = "Note"), XmlAttribute(AttributeName = "Note")]
    public string Text { get; set; }

    [DataMember, XmlAttribute]
    public string Origin { get; set; }
  }
}
