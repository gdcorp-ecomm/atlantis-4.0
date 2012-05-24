using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.BizRegImageGet.Interface;
using Atlantis.Framework.BizRegImageGet.Impl.BizRegWS;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BizRegImageGet.Impl
{
  public class BizRegImageGetRequest : IRequest
  {

    #region IRequest memebers
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      BizRegImageGetResponseData oResponseData = null;

      try
      {
        BizRegImageGetRequestData request = (BizRegImageGetRequestData)oRequestData;
        BusinessRegistration service = new BusinessRegistration();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = request.Timeout;
        BizRegWS.urlDTO UrlDTO = service.GetImage(request.BusinessDataID, request.BusinessDataImageType);

        LocalURL localURL = new LocalURL();

        localURL.ImageURL = UrlDTO.imageURL;

        oResponseData = new BizRegImageGetResponseData(localURL);
      }
      catch (Exception ex)
      {
        oResponseData = new BizRegImageGetResponseData(oRequestData, ex);
      }

      return oResponseData;
    }
  }
  #endregion IRequest members
}
