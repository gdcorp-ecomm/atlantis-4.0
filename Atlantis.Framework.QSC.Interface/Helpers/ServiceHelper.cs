using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSC.Interface.Helpers
{
  public static class ServiceHelper
  {
    public static Mobilev10 GetServiceReference(string wsUrl)
    {
      return new Mobilev10() { Url = wsUrl };
    }
  }
}
