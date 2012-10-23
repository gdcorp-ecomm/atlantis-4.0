using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface.Tests
{
  public class TestProviderContainer : IProviderContainer
  {
    private readonly IDictionary<Type, Type> _providerContainerDictionary = new Dictionary<Type, Type>(16);
  
    public void RegisterProvider<TProviderInterface, TProvider>() where TProviderInterface : class where TProvider : ProviderBase
    {
      _providerContainerDictionary[typeof (TProviderInterface)] = typeof (TProvider);
    }

    public TProviderInterface Resolve<TProviderInterface>() where TProviderInterface : class
    {
      TProviderInterface providerInterface;

      Type providerType;
      if(_providerContainerDictionary.TryGetValue(typeof (TProviderInterface), out providerType))
      {
        providerInterface = ProviderContainerHelper.ConstructProvider<TProviderInterface>(providerType, this);
      }
      else
      {
        throw new Exception("Provider not registered");
      }

      return providerInterface;
    }

    public bool TryResolve<TProviderInterface>(out TProviderInterface provider) where TProviderInterface : class
    {
      bool isRegistered = false;
      provider = null;

      Type providerType;
      if (_providerContainerDictionary.TryGetValue(typeof(TProviderInterface), out providerType))
      {
        provider = ProviderContainerHelper.ConstructProvider<TProviderInterface>(providerType, this);
        isRegistered = true;
      }

      return isRegistered;
    }

    public bool CanResolve<TProviderInterface>() where TProviderInterface : class
    {
      return _providerContainerDictionary.ContainsKey(typeof(TProviderInterface));
    }
  }
}
