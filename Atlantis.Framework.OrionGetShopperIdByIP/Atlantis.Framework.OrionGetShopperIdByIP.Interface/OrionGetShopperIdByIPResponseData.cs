using System;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionGetShopperIdByIP.Interface
{
  public class OrionGetShopperIdByIPResponseData : IResponseData
  {
    AtlantisException _exception;

    public bool IsSuccess { get; private set; }
    public string ShopperId { get; private set; }

    public OrionGetShopperIdByIPResponseData(string shopperId)
    {
      ShopperId = shopperId;
      IsSuccess = true;
    }

    public OrionGetShopperIdByIPResponseData(AtlantisException ex)
    {
      _exception = ex;
    }

    public OrionGetShopperIdByIPResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData, "OrionGetShopperIdByIPResponseData", exception.Message, requestData.ToXML());
    }

    public string ToXML()
    {
      var sw = new StringWriter();
      var serializer = new XmlSerializer(typeof(string));
      serializer.Serialize(sw, ShopperId);
      return sw.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
