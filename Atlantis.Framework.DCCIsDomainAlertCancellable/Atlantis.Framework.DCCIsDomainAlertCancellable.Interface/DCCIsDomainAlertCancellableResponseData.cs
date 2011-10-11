using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCIsDomainAlertCancellable.Interface
{
  public class DCCIsDomainAlertCancellableResponseData : IResponseData
  {
    private AtlantisException _exception;
    private string _resultXML = string.Empty;
    private bool _success;
    public Dictionary<int, bool> IsCancellableDict { get; private set; }
    public Dictionary<int, UncancellableReasonInfo> UncancellableReasonDict { get; private set; }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public DCCIsDomainAlertCancellableResponseData(string xml)
    {
      _success = true;
      _resultXML = xml;

      IsCancellableDict = new Dictionary<int, bool>();
      UncancellableReasonDict = new Dictionary<int, UncancellableReasonInfo>();
      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(xml);
      XmlNodeList alerts = xdoc.SelectNodes("success/domainalerts/domainalert");

      int i;
      bool b;
      foreach (XmlNode alert in alerts)
      {
        bool cancelAttributeCheck = alert.Attributes["iscancellable"].Value.Equals("0") || alert.Attributes["iscancellable"].Value.Equals("1");
        if (int.TryParse(alert.Attributes["billingid"].Value, out i) && cancelAttributeCheck)
        {
          b = alert.Attributes["iscancellable"].Value.Equals("1") ? true : false;

          IsCancellableDict.Add(i, b);

          // Capture reason why resource is not cancellable
          if (!b)
          {
            UncancellableReasonInfo reason = new UncancellableReasonInfo();
            XmlNode reasonNode = alert.SelectSingleNode("backorder");

            reason.DomainBackorderStatusId = reasonNode.Attributes["domainbackorderstatusid"].Value;
            reason.DomainMonitorBillingId = reasonNode.Attributes["domainmonitorbillingid"].Value;
            reason.DomainMonitorId = reasonNode.Attributes["domainmonitorid"].Value;
            reason.DomainName = reasonNode.Attributes["domainname"].Value;
            reason.Error = reasonNode.Attributes["error"].Value;

            UncancellableReasonDict.Add(i, reason);
          }
        }
      }
    }

    public DCCIsDomainAlertCancellableResponseData(DCCIsDomainAlertCancellableRequestData request, string xml)
    {
      string data = string.Empty;
      XmlDocument xdoc = new XmlDocument();

      xdoc.LoadXml(xml);
      XmlNode error = xdoc.SelectSingleNode("error");

      if (error != null)
      {
        data = string.Format("Method: {0} | CallGuid: {1} | Message: {2} | Server: {3}", error.Attributes["method"].Value, error.Attributes["callguid"].Value, error.Attributes["message"].Value, error.Attributes["server"].Value);
      }

      _exception = new AtlantisException(request
        , "DCCIsDomainAlertCancellableResponseData"
        , "Web Service Failure"
        , data);
    }

    public DCCIsDomainAlertCancellableResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public DCCIsDomainAlertCancellableResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "DCCIsDomainAlertCancellableResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
