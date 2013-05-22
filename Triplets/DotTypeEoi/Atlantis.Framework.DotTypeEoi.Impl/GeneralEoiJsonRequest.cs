using System;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeEoi.Impl.RegEoiWebSvc;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Impl
{
  public class GeneralEoiJsonRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GeneralEoiJsonResponseData responseData;
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

            responseData = new GeneralEoiJsonResponseData(responseXml, exAtlantis);
          }
          else
          {
            responseData = new GeneralEoiJsonResponseData(responseElement.Value);
          }
        }
      }
      catch (Exception ex)
      {
        responseData = new GeneralEoiJsonResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}
