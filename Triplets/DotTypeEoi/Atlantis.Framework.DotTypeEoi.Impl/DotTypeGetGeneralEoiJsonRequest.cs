using System;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeEoi.Impl.RegEoiWebSvc;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Impl
{
  public class DotTypeGetGeneralEoiJsonRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeGetGeneralEoiJsonResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        using (var regEoiWebSvc = new RegEOIWebSvc())
        {
          regEoiWebSvc.Url = ((WsConfigElement)config).WSURL;
          regEoiWebSvc.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          responseXml = regEoiWebSvc.GetGeneralEOIJSON();

          var getGeneralEoiJsonElement = XElement.Parse(responseXml);
          var responseElement = getGeneralEoiJsonElement.Element("response");
          var resultAttribute = responseElement.Attribute("result").Value;
          if (string.IsNullOrEmpty(resultAttribute) || !resultAttribute.Equals("success"))
          {
            AtlantisException exAtlantis = new AtlantisException(requestData,
                                                                  "DotTypeGetGeneralEoiJsonRequest",
                                                                  responseXml,
                                                                  requestData.ToXML());

            responseData = new DotTypeGetGeneralEoiJsonResponseData(responseXml, exAtlantis);
          }
          else
          {
            responseData = new DotTypeGetGeneralEoiJsonResponseData(responseElement.Value);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeGetGeneralEoiJsonResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
