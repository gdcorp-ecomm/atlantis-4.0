using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.HCCAvailableSSL.Interface;
using Atlantis.Framework.HCCAvailableSSL.Impl.HCCAPIWebService;
using System.Collections.ObjectModel;
using Atlantis.Framework.HCC.Interface;

namespace Atlantis.Framework.HCCAvailableSSL.Impl
{
  public class HCCAvailableSSLRequest : IRequest
  {
      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData responseData = null;
        HCCAvailableSSLRequestData apiRequestData = requestData as HCCAvailableSSLRequestData;

        try
        {
          HCCAPIService ws = new HCCAPIWebService.HCCAPIService();
          ws.Url = ((WsConfigElement)config).WSURL;
          ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
          HCCAPIWebService.SSLCertificatesResponse apiResponse = ws.GetAvailableSSLCertificatesInfo(apiRequestData.AccountUid);

          if (apiResponse != null)
          {
            responseData = new HCCAvailableSSLResponseData(GetHCCResponse(apiResponse));
          }
          else
          {
            AtlantisException ex = new AtlantisException(apiRequestData,
              "HCCAvailableSSLRequest.RequestHandler",
              "API Response is null or AccountList is null",
              string.Empty);

            responseData = new HCCAvailableSSLResponseData(apiRequestData, ex);
          }
        }
        catch (Exception ex)
        {
          responseData = new HCCAvailableSSLResponseData(apiRequestData, ex);
        }

        return responseData;
      }

      HCCAvailableSSLResponse GetHCCResponse(HCCAPIWebService.SSLCertificatesResponse apiResponse)
      {
        HCCAvailableSSLResponse response = new HCCAvailableSSLResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);

        response.Enabled = apiResponse.Enabled;
        response.HelpArticleID = apiResponse.HelpArticleID;
        response.InstructionText = apiResponse.InstructionText ?? string.Empty;
        response.SubscriberAgreementUrl = apiResponse.SubscriberAgreementUrl ?? string.Empty;

        if (apiResponse.SSLCreditListItems != null)
        {
          response.SetHCCOptionGroups(GetHCCSSLItemList(apiResponse.SSLCreditListItems));
        }

        return response;
      }

      ReadOnlyCollection<HCCSSLItem> GetHCCSSLItemList(SSLCreditListItem[] sslItemList)
      {
        List<HCCSSLItem> hccSslItemList = new List<HCCSSLItem>(sslItemList.Length);

        foreach (SSLCreditListItem sslItem in sslItemList)
        {
          if (sslItem != null)
          {
            HCCSSLItem item = new HCCSSLItem();
            item.DisplayText = sslItem.DisplayText ?? string.Empty;
            item.Value = sslItem.Value ?? string.Empty;

            hccSslItemList.Add(item);
          }

        }

        return hccSslItemList.AsReadOnly();
      }
  }
}
