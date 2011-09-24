using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCRemoveDomain.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCRemoveDomain.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCRemoveDomain.Impl
{
  public class HCCRemoveDomainRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      var apiRequestData = requestData as HCCRemoveDomainRequestData;

      try
      {
        var ws = new HCCAPIWebService.HCCAPIService { Url = ((WsConfigElement)config).WSURL };
        if (apiRequestData != null)
        {
          ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
          var apiResponse = ws.RemoveDomain(apiRequestData.AccountUid, GetDomainType(apiRequestData.DomainType),
                                            apiRequestData.ParentDomainName, apiRequestData.DomainToDelete);

          if (apiResponse != null)
          {
            responseData = new HCCRemoveDomainResponseData(GetHCCResponse(apiResponse));
          }
          else
          {
            var ex = new AtlantisException(apiRequestData,
                                           "HCCRemoveDomainRequest.RequestHandler",
                                           "API Response is null",
                                           string.Empty);

            responseData = new HCCRemoveDomainResponseData(apiRequestData, ex);
          }
        }

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new HCCRemoveDomainResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HCCRemoveDomainResponseData(requestData, ex);
      }

      return responseData;
    }

    static HCCDomainMgmtResponse GetHCCResponse(DomainManagementResponse apiResponse)
    {
      var response = new HCCDomainMgmtResponse(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode)
      {
        AccountDomainsXml = apiResponse.AccountDomains,
        DomainNameChangeAllowed = apiResponse.DomainNameChangeAllowed,
        ModifiedDomains = apiResponse.ModifiedDomains,
        Warnings = apiResponse.Warnings
      };

      return response;
    }

    static DomainType GetDomainType(string domainType)
    {
      var apiDomainType = DomainType.Domain;
      try
      {
        apiDomainType = (DomainType)Enum.Parse(typeof(DomainType), domainType, true);
      }
      catch (Exception)
      {
        throw new ArgumentException(
          "The domaintType argument \"{0}\" is invalid. Valid domain types are located in the Atlantis.Framework.HCC.Interface.Constants namespace.",
          domainType);
      }

      return apiDomainType;
    }
  }
}
