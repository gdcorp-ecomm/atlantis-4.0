using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.RegRegistryPartnersData.Interface;

namespace Atlantis.Framework.RegRegistryPartnersData.Impl
{
  public class RegRegistryPartnersDataRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData = null;
      string responseXML = string.Empty;

      RegistryPartnersData.RegistryPartnersData registryPartnersDataWebService = null;
      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement)oConfig;
        registryPartnersDataWebService = new RegistryPartnersData.RegistryPartnersData();
        registryPartnersDataWebService.Url = wsConfigElement.WSURL;
        registryPartnersDataWebService.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);

        string inputXml = oRequestData.ToXML();
        responseXML = registryPartnersDataWebService.GetCurrentTldSnapshotList(inputXml);
        responseData = new RegRegistryPartnersDataResponseData(responseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegRegistryPartnersDataResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegRegistryPartnersDataResponseData(responseXML, oRequestData, ex);
      }
      finally
      {
        if (registryPartnersDataWebService != null)
        {
          registryPartnersDataWebService.Dispose();
        }
      }

      return responseData;
    }

    #endregion
  }
}
