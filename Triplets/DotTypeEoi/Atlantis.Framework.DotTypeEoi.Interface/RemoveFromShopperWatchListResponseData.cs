using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class RemoveFromShopperWatchListResponseData : IResponseData
  {
    private readonly XElement _responseXml;
    private readonly AtlantisException _exception;
    private readonly bool _isSuccess;
    private readonly string _responseMessage;

    public RemoveFromShopperWatchListResponseData(AtlantisException exception)
    {
      _exception = exception;
      _isSuccess = false;
    }

    public RemoveFromShopperWatchListResponseData(XElement responseXml)
    {
      try
      {
        _responseXml = responseXml;
        var responseElement = responseXml.Descendants("response").FirstOrDefault();
        if (responseElement != null)
        {
          _responseMessage = responseElement.Value;
          _isSuccess = responseElement.Attribute("result").Value.Equals("success", StringComparison.OrdinalIgnoreCase);
        }
      }
      catch (Exception)
      {
      }
    }

    public static IResponseData FromXElement(XElement responseXml)
    {
      return new RemoveFromShopperWatchListResponseData(responseXml);
    }

    public static IResponseData FromException(AtlantisException ex)
    {
      return new RemoveFromShopperWatchListResponseData(ex);
    }

    public string ToXML()
    {
      return _responseXml.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public string ResponseMessage
    {
      get { return _responseMessage; }
    }
  }
}
