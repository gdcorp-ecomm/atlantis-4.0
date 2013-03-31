using Atlantis.Framework.Interface;
using System;
using System.Reflection;

namespace Atlantis.Framework.Providers.Containers
{
  internal static class ProviderContainerHelper
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
      if(!GetProviderConstructor(out providerConstructor, providerType))
      {
        throw new TargetInvocationException(string.Format("Unable to find constructor for {0} with a single IProviderContainer parameter.", providerType.Name), null);
      }
      
      try
      {
        object[] parameters = new object[1] { providerContainer };
        returnObject = (TProviderInterface)providerConstructor.Invoke(parameters);
      }
      catch(ArgumentException argEx)
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
      Type providerContainerInterface = typeof (IProviderContainer);

      ConstructorInfo[] providerConstructors = providerType.GetConstructors();

      foreach (ConstructorInfo constructor in providerConstructors)
      {
        if(constructor.IsPublic)
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
}
