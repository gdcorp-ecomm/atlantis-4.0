using System;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.CRMTaskCreation.Interface;

namespace Atlantis.Framework.CRMTaskCreation.Impl
{
    public class CRMTaskCreationRequest : IRequest
    {
         #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result;

      try
      {

          var taskRequest = (CRMTaskCreationRequestData) oRequestData;

          if (!String.IsNullOrEmpty(taskRequest.ShopperID) &&
              !String.IsNullOrEmpty(taskRequest.OrderID))
          {
       
                  var service = new CrmAppTaskCreation.TaskCreation()
                                {
                                    Url = ((WsConfigElement) oConfig).WSURL,
                                    Timeout =
                                        (int)
                                        taskRequest.RequestTimeout.TotalMilliseconds
                                };
                  string response = service.CreateTask(taskRequest.ClientId, taskRequest.ToXml());
                  result = new CRMTaskCreationResponseData(response);
          }
          else
          {
              throw new ArgumentException("ShopperID or OrderID are incorrect.");
          }
      }

      catch (AtlantisException aex)
      {
          result = new CRMTaskCreationResponseData(aex);
      }
      catch (Exception ex)
      {
          result = new CRMTaskCreationResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
        
    }
}
