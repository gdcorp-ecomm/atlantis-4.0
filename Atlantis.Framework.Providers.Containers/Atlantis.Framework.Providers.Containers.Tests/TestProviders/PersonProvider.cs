using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Containers.Tests
{
  public class PersonProvider : ProviderBase, IPersonProvider
  {
    public string Location { get; private set; }

    private INameProvider _nameProvider;
    public INameProvider NameProvider
    {
      get
      {
        if(_nameProvider == null)
        {
          _nameProvider = Container.Resolve<INameProvider>();
        }

        return _nameProvider;
      }
    }

    public PersonProvider(IProviderContainer providerContainer) : base(providerContainer)
    {
    }

    public void RegisterProvider<TProviderInterface, TProvider>()
      where TProviderInterface : class
      where TProvider : ProviderBase
    {
    }

    public TProviderInterface Resolve<TProviderInterface>() where TProviderInterface : class
    {
      return this as TProviderInterface;
    }

    public bool CanResolve<TProviderInterface>() where TProviderInterface : class
    {
      return true;
    }

    public T GetData<T>(string key, T defaultValue)
    {
      throw new System.NotImplementedException();
    }

    public void SetData<T>(string key, T value)
    {
      throw new System.NotImplementedException();
    }


    public bool TryResolve<TProviderInterface>(out TProviderInterface provider) where TProviderInterface : class
    {
      throw new System.NotImplementedException();
    }
  }
}
