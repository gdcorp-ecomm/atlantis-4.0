using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using System;
using System.Runtime.Serialization;
using Atlantis.Framework.Iris.Interface.Objects;

namespace Atlantis.Framework.Iris.Interface
{
  [DataContract]
  public class GetIncidentCustomerNotesResponseData : ResponseData
  {
   public static GetIncidentCustomerNotesResponseData FromData(string data)
    {
      return new GetIncidentCustomerNotesResponseData(data);
    }

    private GetIncidentCustomerNotesResponseData(string data)
    {
      try
      {
        RawXml = data;

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);
        var ser = new XmlSerializer(typeof (NotesByIncident));
        if (doc.DocumentElement != null)
        {
          var wrapper = (NotesByIncident)ser.Deserialize(new StringReader(doc.DocumentElement.OuterXml));
          Notes = wrapper.ConvertToNotesList();
        }
      }
      catch (Exception ex)
      {
        IsSuccess = false;
      }
      IsSuccess = true;
    }

    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember]
    public List<Note> Notes { get; set; }

    public string RawXml { get; set; }

  }
}
