using System;

using Atlantis.Framework.DomainContactCheck.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainContactCheck.Impl
{
  public class DomainContactCheckRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      string responseXml = string.Empty;

      try
      {
        using (var contactValidationService = new ContactValidationService.ContactValidationService())
        {
          contactValidationService.Url = ((WsConfigElement)oConfig).WSURL;
          contactValidationService.Timeout = (int) oRequestData.RequestTimeout.TotalMilliseconds;
          
          responseXml = contactValidationService.Validate(oRequestData.ToXML());
        }

        if (!string.IsNullOrEmpty(responseXml) )
        {
          oResponseData = new DomainContactCheckResponseData(responseXml);
        }

        if (oResponseData == null)
        {
          throw new AtlantisException(oRequestData,
                                      "DomainContactCheckRequest.RequestHandler",
                                      "Invalid request, (null) string returned",
                                      string.Empty);
        }

      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainContactCheckResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainContactCheckResponseData(responseXml, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
