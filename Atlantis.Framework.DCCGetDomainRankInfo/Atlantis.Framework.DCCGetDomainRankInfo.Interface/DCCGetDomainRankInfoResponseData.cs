using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.DCCGetDomainRankInfo.Interface
{
  public class DCCGetDomainRankInfoResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception;
    private string _resultXml = string.Empty;
    private bool _success;

    public Dictionary<int, DomainRankInfo> DomainRankInfos { get; private set; }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public DCCGetDomainRankInfoResponseData()
    { }

    public DCCGetDomainRankInfoResponseData(string xml)
    {
      _success = true;
      _resultXml = xml;
      DomainRankInfos = new Dictionary<int, DomainRankInfo>();

      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(xml);
      XmlNodeList domains = xdoc.SelectNodes("results/domains/domain");

      foreach (XmlNode domain in domains)
      {
        DomainRankInfo dri = new DomainRankInfo();
        int domainId = Convert.ToInt32(domain.Attributes["id"].Value);
        dri.ShopperId = domain.Attributes["shopperId"].Value;
        dri.DomainDiagnosticUrl = domain.Attributes["mainUrl"].Value;
        dri.DomainName = domain.Attributes["name"].Value;
        dri.ProcessingStatus = domain.Attributes["processing"].Value.ToLowerInvariant() == "notfound" ? 
          DomainRankInfo.ProcessingStatusType.NotFound : DomainRankInfo.ProcessingStatusType.Success;

        if (dri.ProcessingStatus == DomainRankInfo.ProcessingStatusType.Success)
        {
          dri.DateScored = Convert.ToDateTime(domain.Attributes["dateScored"].Value);
          string rank = domain.Attributes["rank"].Value.ToLowerInvariant();
          switch (rank)
          {
            case "red":
              dri.Rank = DomainRankInfo.RankType.Red;
              break;
            case "yellow":
              dri.Rank = DomainRankInfo.RankType.Yellow;
              break;
            case "green":
              dri.Rank = DomainRankInfo.RankType.Green;
              break;
          }
        }
        DomainRankInfos.Add(domainId, dri);
      }
    }

    public DCCGetDomainRankInfoResponseData(DCCGetDomainRankInfoRequestData request, string xml)
    {
      string data = string.Empty;
      XmlDocument xdoc = new XmlDocument();

      xdoc.LoadXml(xml);
      XmlNode error = xdoc.SelectSingleNode("results/error");
      XmlNode inputXml = xdoc.SelectSingleNode("results/inputXml");
      XmlNode server = xdoc.SelectSingleNode("results/server");

      if (error != null)
      {
        data = string.Format("Server: {0} Code: {1} | Details: {2} | InputXml: {3}"
          , server.LastChild.Value
          , error.FirstChild.InnerText
          , error.LastChild.InnerText
          , inputXml.InnerXml);
      }

      _exception = new AtlantisException(request
        , "DCCGetDomainRankInfoResponseData"
        , "Web Service Failure (method: GetRankInfoForDomainIds)"
        , data);
    }

    public DCCGetDomainRankInfoResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public DCCGetDomainRankInfoResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "DCCGetDomainRankInfoResponseData"
        , exception.Message
        , requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return _resultXml;
    }

    public void DeserializeSessionData(string sessionData)
    {
      _resultXml = sessionData;
    }
    #endregion
  }
}
