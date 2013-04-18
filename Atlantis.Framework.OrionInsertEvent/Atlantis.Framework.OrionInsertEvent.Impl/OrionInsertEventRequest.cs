using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OrionEvent.Interface;
using Atlantis.Framework.OrionEvent.Impl.OrionEventWS;

namespace Atlantis.Framework.OrionEvent.Impl
{
  public class OrionInsertEventRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      OrionInsertEventResponseData responseData;

      try
      {
        string[] results;
        string[] errors;
        var request = (OrionInsertEventRequestData) requestData;
        var wsUrl = ((WsConfigElement)config).WSURL;

        int wsResponseCode;
        using (var orionEventWs = new EventQueue())
        {
          orionEventWs.Url = wsUrl;
          orionEventWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          var orionEvents = CreateOrionEvents(request);
          wsResponseCode = orionEventWs.InsertEvents(orionEvents, out results, out errors);
        }
        if (wsResponseCode.Equals(0) && (errors == null || errors[0] == null))
        {
          var responseCode = results.Length > 0 ? results[0] : "0";
          responseData = new OrionInsertEventResponseData(responseCode);
        }
        else
        {
          var error = string.Empty;
          try
          {
            var xDoc = XDocument.Parse(errors[0]);
            error = xDoc.Element("err").Element("message").Value;
          }
          catch
          {
            error = string.Empty;
          }
          finally
          {
            var data = string.Format("Error invoking OrionEvent.  Result Code: {0}", wsResponseCode);
            var aex = new AtlantisException(requestData, "OrionInsertEventRequest::RequestHandler", error, data);
            responseData = new OrionInsertEventResponseData(aex);
          }
        }
      }

      catch (Exception ex)
      {
        responseData = new OrionInsertEventResponseData(requestData, ex);
      }

      return responseData;
    }

    private static Event[] CreateOrionEvents(OrionInsertEventRequestData request)
    {
      var resellerEventItem = new EventItem {ItemType = "RESELLER_ID", Value = request.PrivateLabelId};
      var shopperEventItem = new EventItem {ItemType = "CUSTOMER_NUM", Value = request.ShopperID};
      var productEventItem = new EventItem {ItemType = request.ProductItemType, Value = request.ProductItemTypeValue};
      var eventItems = new List<EventItem> {resellerEventItem, shopperEventItem, productEventItem};

      var orionEvent = new Event
                         {
                           EventType = request.EventType,
                           Requestor = request.Requestor,
                           AuditMessage = request.AuditMessage,
                           EventItems = eventItems.ToArray()
                         };

      var events = new List<Event> {orionEvent};

      return events.ToArray();
    }
  }
}
