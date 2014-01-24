using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Atlantis.Framework.Iris.Interface.Objects
{
  [DataContract, XmlSerializerFormat]
  public class Incident
  {
    [DataMember, XmlAttribute(AttributeName = "IncidentId")]
    public long IncidentId { get; set; }

    [DataMember, XmlAttribute(AttributeName = "IncidentDescription")]
    public string IncidentDescription { get; set; }

    [DataMember, XmlAttribute(AttributeName = "StatusId")]
    public int StatusId { get; set; }

    [DataMember, XmlAttribute(AttributeName = "Status")]
    public string Status { get; set; }

    [DataMember, XmlAttribute(AttributeName = "NoteId")]
    public int NoteId { get; set; }


    private string _modDate = string.Empty;
    [DataMember, XmlAttribute(AttributeName = "ModDate")]
    public string ModDate {
      get
      {
        return ModifiedDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");
      }
      set
      {
        _modDate = value;
      }

    }

    [XmlIgnore]
    private DateTime ModifiedDate
    {
      get
      {
        DateTime outDate;
        DateTime.TryParse(_modDate, out outDate);
        outDate = DateTime.SpecifyKind(outDate, DateTimeKind.Local);
        
        return outDate;
      }
    }

    [DataMember, XmlElement(ElementName = "Notes")]
    public List<Note> Notes { get; set; }

  }
}
