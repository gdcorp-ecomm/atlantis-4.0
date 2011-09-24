using System;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCEditDomain.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCEditDomain.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCEditDomain.Impl
{
  public class HCCEditDomainRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      var apiRequestData = requestData as HCCEditDomainRequestData;

      try
      {
        var ws = new HCCAPIWebService.HCCAPIService { Url = ((WsConfigElement)config).WSURL };
        if (apiRequestData != null)
        {
          ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
          var apiResponse = ws.EditDomain(apiRequestData.AccountUid, GetDomainType(apiRequestData.DomainType),
                                          apiRequestData.ParentDomainName, apiRequestData.NewSubDomainName,
                                          apiRequestData.DomainToEdit, apiRequestData.NewPath);

          if (apiResponse != null)
          {
            responseData = new HCCEditDomainResponseData(GetHCCResponse(apiResponse));
          }
          else
          {
            var ex = new AtlantisException(apiRequestData,
                                           "HCCEditDomainRequest.RequestHandler",
                                           "API Response is null",
                                           string.Empty);

            responseData = new HCCEditDomainResponseData(apiRequestData, ex);
          }
        }

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new HCCEditDomainResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HCCEditDomainResponseData(requestData, ex);
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
