﻿using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MailApi.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.MailApi.Impl.GetMessageListRequestHandler" assembly="Atlantis.Framework.MailApi.Impl.dll" request_type="###" />

  public class GetMessageListRequestHandler : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      // Handle the request and return the IResponseData object for the request
      // Returning null will cause an exception

      return result;
    }
  }
}
