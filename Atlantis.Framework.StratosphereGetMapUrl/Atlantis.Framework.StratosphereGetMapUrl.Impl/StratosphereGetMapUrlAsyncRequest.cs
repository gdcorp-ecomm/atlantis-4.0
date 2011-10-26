using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.StratosphereGetMapUrl.Impl.StratosphereRequestbroker;
using Atlantis.Framework.StratosphereGetMapUrl.Interface;

namespace Atlantis.Framework.StratosphereGetMapUrl.Impl
{
  class StratosphereGetMapUrlAsyncRequest: IAsyncRequest
  {
    public IAsyncResult BeginHandleRequest(RequestData requestData, ConfigElement config, AsyncCallback callback, object state)
    {
      StratosphereGetMapUrlRequestData request = (StratosphereGetMapUrlRequestData)requestData;

      StratosphereRequestbroker.Service stratosphereWS = new StratosphereRequestbroker.Service();
      stratosphereWS.Url = ((WsConfigElement)config).WSURL;
      stratosphereWS.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
      request.Certificate.Verify();
      stratosphereWS.ClientCertificates.Add(request.Certificate);

      AsyncState asyncState = new AsyncState(requestData, config, stratosphereWS, state);
      IAsyncResult asyncResult = stratosphereWS.BeginGetMapUrl(request.MapType, request.LookupValue, callback, asyncState);

      return asyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult asyncResult)
    {
      IResponseData responseData = null;
      string urlResponse = string.Empty;
      AsyncState asyncState = (AsyncState)asyncResult.AsyncState;

      try
      {
        StratosphereRequestbroker.Service stratosphereWS = (StratosphereRequestbroker.Service)asyncState.Request;
        WebServiceResponse wsResponse = stratosphereWS.EndGetMapUrl(asyncResult, out urlResponse);

        if (wsResponse.ResultCode == 0)
        {
          responseData = new StratosphereGetMapUrlResponseData(urlResponse);
        }
        else
        {
          string data = string.Format("Error invoking GetMapUrl web method.  Error Code: {0}", wsResponse.ResultCode);
          AtlantisException aex = new AtlantisException(asyncState.RequestData, "StratosphereGetMapUrlAsyncRequest::EndHandleRequest", wsResponse.Error, data);
          responseData = new StratosphereGetMapUrlResponseData(aex);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new StratosphereGetMapUrlResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new StratosphereGetMapUrlResponseData(asyncState.RequestData, ex);
      }

      return responseData;
    }
  }
}
