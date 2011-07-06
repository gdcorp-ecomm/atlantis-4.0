
namespace Atlantis.Framework.Interface.Tests
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

    public bool CanResolve<TypeT>()
    {
      return true;
    }
  }
}
