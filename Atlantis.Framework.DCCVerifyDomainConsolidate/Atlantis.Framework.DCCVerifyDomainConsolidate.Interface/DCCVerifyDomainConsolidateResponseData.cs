using System;
using System.Xml;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.DCCVerifyDomainConsolidate.Interface
{
  public class DCCVerifyDomainConsolidateResponseData : IResponseData
  {
    RequestData _requestData;
    string _responseXml;
    public AtlantisException _exception;

    public bool IsSuccess { get; private set; }
    public Dictionary<int, int> DomainActionResults { get; private set; }
    public Dictionary<int, string> ActionResultLookup { get; private set; }

    public DCCVerifyDomainConsolidateResponseData(string responseXML, RequestData requestData)
    {
      DomainActionResults = new Dictionary<int, int>();
      ActionResultLookup = new Dictionary<int, string>();

      _requestData = requestData;
      _responseXml = responseXML;
      ParseResponseXML();
    }

    public DCCVerifyDomainConsolidateResponseData(string responseXML, RequestData requestData, AtlantisException exAtlantis) : this(responseXML, requestData)
    {
      _exception = exAtlantis;
    }

    private void ParseResponseXML()
    {
      IsSuccess = false;

      XElement verificationXElement = null;

      try
      {
        verificationXElement = XElement.Parse(_responseXml);
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException(_requestData,
                             "DCCVerifyDomainConsolidateResponseData ParseResponseXML",
                             "Could not parse the response xml from the VerifyConsolidate webmethod. " + ex.Message,
                             _requestData.ToXML());
        verificationXElement = null;
      }

      if (verificationXElement != null)
      {
        IsSuccess = (verificationXElement.Attributes("result").Any() && (string)verificationXElement.Attribute("result") == "success");

        if (IsSuccess)
        {
          IEnumerable<XElement> domainElements
            = from domain in verificationXElement.Descendants("DOMAINS").Elements("DOMAIN")
              where domain.Attributes("id").Any() &&
              domain.Attributes("ActionResultID").Any()
              select domain;

          foreach (XElement domainElement in domainElements)
          {
            string domainIdString = domainElement.Attribute("id").Value;
            int domainId;
            if (int.TryParse(domainIdString, out domainId))
            {
              string actionIdString = domainElement.Attribute("ActionResultID").Value;
              int actionId;
              if (int.TryParse(actionIdString, out actionId))
              {
                DomainActionResults[domainId] = actionId;
              }
            }
          }

          IEnumerable<XElement> actionResultElements
            = from domain in verificationXElement.Descendants("ACTIONRESULTS").Elements("ACTIONRESULT")
              where domain.Attributes("ActionResultID").Any() &&
              domain.Attributes("Description").Any()
              select domain;

          foreach (XElement actionResultElement in actionResultElements)
          {
            string actionIdString = actionResultElement.Attribute("ActionResultID").Value;
            int actionId;
            if (int.TryParse(actionIdString, out actionId))
            {
              ActionResultLookup[actionId] = actionResultElement.Attribute("Description").Value;
            }
          }
        }
      }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      return _responseXml;
    }

    #endregion
  }
}
