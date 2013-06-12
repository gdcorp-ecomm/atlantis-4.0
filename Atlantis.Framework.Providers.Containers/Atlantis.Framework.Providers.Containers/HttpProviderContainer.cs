using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;

namespace Atlantis.Framework.Providers.Containers
{
  public class HttpProviderContainer : IProviderContainer
  {
    private const string KEY_FORMAT = "Atlantis.Framework.Interface.HttpProviderContainer.{0}";

    private readonly object _lockSync = new object();
    private readonly IDictionary<Type, Type> _registeredProvidersDictionary = new Dictionary<Type, Type>(256);
    private readonly ProviderContainerDataBase _providerContainerData = new HttpProviderContainerData();

    private readonly static IProviderContainer _instance = new HttpProviderContainer();
    public static IProviderContainer Instance
    {
      get { return _instance; }
    }

    private HttpProviderContainer()
    {
    }

    private static string GetHttpContextKey(Type type)
    {
      return string.Format(KEY_FORMAT, type.FullName);
    }

    private void AddProviderRegistration(Type providerInterface, Type provider)
    {
      if(!_registeredProvidersDictionary.ContainsKey(providerInterface))
      {
        lock(_lockSync)
        {
          if (!_registeredProvidersDictionary.ContainsKey(providerInterface))
          {
            _registeredProvidersDictionary.Add(providerInterface, provider);
          }
        }
      }
    }

    private Type GetProviderFromRegisteredProviders(Type providerInterface)
    {
      Type providerType;
      
      if (!_registeredProvidersDictionary.TryGetValue(providerInterface, out providerType))
      {
        throw new Exception(string.Format("Type {0} is not registered.", providerInterface.Name));
      }

      return providerType;
    }

    public void RegisterProvider<TProviderInterface, TProvider>() where TProviderInterface : class where TProvider : ProviderBase
    {
      Type providerInterfaceType = typeof (TProviderInterface);
      Type providerType = typeof (TProvider);

      if(ProviderContainerHelper.TypeIsAssignable(providerInterfaceType, providerType))
      {
        AddProviderRegistration(providerInterfaceType, providerType);
      }
    }

    public TProviderInterface Resolve<TProviderInterface>() where TProviderInterface : class
    {
      TProviderInterface providerInterface = default(TProviderInterface);

      Type providerInterfaceType = typeof (TProviderInterface);
      Type providerType = GetProviderFromRegisteredProviders(providerInterfaceType);

      string key = GetHttpContextKey(providerInterfaceType);

      if(HttpContext.Current != null)
      {
        providerInterface = HttpContext.Current.Items[key] as TProviderInterface;
      }

      if (providerInterface == null)
      {
        providerInterface = ProviderContainerHelper.ConstructProvider<TProviderInterface>(providerType, this);
        if (HttpContext.Current != null)
        {
          Debug.WriteLine("HttpProviderContainer: {0}:{1} instantiated | Key: {2} | Url: {3}", providerInterfaceType.Name, providerType.Name, key, HttpContext.Current.Request.Url);
          HttpContext.Current.Items[key] = providerInterface;
        }
      }

      return providerInterface;
    }

    public bool TryResolve<TProviderInterface>(out TProviderInterface providerInterface) where TProviderInterface : class
    {
      bool isRegistered = false;

      Type providerInterfaceType = typeof(TProviderInterface);
      Type providerType;
      if (_registeredProvidersDictionary.TryGetValue(providerInterfaceType, out providerType))
      {
        providerInterface = Resolve<TProviderInterface>();
        isRegistered = true;
      }
      else
      {
        providerInterface = default(TProviderInterface);
      }

      return isRegistered;
    }

    public bool CanResolve<TProviderInterface>() where TProviderInterface : class
    {
      Type providerInterfaceType = typeof(TProviderInterface);
      return _registeredProvidersDictionary.ContainsKey(providerInterfaceType);
    }

    public T GetData<T>(string key, T defaultValue)
    {
      return _providerContainerData.GetData(key, defaultValue);
    }

    public void SetData<T>(string key, T value)
    {
      _providerContainerData.SetData(key, value);
    }
  }
}
