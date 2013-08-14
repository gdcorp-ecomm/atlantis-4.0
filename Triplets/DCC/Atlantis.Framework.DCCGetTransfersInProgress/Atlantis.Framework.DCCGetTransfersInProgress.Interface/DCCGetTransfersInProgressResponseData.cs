using System;
using System.Collections.Generic;
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

    public DCCGetTransfersInProgressResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      ResponseXml = responseXml;
      _exception = new AtlantisException(requestData, "DCCGetTransfersInProgressResponseData", ex.Message, requestData.ToXML());
      IsSuccess = false;
    }

    public DomainTransferCollection TransferCollection
    {
      get
      {
        DomainTransferCollection xferCol = null;
        if (IsSuccess)
        {
          var doc = XDocument.Parse(ResponseXml);
          if (doc.Document != null)
          {
            var root = doc.Document.Element("results");
            if (root != null)
            {
              var xElement = root.Element("searchsummary");
              if (xElement != null)
              {
                xferCol = new DomainTransferCollection((bool)root.Element("success"), Convert.ToInt32(xElement.Attribute("resultcount").Value), Convert.ToInt32(xElement.Attribute("totalpages").Value));
              }
              var xDomains = root.Element("domains");

              if (xDomains != null)
              {
                //Note: Only the text descriptions uses by MYA have been Translated.  If you need the others, you'll have to translate them too.
                var q = from info in xDomains.Descendants("domain")
                        select new DomainTransfer
                        {
                          DomainId = info.Attribute("id").Value,
                          DomainName = info.Attribute("domainname").Value,
                          ErrorDescription = info.Attribute("errordesc").Value,
                          ExternalActionToTake = info.Attribute("externalactiontotake").Value,
                          ExternalShortActionToTake = GetExternalShortActionToTakeLanguageKey(Convert.ToInt32(info.Attribute("transferstepstatusid").Value)),
                          ExternalStatusDescription = info.Attribute("externalstatusdescription").Value,
                          ExternalStepDescription = GetExternalStepDescriptionLanguageKey(Convert.ToInt32(info.Attribute("transferstepid").Value)),
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
                          TransferType = GetTransferTypeLanguageKey(Convert.ToInt32(info.Attribute("transfertypeid").Value)),
                          TransferTypeId = Convert.ToInt32(info.Attribute("transfertypeid").Value)
                        };

                if (xferCol != null)
                {
                  foreach (DomainTransfer dt in q)
                  {
                    xferCol.DomainTransferList.Add(dt);
                  }
                }
              }
            }
          }
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

    #region Resource String Conversion
    
    #region TransferType
    private string GetTransferTypeLanguageKey(int transferTypeId)
    {
      string languageKey;
      if (!TransferTypeDict.TryGetValue(transferTypeId, out languageKey))
      {
        languageKey = string.Empty;
      }
      return languageKey;
    }

    private Dictionary<int, string> _transferTypeDict;
    private Dictionary<int, string> TransferTypeDict
    {
      get { return _transferTypeDict ?? (_transferTypeDict = BuildTransferTypeDict()); }
    }

    private static Dictionary<int, string> BuildTransferTypeDict()
    {
      return new Dictionary<int, string>
        {
          {1, "Dcc_WS_TransferType1"},  // English: TransferIn
          {2, "Dcc_WS_TransferType2"}   // English: TransferOut
        };
    }
    #endregion

    #region ExternalStepDescription
    private string GetExternalStepDescriptionLanguageKey(int transferStepId)
    {
      string languageKey;
      if (!ExternalStepDescriptionDict.TryGetValue(transferStepId, out languageKey))
      {
        languageKey = string.Empty;
      }
      return languageKey;
    }

    private Dictionary<int, string> _externalStepDescriptionDict;
    private Dictionary<int, string> ExternalStepDescriptionDict
    {
      get { return _externalStepDescriptionDict ?? (_externalStepDescriptionDict = BuildExternalStepDescriptionDict()); }
    }

    private static Dictionary<int, string> BuildExternalStepDescriptionDict()
    {
      return new Dictionary<int, string>
        {
          {1, "Dcc_WS_ExternalStep1Description"},  // English: Validating domain Whois information
          {2, "Dcc_WS_ExternalStep2Description"},  // English: Awaiting transfer authorization
          {3, "Dcc_WS_ExternalStep3Description"},  // English: Waiting for transfer acceptance
          {6, "Dcc_WS_ExternalStep6Description"}   // English: Accept/decline transfer
        };
    }
    #endregion

    #region ExternalShortActionToTake
    private string GetExternalShortActionToTakeLanguageKey(int transferStepStatusId)
    {
      string languageKey;
      if (!ExternalShortActionToTakeDict.TryGetValue(transferStepStatusId, out languageKey))
      {
        languageKey = string.Empty;
      }
      return languageKey;
    }

    private Dictionary<int, string> _externalShortActionToTakeDict;
    private Dictionary<int, string> ExternalShortActionToTakeDict
    {
      get { return _externalShortActionToTakeDict ?? (_externalShortActionToTakeDict = BuildExternalShortActionToTakeDict()); }
    }

    private static Dictionary<int, string> BuildExternalShortActionToTakeDict()
    {
      return new Dictionary<int, string>
        {
          {1, "Dcc_WS_ExternalShortActionToTake1"},    // English: Processing. No action needed.
          {2, "Dcc_WS_ExternalShortActionToTake2"},    // English: Unlock domain with current registrar.
          {3, "Dcc_WS_ExternalShortActionToTake3"},    // English: Awaiting eligibility date. No action needed.
          {4, "Dcc_WS_ExternalShortActionToTake1"},    // English: Processing. No action needed.
          {5, "Dcc_WS_ExternalShortActionToTake1"},    // English: Processing. No action needed.
          {6, "Dcc_WS_ExternalShortActionToTake6"},    // English: Enter the Transaction ID and Security Code.
          {7, "Dcc_WS_ExternalShortActionToTake1"},    // English: Processing. No action needed.
          {8, "Dcc_WS_ExternalShortActionToTake8"},    // English: Enter the correct Authorization Code.
          {9, "Dcc_WS_ExternalShortActionToTake9"},    // English: Resolve issues and re-initiate transfer.
          {10, "Dcc_WS_ExternalShortActionToTake10"},  // English: Accept transfer with current registrar.
          {11, "Dcc_WS_ExternalShortActionToTake1"},   // English: Processing. No action needed.
          {12, "Dcc_WS_ExternalShortActionToTake9"},  // English: Resolve issues and re-initiate transfer.
          {13, "Dcc_WS_ExternalShortActionToTake13"},  // English: Declined. No action needed.
          {14, "Dcc_WS_ExternalShortActionToTake14"},  // English: Accept or decline.
          {15, "Dcc_WS_ExternalShortActionToTake1"},   // English: Processing. No action needed.
          {16, "Dcc_WS_ExternalShortActionToTake1"},   // English: Processing. No action needed.
          {17, "Dcc_WS_ExternalShortActionToTake17"},  // English: Processing completed. No action needed.
          {22, "Dcc_WS_ExternalShortActionToTake14"},  // English: Accept or decline.
          {23, "Dcc_WS_ExternalShortActionToTake14"},  // English: Accept or decline.
          {24, "Dcc_WS_ExternalShortActionToTake24"},  // English: Remove privacy from domain.
          {25, "Dcc_WS_ExternalShortActionToTake25"}   // English: You must renew the domain.
        };
    }
    #endregion
      
    #endregion

  }
}
