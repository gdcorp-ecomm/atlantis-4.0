using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class IProviderContainerExtensions
  {
    internal static bool TryResolve<TProviderInterface>(this IProviderContainer providerContainer, Type providerType, out TProviderInterface providerImplementation) where TProviderInterface : class
    {
      providerImplementation = null;

      object[] parameters = new object[] { null };
      bool isRegistered = (bool)providerContainer.GetType().GetMethod("TryResolve").MakeGenericMethod(providerType).Invoke(providerContainer, parameters);
      
      if(isRegistered)
      {
        providerImplementation = (TProviderInterface)parameters[0];
      }

      return isRegistered;
    }
  }
}
