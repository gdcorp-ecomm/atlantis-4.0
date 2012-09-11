using Atlantis.Framework.EcommInstoreAccept.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.EcommInstoreAccept.Impl
{
  public class EcommInstoreAcceptRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      try
      {
        EcommInstoreAcceptRequestData acceptRequest = (EcommInstoreAcceptRequestData)requestData;


        result = new EcommInstoreAcceptResponseData(-2, "Unknown: Nothing happened");
      }
      catch(Exception ex)
      {
        result = new EcommInstoreAcceptResponseData(requestData, ex);
      }

      return result;
    }
  }
}
