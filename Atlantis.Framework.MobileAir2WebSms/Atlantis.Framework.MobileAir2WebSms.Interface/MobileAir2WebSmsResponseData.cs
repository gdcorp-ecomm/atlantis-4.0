using System;
using System.Reflection;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MobileAir2WebSms.Interface
{
  public class MobileAir2WebSmsResponseData : IResponseData
  {
    private readonly AtlantisException _atlException;

    public string XmlResponse { get; private set; }

    public string ResponseCode { get; private set; }

    public string ResponseMessage { get; private set; }

    public bool IsSuccess { get; private set; }

    public MobileAir2WebSmsResponseData(string responseString, RequestData requestData)
    {
      try
      {
        XmlResponse = responseString;

        XmlDocument responseDocument = new XmlDocument();
        responseDocument.LoadXml(XmlResponse);

        XmlNodeList codeNodeList = responseDocument.SelectNodes("//code");

        if (codeNodeList != null && codeNodeList.Count > 0)
        {
          ResponseCode = codeNodeList[0].InnerText;
          XmlNodeList descriptionNodeList = responseDocument.SelectNodes("//description");
          if (descriptionNodeList != null && descriptionNodeList.Count > 0)
          {
            ResponseMessage = descriptionNodeList[0].InnerText;
          }
        }
        else
        {
          ResponseCode = string.Empty;
          ResponseMessage = string.Empty;
          IsSuccess = false;
          _atlException = new AtlantisException(requestData, MethodBase.GetCurrentMethod().Name, "No response code returned.", string.Empty);
        }

        if (ResponseCode == "100")
        {
          IsSuccess = true;
        }
        else
        {
          IsSuccess = false;
          _atlException = new AtlantisException(requestData, MethodBase.GetCurrentMethod().Name, ResponseMessage, string.Empty);

        }
      }
      catch (Exception ex)
      {
        IsSuccess = false;
        _atlException = new AtlantisException(requestData, MethodBase.GetCurrentMethod().Name, ex.Message + " | " + ex.StackTrace, string.Empty);
      }
    }

    public MobileAir2WebSmsResponseData(Exception ex, RequestData requestData)
    {
      IsSuccess = false;
      XmlResponse = string.Empty;
      _atlException = new AtlantisException(requestData, MethodBase.GetCurrentMethod().Name, ex.Message + " | " + ex.StackTrace, string.Empty);
    }

    public string ToXML()
    {
      return XmlResponse;
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }
  }
}
