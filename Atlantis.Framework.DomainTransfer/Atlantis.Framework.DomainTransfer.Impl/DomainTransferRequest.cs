using System;
using System.Net;
using Atlantis.Framework.DomainTransfer.Impl.AvailCheckWS;
using Atlantis.Framework.DomainTransfer.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainTransfer.Impl
{
  public class DomainTransferRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      string sResponseXML = string.Empty;

      try
      {
        DomainTransferRequestData oDomainTransferRequestData = (DomainTransferRequestData)oRequestData;

        AvailCheckWebSvc availCheckService = new AvailCheckWebSvc();
        availCheckService.Url = ((WsConfigElement)oConfig).WSURL;
        availCheckService.Timeout = (int)oDomainTransferRequestData.RequestTimeout.TotalMilliseconds;

        sResponseXML = availCheckService.Check(oDomainTransferRequestData.ToXML());
        if (sResponseXML == null)
        {
          throw new Exception("AvailCheck returned null response.");
        }

        oResponseData = new DomainTransferResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainTransferResponseData(sResponseXML, exAtlantis);
      }
      catch (WebException exWeb)
      {
        oResponseData = new DomainTransferResponseData(exWeb.Status);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainTransferResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
