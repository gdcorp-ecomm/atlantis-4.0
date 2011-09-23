using System;
using Atlantis.Framework.DCCCreateBlogRecord.Impl.DnsApi;
using Atlantis.Framework.DCCCreateBlogRecord.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCCreateBlogRecord.Impl
{
  public class DCCCreateBlogRecordRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DCCCreateBlogRecordResponseData response;

      var request = (DCCCreateBlogRecordRequestData) requestData;

      dnssoapapi service = null;

      try
      {
        service = new dnssoapapi();
        service.Url = ((WsConfigElement) config).WSURL;
        service.Timeout = (int) request.RequestTimeout.TotalMilliseconds;

        var authData = new authDataType {clientid = request.ClientId};
        service.clientAuth = authData;

        var custData = new custDataType
                         {
                           shopperid = request.ShopperID,
                           resellerid = request.PrivateLabelId,
                           origin = request.Origin,
                           enduserip = request.EndUserIp
                         };
        service.custInfo = custData;

        booleanResponseType svcResponse;
        if (string.IsNullOrEmpty(request.SubDomainName))
        {
          svcResponse = service.createBlogRecords(request.DomainName);
        }
        else
        {
          svcResponse = service.createSubdomainBlogRecords(request.DomainName, request.SubDomainName);
        }

        if (svcResponse.result)
        {
          response = new DCCCreateBlogRecordResponseData();
        }
        else
        {
          response = new DCCCreateBlogRecordResponseData(svcResponse.errorcode);
        }
      }
      catch(Exception ex)
      {
        response = new DCCCreateBlogRecordResponseData(request, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return response;
    }
  }
}