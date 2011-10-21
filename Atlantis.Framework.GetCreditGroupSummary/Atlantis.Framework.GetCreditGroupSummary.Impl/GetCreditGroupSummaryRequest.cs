using System;
using System.Xml;
using Atlantis.Framework.GetCreditGroupSummary.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetCreditGroupSummary.Impl
{
  public class GetCreditGroupSummaryRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetCreditGroupSummaryRequestData requestData = (GetCreditGroupSummaryRequestData)oRequestData;
      GetCreditGroupSummaryResponseData responseData = new GetCreditGroupSummaryResponseData();
      string response = string.Empty;
      try
      {
        using (CMS.WSCgdCMSService cms = new CMS.WSCgdCMSService())
        {
          cms.Url = ((WsConfigElement)oConfig).WSURL;
          response = cms.GetCreditGroupSummary(requestData.ShopperID, requestData.DisplayGroupID, 0);
        }

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(response);

        if(string.Compare(doc.SelectSingleNode("/RESPONSE/MESSAGE").InnerText, "Success", true) != 0)
        {
          string data = string.Format("DisplayGroupID: {0}, Response: {1}", requestData.DisplayGroupID, response);
          responseData.AtlException = new AtlantisException(requestData,
            "GetCreditGroupSummaryRequest.RequestHandler", "Could not retrieve summary of credit group", data);
        }
        else
        {
          responseData.XML = doc.SelectSingleNode("/RESPONSE/CREDITS").OuterXml;
        }
      }
      catch (Exception ex)
      {
        string data = string.Format("DisplayGroupID: {0}", requestData.DisplayGroupID);
        responseData.AtlException = new AtlantisException(requestData,
          "GetCreditGroupSummaryRequest.RequestHandler", "Could not retrieve summary of credit group", data, ex);
      }

      return responseData;
    }
  }
}
