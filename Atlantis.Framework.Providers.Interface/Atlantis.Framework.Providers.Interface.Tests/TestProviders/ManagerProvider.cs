
namespace Atlantis.Framework.Interface.Tests
{
  public class ManagerProvider : ProviderBase, IManagerProvider
  {
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

    private IEmployeeProvider _employeeProvider;
    public IEmployeeProvider EmployeeProvider
    {
      get
      {
        if (_employeeProvider == null)
        {
          _employeeProvider = Container.Resolve<IEmployeeProvider>();
        }

        return _employeeProvider;
      }
    }

    public ManagerProvider(IProviderContainer providerContainer) : base(providerContainer)
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
