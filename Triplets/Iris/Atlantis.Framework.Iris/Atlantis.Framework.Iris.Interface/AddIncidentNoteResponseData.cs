using Atlantis.Framework.Interface;
using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Iris.Interface
{
  [DataContract]
  public class AddIncidentNoteResponseData : IResponseData
  {
    private AtlantisException atlEx = null;

    public static AddIncidentNoteResponseData FromData(long data)
    {
      return new AddIncidentNoteResponseData(data);
    }

    private AddIncidentNoteResponseData(long responseCode)
    {
      IncidentNoteId = responseCode;

      IsSuccess = IncidentNoteId != 0;
    }

    public AddIncidentNoteResponseData(AddIncidentNoteRequestData request, Exception ex)
    {
      atlEx = new AtlantisException(request, "AddIncidentNoteResponseData", ex.Message, string.Empty);
      IsSuccess = false;
    }

    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember]
    public long IncidentNoteId { get; set; }

    public string ToXML()
    {
      string xml;
      try
      {
        var serializer = new DataContractSerializer(this.GetType());
        using (var backing = new System.IO.StringWriter())
        using (var writer = new System.Xml.XmlTextWriter(backing))
        {
          serializer.WriteObject(writer, this);
          xml = backing.ToString();
        }
      }
      catch (Exception)
      {
        xml = string.Empty;
      }
      return xml;
    }

    public AtlantisException GetException()
    {
      return atlEx;
    }
  }
}
