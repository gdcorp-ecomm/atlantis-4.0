﻿using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OFFUsageByUsername.Impl.OFFService;
using Atlantis.Framework.OFFUsageByUsername.Interface;

namespace Atlantis.Framework.OFFUsageByUsername.Impl
{
  public class OFFUsageByUsernameRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var offUsageRequest = (OFFUsageByUsernameRequestData) requestData;
      OFFUsageByUsernameResponseData offUsageResponse;

      try
      {
        using (var offWs = new OFFService.OFFSoap())
        {
          offWs.Url = ((WsConfigElement) config).WSURL;
          offWs.Timeout = (int) offUsageRequest.RequestTimeout.TotalMilliseconds;
          string key = config.GetConfigValue("OffKey");

          User offUser = offWs.WorkspaceSoapServicegetUserByUserID(key, offUsageRequest.Username);

          offUsageResponse = new OFFUsageByUsernameResponseData(offUser.quota_bytes, offUser.used_bytes);
        }
      }
      catch (Exception ex)
      {
        string data = string.Format("username={0}|RequestTimeout={1}s", offUsageRequest.Username, offUsageRequest.RequestTimeout.TotalSeconds);
        string message = string.Format("Unable to retrieve OFF usage: {0}", ex.Message);
        var aEx = new AtlantisException(requestData, "OFFUsageByUsernameRequest.RequestHandler", message, data, ex);
        offUsageResponse = new OFFUsageByUsernameResponseData(aEx);
      }

      return offUsageResponse;
    }
  }
}
