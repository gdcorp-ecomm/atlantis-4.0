using System;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetTransfersInProgress.Interface
{
  public class DCCGetTransfersInProgressResponseData : IResponseData
  {

    private readonly AtlantisException _exception;
    public string ResponseXml { get; set; }
    public bool IsSuccess { get; private set; }

    public DCCGetTransfersInProgressResponseData(string responseXml)
    {
      ResponseXml = responseXml;
      IsSuccess = true;
    }

    public DCCGetTransfersInProgressResponseData(string responseXml, AtlantisException exAtlantis)
    {
      ResponseXml = responseXml;
      _exception = exAtlantis;
      IsSuccess = false;
    }

    public DCCGetTransfersInProgressResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      ResponseXml = responseXml;
      _exception = new AtlantisException(oRequestData,
                                   "DCCGetTransfersInProgressResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
      IsSuccess = false;
    }

    public DomainTransferCollection TransferCollection
    {
      get
      {
        DomainTransferCollection xferCol = null;
        if (IsSuccess)
        {
          XDocument doc = XDocument.Parse(ResponseXml);
          XElement root = doc.Document.Element("results");
          xferCol = new DomainTransferCollection((bool)root.Element("success")
              , Convert.ToInt32(root.Element("searchsummary").Attribute("resultcount").Value)
              , Convert.ToInt32(root.Element("searchsummary").Attribute("totalpages").Value));
          XElement xDomains = root.Element("domains");

          var q = from info in xDomains.Descendants("domain")
                  select new DomainTransfer
                  {
                    DomainId = info.Attribute("id").Value,
                    DomainName = info.Attribute("domainname").Value,
                    ErrorDescription = info.Attribute("errordesc").Value,
                    ExternalActionToTake = info.Attribute("externalactiontotake").Value,
                    ExternalShortActionToTake = info.Attribute("externalshortactiontotake").Value,
                    ExternalStatusDescription = info.Attribute("externalstatusdescription").Value,
                    ExternalStepDescription = info.Attribute("externalstepdescription").Value,
                    ExternalStepName = info.Attribute("externalstepname").Value,
                    InternalActionToTake = info.Attribute("internalactiontotake").Value,
                    InternalShortActionToTake = info.Attribute("internalshortactiontotake").Value,
                    InternalStatusDescription = info.Attribute("internalstatusdescription").Value,
                    InternalStepDescription = info.Attribute("internalstepdescription").Value,
                    InternalStepName = info.Attribute("internalstepname").Value,
                    IntlDomaiNname = info.Attribute("intldomainname").Value,
                    Status = info.Attribute("status").Value,
                    StepCount = Convert.ToInt32(info.Attribute("stepcount").Value),
                    StepNumber = Convert.ToInt32(info.Attribute("stepnumber").Value),
                    TransferStepId = Convert.ToInt32(info.Attribute("transferstepid").Value),
                    TransferStepStatusId = Convert.ToInt32(info.Attribute("transferstepstatusid").Value),
                    TransferType = info.Attribute("transfertype").Value,
                    TransferTypeId = Convert.ToInt32(info.Attribute("transfertypeid").Value)
                  };

          foreach (DomainTransfer d in q)
            xferCol.DomainTransferList.Add(d);
        }
        return xferCol;
      }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return ResponseXml;
    }

  }
}
