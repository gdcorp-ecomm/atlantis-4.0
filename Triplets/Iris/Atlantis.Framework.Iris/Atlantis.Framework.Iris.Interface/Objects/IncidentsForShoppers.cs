using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Atlantis.Framework.Iris.Interface.Objects
{
  [DataContract(Name = "IncidentsForShoppers"),
   XmlRoot(ElementName = "IncidentsForShoppers"),
  XmlSerializerFormat]
  public class IncidentsForShoppers
  {
    public IncidentsForShoppers()
    {
      Incidents = new ShopperIncidents();
    }

    [DataMember(Name = "Incidents"), XmlElement(ElementName = "Incidents")]
    public ShopperIncidents Incidents { get; set; }

    public List<Incident> ConvertToIncidentsList()
    {
      return this.Incidents.Items ?? new List<Incident>();
    }

  }

  [DataContract(Name = "Incidents")]
  [XmlRoot(ElementName = "Incidents")]
  [XmlSerializerFormat]
  public class ShopperIncidents
  {
    [DataMember(Name = "Item"), XmlElement(ElementName = "Item")]
    public List<Incident> Items { get; set; }
  }



}

