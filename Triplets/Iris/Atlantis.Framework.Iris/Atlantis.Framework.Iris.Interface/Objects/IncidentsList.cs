using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Atlantis.Framework.Iris.Interface.Objects
{
  [DataContract(Name = "IncidentsForShoppers")]
  [XmlRoot(ElementName = "IncidentsForShoppers")]
  [XmlSerializerFormat]
  public class IncidentsList
  {
    public IncidentsList()
    {
      Incidents = new Incidents();
    }

    [DataMember(Name = "Incidents"), XmlElement(ElementName = "Incidents")]
    public Incidents Incidents { get; set; }
  }

  [DataContract(Name = "Incidents")]
  [XmlRoot(ElementName = "Incidents")]
  [XmlSerializerFormat]
  public class Incidents
  {
    [DataMember(Name = "Item"), XmlElement(ElementName = "Item")]
    public List<Incident> Items { get; set; }
  }
}
