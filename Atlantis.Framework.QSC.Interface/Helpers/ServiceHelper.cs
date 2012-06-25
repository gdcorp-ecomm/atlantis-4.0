using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSC.Interface.Helpers
{
  public static class ServiceHelper
  {
    public static Mobile GetServiceReference(string wsUrl)
    {
      return new Mobile() { Url = wsUrl };
    }
  }
}
