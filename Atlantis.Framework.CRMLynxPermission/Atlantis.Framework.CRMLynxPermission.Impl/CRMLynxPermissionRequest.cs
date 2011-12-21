using System;
using Atlantis.Framework.CRMLynxPermission.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CRMLynxPermission.Impl
{
  public class CRMLynxPermissionRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
     CRMLynxPermissionResponseData responseData;

      try
      {
        var request = (CRMLynxPermissionRequestData) requestData;
        bool userHasAccess;

        using (var ws = new LynxPermissionService.PermissionService())
        {
          ws.Url = ((WsConfigElement) config).WSURL;
          ws.Timeout = (int) request.RequestTimeout.TotalMilliseconds;
          userHasAccess = ws.UserHasAccess(request.PermissionKey, request.ManagerUserId);
        }

        responseData = new CRMLynxPermissionResponseData(userHasAccess);       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new CRMLynxPermissionResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CRMLynxPermissionResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
