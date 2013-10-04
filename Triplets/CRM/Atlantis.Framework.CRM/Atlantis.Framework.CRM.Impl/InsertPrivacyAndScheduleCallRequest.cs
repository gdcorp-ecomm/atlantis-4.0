using Atlantis.Framework.CRM.Impl.crmTaskUtil;
using Atlantis.Framework.CRM.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CRM.Impl
{
  public class InsertPrivacyAndScheduleCallRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      InsertPrivacyDataAndScheduleCallRequestData wsRequestData = (InsertPrivacyDataAndScheduleCallRequestData)requestData;
      TaskUtil service = new TaskUtil();
      service.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      service.Url = ((WsConfigElement)config).WSURL;

      crmTaskUtil.InsertPrivacyDataAndScheduleCallRequest oRequest = new crmTaskUtil.InsertPrivacyDataAndScheduleCallRequest();
      oRequest.privacyXml = wsRequestData.PrivacyDataXML;
      oRequest.taskCreationXml = wsRequestData.ScheduleXML;
      oRequest.clientId = wsRequestData.ClientId;

      crmTaskUtil.InsertPrivacyDataAndScheduleCallResponse oResponse = service.InsertPrivacyDataAndScheduleCall(oRequest);

      return InsertPrivacyDataAndScheduleCallResponseData.FromXml(oResponse.Result);

    }
    
  }
}
