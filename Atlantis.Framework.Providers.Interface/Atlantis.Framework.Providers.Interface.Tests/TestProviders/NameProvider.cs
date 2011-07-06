
using System;

namespace Atlantis.Framework.Interface.Tests
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

    public bool CanResolve<TypeT>()
    {
      return true;
    }
  }
}