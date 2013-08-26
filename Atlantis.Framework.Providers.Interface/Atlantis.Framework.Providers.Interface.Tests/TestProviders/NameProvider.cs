
using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Interface.Tests;

namespace Atlantis.Framework.Providers.Interface.Tests.TestProviders
{
  public class NameProvider : ProviderBase, INameProvider
  {
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public NameProvider(IProviderContainer providerContainer) : base(providerContainer)
    {
    }

    public void RegisterProvider<TProviderInterface, TProvider>() where TProviderInterface : class where TProvider : ProviderBase
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
      throw new NotImplementedException();
    }

    public void SetData<T>(string key, T value)
    {
      throw new NotImplementedException();
    }


    public bool TryResolve<TProviderInterface>(out TProviderInterface provider) where TProviderInterface : class
    {
      throw new NotImplementedException();
    }
  }
}