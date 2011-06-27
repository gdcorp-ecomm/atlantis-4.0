using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine.Tests.MockTriplet
{
  public class ConfigTestRequest1 : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData response = null;
      string value1 = oConfig.GetConfigValue("value1");
      string value2 = oConfig.GetConfigValue("value2");

      if (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
      {
        response = new ConfigTestResponseData();
      }
      else
      {
        AtlantisException ex = new AtlantisException(oRequestData, "ConfigTestRequest1.RequestHandler", "Expected config values are missing.", string.Empty);
        response = new ConfigTestResponseData(ex);
      }

      return response;
    }

    #endregion
  }
}
