using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Atlantis.Framework.Iris.Interface.Objects
{
  [DataContract(Name = "NotesByIncident")]
  [XmlRoot(ElementName = "NotesByIncident")]
  [XmlSerializerFormat]
  public class NotesList
  {
    public NotesList()
    {
      Notes = new Notes();
    }

    [DataMember(Name = "Notes"), XmlElement(ElementName = "Notes")]
    public Notes Notes { get; set; }
  }

  [DataContract(Name = "Notes")]
  [XmlRoot(ElementName = "Notes")]
  [XmlSerializerFormat]
  public class Notes
  {
    [DataMember(Name = "Item"), XmlElement(ElementName = "Item")]
    public List<Note> Items { get; set; }
  }

}
