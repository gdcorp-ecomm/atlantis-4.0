using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeRegistry.Interface
{
  public class RegDotTypeRegistryResponseData : IResponseData
  {
    XElement _responseXml;
    AtlantisException _exception;
    RegistryApi _transferApi;
    RegistryApi _regsitrationApi;

    private RegDotTypeRegistryResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public RegistryApi TransferAPI
    {
      get { return _transferApi; }
    }

    public RegistryApi RegistrationAPI
    {
      get { return _regsitrationApi; }
    }
    
    private RegDotTypeRegistryResponseData(XElement responseXml)
    {
      _responseXml = responseXml;

      XAttribute processingResult = _responseXml.Attribute("processing");
      if (!"success".Equals(processingResult.Value, StringComparison.OrdinalIgnoreCase))
      {
        throw new ArgumentException("Processing result was not 'success'.");
      }

      var apiinfoItems = _responseXml.Descendants("apiinfo");
      foreach (XElement apiInfo in apiinfoItems)
      {
        XAttribute type = apiInfo.Attribute("type");
        XAttribute registryApiId = apiInfo.Attribute("registryapiid");
        XAttribute registryApiDescription = apiInfo.Attribute("registryapidescription");

        if ((type != null) && (registryApiId != null) && (registryApiDescription != null))
        {
          RegistryApi api = new RegistryApi(registryApiId.Value, registryApiDescription.Value);

          switch (type.Value.ToLowerInvariant())
          {
            case "registration":
              _regsitrationApi = api;
              break;
            case "transfer":
              _transferApi = api;
              break;
          }
        }
      }
    }

    public static IResponseData FromXElement(XElement responseXml)
    {
      return new RegDotTypeRegistryResponseData(responseXml);
    }

    public static IResponseData FromException(AtlantisException ex)
    {
      return new RegDotTypeRegistryResponseData(ex);
    }

    public string ToXML()
    {
      return _responseXml.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

  }
}
