using System;
using Atlantis.Framework.DomainContactValidation.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainContactValidation.Impl
{
  public class DomainContactValidationRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      #region IRequest Members

      IResponseData responseData = null;

      string responseXml = string.Empty;

      try
      {
        using (var contactValidationService = new ContactValidationService.ContactValidationService())
        {
          contactValidationService.Url = ((WsConfigElement)config).WSURL;
          contactValidationService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          responseXml = contactValidationService.Validate(requestData.ToXML());
        }

        if (!string.IsNullOrEmpty(responseXml))
        {
          responseData = new DomainContactValidationResponseData(responseXml);
        }

        if (responseData == null)
        {
          throw new AtlantisException(requestData,
                                      "DomainContactValidationRequest.RequestHandler",
                                      "Invalid request, (null) string returned",
                                      string.Empty);
        }

      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DomainContactValidationResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DomainContactValidationResponseData(responseXml, requestData, ex);
      }

      return responseData;

      #endregion
    }
  }
}
