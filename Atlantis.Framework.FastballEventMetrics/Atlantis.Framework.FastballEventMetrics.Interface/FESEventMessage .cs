using System.Collections.Generic;
using System.Xml.Serialization;

namespace Atlantis.Framework.FastballEventMetrics.Interface
{

  public class FESEventMessage
  {
    [XmlAttribute(AttributeName = "type")]
    public string MDStype { get; set; }

    [XmlAttribute(AttributeName = "namespace")]
    public string MDSnamespace { get; set; }

    [XmlElement(ElementName = "metricDataSave")]
    public metricDataSave metricDataSave { get; set; }

    public FESEventMessage()
    {
      MDStype = "MetricDataSave";
      MDSnamespace = "fbpms";
      
      metricDataSave = new metricDataSave
      {
        resources = new resource[1]
      };
    }
  }

  public class metricDataSave 
  {

    [XmlAttribute(AttributeName = "shopperId")]
    public string shopperId { get; set; }
    [XmlAttribute(AttributeName = "dataSource")]
    public string dataSource { get; set; }
    [XmlAttribute(AttributeName = "requestGuid")]
    public string requestGuid { get; set; }

    [XmlAttribute(AttributeName = "orionAccountUid")]
    public string orionAccountUid { get; set; }

    [XmlArray(ElementName = "resources")]
    public resource[] resources { get; set; }

  }

  public class resource
  {
    [XmlAttribute(AttributeName = "id")]
    public int id { get; set; }
    [XmlAttribute(AttributeName = "type")]
    public string reourcetype { get; set; }

    [XmlAttribute(AttributeName = "orionAccountUid")]
    public string orionAccountUid { get; set; }

    [XmlAttribute(AttributeName = "billingResourceId")]
    public string BillingResourceId { get; set; }

    public List<metric> metrics { get; set; }
  }

  public class metric
  {
    [XmlAttribute(AttributeName="name")]
    public string name { get; set; }
    [XmlAttribute(AttributeName = "value")]
    public string value { get; set; }
    [XmlAttribute(AttributeName = "statusCode")]
    public string statusCode { get; set; }
    [XmlElement(ElementName = "supportData")]
    public string supportData { get; set; }
    [XmlAttribute(AttributeName = "seqDateGmt")]
    public string seqDateGmt { get; set; }
  }

}
