using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockProviderContainer : IProviderContainer
  {
    private const string KEY_FORMAT = "Atlantis.Framework.Interface.MockProviderContainer.{0}";

    private readonly object _lockSync = new object();
    private readonly IDictionary<Type, Type> _registeredProvidersDictionary = new Dictionary<Type, Type>(64);
    private readonly IDictionary<string, object> _providerInterfaces = new Dictionary<string, object>();
    private readonly IDictionary<string, object> _mockProviderSettings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    private readonly MockProviderContainerData _containerData = new MockProviderContainerData();

    private static string GetObjectKey(Type type)
    {
      return string.Format(KEY_FORMAT, type.FullName);
    }

    private void AddProviderRegistration(Type providerInterface, Type provider)
    {
      if (!_registeredProvidersDictionary.ContainsKey(providerInterface))
      {
        lock (_lockSync)
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

    public void RegisterProvider<TProviderInterface, TProvider>()
      where TProviderInterface : class
      where TProvider : ProviderBase
    {
      Type providerInterfaceType = typeof(TProviderInterface);
      Type providerType = typeof(TProvider);

      if (ProviderContainerHelper.TypeIsAssignable(providerInterfaceType, providerType))
      {
        AddProviderRegistration(providerInterfaceType, providerType);
      }
    }

    public TProviderInterface Resolve<TProviderInterface>() where TProviderInterface : class
    {
      TProviderInterface providerInterface = default(TProviderInterface);

      Type providerInterfaceType = typeof(TProviderInterface);
      Type providerType = GetProviderFromRegisteredProviders(providerInterfaceType);

      string key = GetObjectKey(providerInterfaceType);

      if (_providerInterfaces.ContainsKey(key))
      {
        providerInterface = _providerInterfaces[key] as TProviderInterface;
      }

      if (providerInterface == null)
      {
        providerInterface = ProviderContainerHelper.ConstructProvider<TProviderInterface>(providerType, this);
        #if DEBUG
        Debug.WriteLine("MockProviderContainer: {0}:{1} instantiated | Key: {2}", providerInterfaceType.Name, providerType.Name, key);
        #endif
        _providerInterfaces[key] = providerInterface;
      }

      return providerInterface;
    }

    public bool TryResolve<TProviderInterface>(out TProviderInterface provider) where TProviderInterface : class
    {
      bool isRegistered = false;

      Type providerInterfaceType = typeof(TProviderInterface);
      Type providerType;
      if (_registeredProvidersDictionary.TryGetValue(providerInterfaceType, out providerType))
      {
        provider = Resolve<TProviderInterface>();
        isRegistered = true;
      }
      else
      {
        provider = default(TProviderInterface);
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
      return _containerData.GetData(key, defaultValue);
    }

    public void SetData<T>(string key, T value)
    {
      _containerData.SetData(key, value);
    }

    private static class ProviderContainerHelper
    {
      public static bool TypeIsAssignable(Type assignmentTargetType, Type assignmentValueType)
      {
        if (assignmentTargetType == null) throw new ArgumentNullException("assignmentTargetType");
        if (assignmentValueType == null) throw new ArgumentNullException("assignmentValueType");

        if (!assignmentTargetType.IsAssignableFrom(assignmentValueType))
        {
          throw new ArgumentException(string.Format("The type {0} cannot be assigned to type {1}.", assignmentValueType.Name, assignmentTargetType.Name));
        }

        return true;
      }

      public static TProviderInterface ConstructProvider<TProviderInterface>(Type providerType, IProviderContainer providerContainer)
      {
        TProviderInterface returnObject;

        ConstructorInfo providerConstructor;
        if (!GetProviderConstructor(out providerConstructor, providerType))
        {
          throw new TargetInvocationException(string.Format("Unable to find constructor for {0} with a single IProviderContainer parameter.", providerType.Name), null);
        }

        try
        {
          object[] parameters = new object[] { providerContainer };
          returnObject = (TProviderInterface)providerConstructor.Invoke(parameters);
        }
        catch (ArgumentException argEx)
        {
          throw new ArgumentException(string.Format("The parameter array does not contain values that match the types accepted by the {0} constructor. {1}", providerType.Name, argEx.Message));
        }
        catch (Exception ex)
        {
          throw new TargetInvocationException(string.Format("An exception has occured invoking the {0} constructor. {1}", providerType.Name, ex.Message), ex);
        }

        return returnObject;
      }

      private static bool GetProviderConstructor(out ConstructorInfo constructorInfo, Type providerType)
      {
        constructorInfo = null;
        Type providerContainerInterface = typeof(IProviderContainer);

        ConstructorInfo[] providerConstructors = providerType.GetConstructors();

        foreach (ConstructorInfo constructor in providerConstructors)
        {
          if (constructor.IsPublic)
          {
            ParameterInfo[] parameterInfo = constructor.GetParameters();
            if (parameterInfo.Length == 1)
            {
              bool isSignatureMatch = false;

              if (providerContainerInterface == parameterInfo[0].ParameterType &&
                  providerContainerInterface.IsAssignableFrom(typeof(IProviderContainer)))
              {
                isSignatureMatch = true;
              }

              if (isSignatureMatch)
              {
                constructorInfo = constructor;
                break;
              }
            }
          }
        }

        return constructorInfo != null;
      }
    }

    [Obsolete("Use IProviderContainer.GetData directly.")]
    public object GetMockSetting(string name)
    {
      return GetData(name, (string) null);
    }

    [Obsolete("Use IProviderContainer.SetData directly.")]
    public void SetMockSetting(string name, object value)
    {
      SetData(name, value);
    }

  }
}
