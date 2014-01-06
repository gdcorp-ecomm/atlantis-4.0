using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Iris.Interface.Objects;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Atlantis.Framework.Iris.Interface
{
  [DataContract]
  public class GetIncidentsByShopperIdAndDateRangeResponseData : IResponseData
  {
    private AtlantisException atlEx = null;

    public static GetIncidentsByShopperIdAndDateRangeResponseData FromData(string data)
    {
      return new GetIncidentsByShopperIdAndDateRangeResponseData(data);
    }

    private GetIncidentsByShopperIdAndDateRangeResponseData(string data)
    {
      RawXml = data;

      //deserialize xml into object list
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);
        var ser = new XmlSerializer(typeof(IncidentsForShoppers));
        if (doc.DocumentElement != null)
        {
          var wrapper = (IncidentsForShoppers)ser.Deserialize(new StringReader(doc.DocumentElement.OuterXml));
          Incidents = wrapper.ConvertToIncidentsList();
        }
      }
      catch (Exception ex)
      {
        IsSuccess = false;
        atlEx = new AtlantisException(null, "GetIncidentsByShopperIdAndDateRangeResponseData", ex.Message, string.Empty);
      }
      IsSuccess = true;
    }
    
    public GetIncidentsByShopperIdAndDateRangeResponseData(GetIncidentsByShopperIdAndDateRangeRequestData request, Exception ex)
    {
      atlEx = new AtlantisException(request, "GetIncidentsByShopperIdAndDateRange", ex.Message, string.Empty);
      IsSuccess = false;
    }

    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember]
    public List<Incident> Incidents { get; set; }

    public string RawXml { get; set; }

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
