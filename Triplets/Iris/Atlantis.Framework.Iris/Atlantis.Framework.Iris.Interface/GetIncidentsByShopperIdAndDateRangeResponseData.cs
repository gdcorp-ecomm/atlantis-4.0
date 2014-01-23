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
  public class GetIncidentsByShopperIdAndDateRangeResponseData : ResponseData
  {

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
      }
      IsSuccess = true;
    }
    
    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember]
    public List<Incident> Incidents { get; set; }

    public string RawXml { get; set; }

  }
}
