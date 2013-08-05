namespace Atlantis.Framework.Providers.MobileContext.Interface
{
  public interface IMobileContextProvider
  {
    MobileApplicationType MobileApplicationType { get; }

    MobileDeviceType MobileDeviceType { get; }

    MobileViewType MobileViewType { get; }
  }
}
