using System;
using Atlantis.Framework.HCCAddDomain.Impl.HCCAPIWebService;
using Atlantis.Framework.HCCAddDomain.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCAddDomain.Impl
{
  public class HCCAddDomainRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      var apiRequestData = requestData as HCCAddDomainRequestData;

      try
      {
        var ws = new HCCAPIWebService.HCCAPIService { Url = ((WsConfigElement)config).WSURL };
        if (apiRequestData != null)
        {
          ws.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
          var apiResponse = ws.AddDomain(apiRequestData.AccountUid, GetDomainType(apiRequestData.DomainType),
                                         apiRequestData.ParentDomainName, apiRequestData.DomainToAdd,
                                         apiRequestData.Directory, apiRequestData.EnablePreviewDns,
                                         apiRequestData.PathIsSubdirectoryName);

          if (apiResponse != null)
          {
            responseData = new HCCAddDomainResponseData(apiResponse.Message, apiResponse.Status, apiResponse.StatusCode);
          }
          else
          {
            var ex = new AtlantisException(apiRequestData,
                                           "HCCAddDomainRequest.RequestHandler",
                                           "API Response is null",
                                           string.Empty);

            responseData = new HCCAddDomainResponseData(apiRequestData, ex);
          }
        }

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new HCCAddDomainResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HCCAddDomainResponseData(requestData, ex);
      }

      return responseData;
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
