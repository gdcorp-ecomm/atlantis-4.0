using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.TargetedShopperService.Interface
{
  public class ShopperXidEncodeResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;

    public string ResultStatus { get; private set; }
    public string ResultData { get; private set; }


    #region Constructors
    public ShopperXidEncodeResponseData() { }

    public ShopperXidEncodeResponseData(string xml)
    {
      ParseResponse(xml);
    }

    public ShopperXidEncodeResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData, "ShopperXidEncodeResponseData", exception.Message, requestData.ToXML());
    }
    #endregion

    private void ParseResponse(string xml)
    {
      _resultXML = xml;

      try
      {
        var xResultXml = XElement.Parse(xml);

        var xResult = xResultXml.Element("Result");
        ResultStatus = xResult != null ? xResult.Value : "Error";

        var xData = xResultXml.Element("Data");
        ResultData = xData != null ? xData.Value : String.Empty;
      }
      catch (Exception ex)
      {
        _exception = new AtlantisException(null, "ShopperXidEncodeResponseData.ParseResponse", ex.Message, _resultXML);
      }

    }

    #region IResponseData Members
    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion

    #region ISessionSerializableResponse Members
    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (string.IsNullOrEmpty(sessionData)) return;

      ParseResponse(sessionData);
    }
    #endregion
  }
}
