using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Atlantis.Framework.Iris.Interface.Objects
{
  [XmlSerializerFormat]
  public class Note
  {
    [DataMember, XmlAttribute]
    public long IncidentId { get; set; }

    [DataMember, XmlAttribute]
    public long IncidentNoteId { get; set; }

    private string _createDate = string.Empty;
    [DataMember, XmlAttribute(AttributeName = "CreateDate")]
    public string CreateDate
    {
      get
      {
        return DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");
      }
      set
      {
        _createDate = value;
      }

    }

    [XmlIgnore]
    private DateTime DateCreated
    {
      get
      {
        DateTime outDate;
        DateTime.TryParse(_createDate, out outDate);
        outDate = DateTime.SpecifyKind(outDate, DateTimeKind.Local);

        return outDate;
      }
    }





    [DataMember(Name = "Note"), XmlAttribute(AttributeName = "Note")]
    public string Text { get; set; }

    [DataMember, XmlAttribute]
    public string Origin { get; set; }
  }
}
