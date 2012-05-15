using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Atlantis.Framework.GadgetsVote.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GadgetsVote.Impl
{
  public class GadgetsVoteRequest : IRequest
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      const string requestUrlFormat = "{0}?{1}={2}";
      IResponseData responseData;
      var voteRequest = (GadgetsVoteRequestData) requestData;
      StreamReader readStream = null;

      try
      {
        var wsConfig = ((WsConfigElement)config);

        string requestUrl = string.Format(requestUrlFormat, wsConfig.WSURL, "voteForId", voteRequest.VoteCode);

        var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;

        if (webRequest != null)
        {
          webRequest.Timeout = (int)voteRequest.RequestTimeout.TotalMilliseconds;
          webRequest.Method = "GET";
          HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;

          if (webResponse != null)
          {
            Encoding encode = Encoding.GetEncoding("utf-8");
            readStream = new StreamReader(webResponse.GetResponseStream(), encode);
            
            string voteResponse = readStream.ReadToEnd();
            
            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<Dictionary<string, string>>(voteResponse);

            responseData = new GadgetsVoteResponseData(voteResponse, dict);
          }
          else
          {
            responseData = new GadgetsVoteResponseData(string.Empty, new Dictionary<string, string>(0));
          }
        }
        else
        {
          responseData = new GadgetsVoteResponseData(string.Empty, new Dictionary<string, string>(0));
        }
      }
      catch (Exception ex)
      {
        responseData = new GadgetsVoteResponseData(requestData, ex);
      }
      finally
      {
        if (readStream != null)
        {
          readStream.Close();
        }
      }
      return responseData;
    }

    #endregion
  }
}
