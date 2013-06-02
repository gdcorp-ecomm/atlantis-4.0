
namespace Atlantis.Framework.Interface
{
  public interface IProviderContainer
  {
    void RegisterProvider<TProviderInterface, TProvider>() where TProviderInterface : class where TProvider : ProviderBase;
    
    TProviderInterface Resolve<TProviderInterface>() where TProviderInterface : class;
    
    bool TryResolve<TProviderInterface>(out TProviderInterface provider) where TProviderInterface : class;

    bool CanResolve<TProviderInterface>() where TProviderInterface : class;

    T GetData<T>(string key, T defaultValue);

    void SetData<T>(string key, T value);
  }
}
