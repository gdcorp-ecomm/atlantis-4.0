using Atlantis.Framework.HDVD.Interface.Aries;

namespace Atlantis.Framework.HDVD.Interface.Helpers
{
  public static class ServiceHelper
  {
    public static HCCAPIServiceAries GetServiceReference(string wsUrl)
    {
      return new HCCAPIServiceAries() { Url = wsUrl }; 
    }
  }
}
