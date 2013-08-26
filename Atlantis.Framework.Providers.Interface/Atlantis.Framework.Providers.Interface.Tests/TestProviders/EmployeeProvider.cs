using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Interface.Tests;

namespace Atlantis.Framework.Providers.Interface.Tests.TestProviders
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
