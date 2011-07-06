using System.Collections.Generic;

namespace Atlantis.Framework.Interface.Tests
{
  public class EmployeeProvider : ProviderBase, IEmployeeProvider
  {
    public IList<string> Employees { get; private set; }

    public EmployeeProvider(IProviderContainer providerContainer) : base(providerContainer)
    {
      Employees = new List<string>();
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
