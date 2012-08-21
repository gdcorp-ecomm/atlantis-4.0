using System;
using Atlantis.Framework.Interface;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;

namespace Atlantis.Framework.FastballEventMetrics.Interface
{
  public class FastballEventMetricsRequestData : RequestData
  {
    private FESEventMessage eventMessage;

    public FESEventMessage EventMessage
    {
      get { return eventMessage; }
    }

    public FastballEventMetricsRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount, int privateLableId, string metricName, string metricValue)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {

      eventMessage = new FESEventMessage();
      eventMessage.metricDataSave.shopperId = shopperId;
      eventMessage.metricDataSave.requestGuid = string.Empty;
      eventMessage.metricDataSave.resources = new resource[1];

      eventMessage.metricDataSave.resources[0] = new resource
      {
        reourcetype = "PLID",
        id = privateLableId,
        metrics = new List<metric>(),
      };
      EventMessage.metricDataSave.resources[0].metrics.Add(
        new Atlantis.Framework.FastballEventMetrics.Interface.metric
        {
          name = metricName,
          value = metricValue,
          statusCode = "0",
          seqDateGmt = DateTime.Now.ToUniversalTime().ToString("yyyy/MM/dd HH:mm:ss"),
        });
    }
    
    public FastballEventMetricsRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount, int privateLableId, string metricName, string metricValue, string orionAccountUid, string billingResourceId)
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, privateLableId, metricName, metricValue)
    {
      eventMessage.metricDataSave.orionAccountUid = orionAccountUid;
      eventMessage.metricDataSave.resources[0].BillingResourceId = billingResourceId;
      eventMessage.metricDataSave.resources[0].BillingResourceId = orionAccountUid;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("Do not Implement Caching on Activation Data");
    }

    public string Message
    {
      get
      {
        return ObjectSerializer.SerializeToXml<FESEventMessage>(eventMessage);
      }
    }
  }
}
