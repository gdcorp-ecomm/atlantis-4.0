using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.XmlHttpFileGet.Interface;

namespace Atlantis.Framework.XmlHttpFileGet.Impl
{
  public class XmlHttpFileGetRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      XmlHttpFileGetResponseData responseData;
      var xmlRequestData = requestData as XmlHttpFileGetRequestData;
      try
      {
        if (xmlRequestData == null)
        {
          throw new Exception("XmlHttpFileGetRequestData requestData is null");
        }

        var xml = FileRequest.SendRequest(xmlRequestData.XmlUrlPath, xmlRequestData.RequestTimeout, xmlRequestData.CacheLevel);
        if (!string.IsNullOrEmpty(xml))
        {
          responseData = new XmlHttpFileGetResponseData(XDocument.Parse(xml), DateTime.Now);
        }
        else
        {
          var aex = new AtlantisException(
            requestData, "XmlHttpFileGet.RequestHandler",
            "Request returned an empty xml.", xmlRequestData.XmlUrlPath);
          responseData = new XmlHttpFileGetResponseData(aex);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        string data = xmlRequestData.XmlUrlPath;
        var aex = new AtlantisException(requestData, "XmlHttpFileGet.RequestHandler", message, data);
        responseData = new XmlHttpFileGetResponseData(aex);
      }

      return responseData;
    }
  }
}