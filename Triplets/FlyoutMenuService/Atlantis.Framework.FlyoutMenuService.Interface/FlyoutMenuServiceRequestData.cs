using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FlyoutMenuService.Interface
{
  public class FlyoutMenuServiceRequestData : RequestData
  {
    #region Properties
    public enum ServiceType
    {
      MenuItem = 0,
      MenuSite = 1
    }

    public ServiceType MenuServiceType { get; private set; }
    
    #endregion

    public FlyoutMenuServiceRequestData(ServiceType type)
    {
      MenuServiceType = type;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(MenuServiceType.ToString());
    }
  }
}
