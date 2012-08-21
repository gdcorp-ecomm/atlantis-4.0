using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Xml;
using Atlantis.Framework.FastballEventMetrics.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballEventMetrics.Impl
{
  public class FastballEventMetricsRequest : IAsyncRequest, IRequest
  {
    
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      FastballEventMetricsResponseData responseData = null;

      try
      {
        string sResponseXML = string.Empty;
        XmlDocument responseDoc = new XmlDocument();
        try
        {
          FastballEventMetricsRequestData FastballEventMetricsRequestData = (FastballEventMetricsRequestData)requestData;
          sResponseXML = string.Empty;

          WsConfigElement configElement = (WsConfigElement)config;
          WSHttpBinding binding = new WSHttpBinding(SecurityMode.Transport);
          binding.ReceiveTimeout = FastballEventMetricsRequestData.RequestTimeout;
          binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
          EndpointAddress remoteAddress = new EndpointAddress(configElement.WSURL);

          using (gdFbMetricService.Service1Client activationSvc = new gdFbMetricService.Service1Client(binding, remoteAddress))
          {
            //Required.  This is pre-configured on the FBPMS system and describes the Metric Data Source name from which this data belongs.
            FastballEventMetricsRequestData.EventMessage.metricDataSave.dataSource = config.GetConfigValue("datasource");
            

            activationSvc.ClientCredentials.ClientCertificate.Certificate = configElement.GetClientCertificate("CertificateName");
            activationSvc.Open();

             sResponseXML = activationSvc.MessageReceiver(FastballEventMetricsRequestData.Message);

             
            
            responseDoc.LoadXml(sResponseXML);
          }
          if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(requestData,
                                                                 "FastballEventMetricsRequest.RequestHandler",
                                                                 sResponseXML,
                                                                 requestData.ToXML());

            responseData = new FastballEventMetricsResponseData(sResponseXML, exAtlantis);
          }
          else
          {
            responseData = new FastballEventMetricsResponseData(responseDoc);
          }

        }
        catch (AtlantisException exAtlantis)
        {
          responseData = new FastballEventMetricsResponseData(sResponseXML, exAtlantis);
        }
        catch (Exception ex)
        {
          responseData = new FastballEventMetricsResponseData(requestData, ex);
        }

        return responseData;
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new FastballEventMetricsResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new FastballEventMetricsResponseData(requestData, ex);
      }
      return responseData;
    }

    
    public IAsyncResult BeginHandleRequest(RequestData requestData, ConfigElement config, AsyncCallback callback, object state)
    {
      ParameterizedThreadStart tStart = new ParameterizedThreadStart(RequestThread);
      Thread t = new Thread(tStart);
      stateObjectRequest oAsyncState = new stateObjectRequest { requestData = requestData,  config = config, callback= callback, stateObject = state };
      t.Start(oAsyncState);
      return oAsyncState.AsyncResult;
    }
   
    public IResponseData EndHandleRequest(IAsyncResult asyncResult)
    {
      throw new NotImplementedException();
    }

    #region private asynchronous implementation
    private void RequestThread(object o)
    {
      stateObjectRequest request = (stateObjectRequest)o;
      FastballEventMetricsResponseData response = (FastballEventMetricsResponseData)RequestHandler(request.requestData, request.config);
      response.setStateObject(request.stateObject);
      request.AsyncResult.AsyncState = response;
      request.callback(request.AsyncResult);
    }

    internal class fb_asyncResult : IAsyncResult
    {
      public object AsyncState{get;set;}
      public WaitHandle AsyncWaitHandle{get { throw new NotImplementedException(); }}
      public bool CompletedSynchronously{get { throw new NotImplementedException(); }}
      public bool IsCompleted{get { throw new NotImplementedException(); }}
    }

    internal class stateObjectRequest
    {
      private fb_asyncResult asyncResult;
      public stateObjectRequest()
      {
        asyncResult = new fb_asyncResult();
      }
      public fb_asyncResult AsyncResult { get { return asyncResult; } }
      public RequestData requestData { get; set; }
      public ConfigElement config { get; set; }
      public AsyncCallback callback { get; set; }
      public object stateObject { get; set; }
    }
    #endregion

  }
}
