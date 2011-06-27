
namespace Atlantis.Framework.Interface
{
  public interface IProviderContainer
  {
    void RegisterProvider<TProviderInterface, TProvider>() where TProviderInterface : class where TProvider : ProviderBase;
    TProviderInterface Resolve<TProviderInterface>() where TProviderInterface : class;
    bool CanResolve<TypeT>();
  }
}
