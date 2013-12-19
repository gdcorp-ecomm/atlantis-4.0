using Atlantis.Framework.Interface;
using System;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Iris.Interface
{
  [DataContract]
  public class CreateIncidentResponseData : IResponseData
  {
    private AtlantisException atlEx = null;

    public static CreateIncidentResponseData FromData(long data)
    {
      return new CreateIncidentResponseData(data);
    }

    private CreateIncidentResponseData(long responseCode)
    {
      IncidentId = responseCode;
      IsSuccess = IncidentId != 0;
    }

    public CreateIncidentResponseData(CreateIncidentRequestData request, Exception ex)
    {
      atlEx = new AtlantisException(request, "CreateIncident", ex.Message, string.Empty);
      IsSuccess = false;
    }

    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember]
    public long IncidentId { get; set; }

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
