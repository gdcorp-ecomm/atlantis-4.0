using System.Collections.Generic;

namespace Atlantis.Framework.Interface
{
  public class WsConfigElement : ConfigElement
  {
    string _webServiceUrl;

    public WsConfigElement(string progId, string assembly, bool LPC, string webServiceUrl)
      : base(progId, assembly, LPC)
    {
      _webServiceUrl = webServiceUrl;
    }

    public WsConfigElement(string progId, string assembly, bool LPC, string webServiceUrl, Dictionary<string, string> configValues)
      : base(progId, assembly, LPC, configValues)
    {
      _webServiceUrl = webServiceUrl;
    }

    public string WSURL
    {
      get { return _webServiceUrl; }
    }
  }
}
