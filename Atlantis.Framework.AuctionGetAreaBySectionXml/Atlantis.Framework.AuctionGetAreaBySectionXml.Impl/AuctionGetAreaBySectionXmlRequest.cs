using System;
using Atlantis.Framework.AuctionGetAreaBySectionXml.Impl.AuctionGetMemberAreaBySectionXml;
using Atlantis.Framework.AuctionGetAreaBySectionXml.Interface;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.AuctionGetAreaBySectionXml.Impl
{
  public class AuctionGetAreaBySectionXmlRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      trpAdminUtilsService service = null;
      string responseXml = string.Empty;

      try
      {
        var request = requestData as AuctionGetAreaBySectionXmlRequestData;

        service = new trpAdminUtilsService();
        service.Url = ((WsConfigElement)config).WSURL;
        if (request != null)
        {
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          responseXml = service.GetMembersAreaBySectionXML(request.ToXML());
        }

        responseData = new AuctionGetAreaBySectionXmlResponseData(string.Format("<AuctionXmlResponse><results>{0}</results></AuctionXmlResponse>", responseXml));

      }
      catch (Exception ex)
      {
        responseData = new AuctionGetAreaBySectionXmlResponseData(requestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return responseData;
    }

    #endregion
  }
}