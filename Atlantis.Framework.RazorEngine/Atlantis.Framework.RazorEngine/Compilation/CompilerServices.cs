﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.RazorEngine.Compilation
{
  /// <summary>
  /// Provides service methods for compilation.
  /// </summary>
  internal static class CompilerServices
  {
    private static readonly Type _dynamicType = typeof(DynamicObject);
    private static readonly Type _expandoType = typeof(ExpandoObject);

    /// <summary>
    /// Determines if the specified type is an anonymous type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is an anonymous type, otherwise false.</returns>
    public static bool IsAnonymousType(Type type)
    {
      if (type == null)
        throw new ArgumentNullException("type");

      return (type.IsClass
              && type.IsSealed
              && type.BaseType == typeof(object)
              && type.Name.StartsWith("<>")
              && type.IsDefined(typeof(CompilerGeneratedAttribute), true));
    }

    /// <summary>
    /// Generates a random class name.
    /// </summary>
    /// <returns>A new random class name.</returns>
    public static string GenerateClassName()
    {
      Guid guid = Guid.NewGuid();
      return Regex.Replace(guid.ToString("N"), @"[^A-Za-z]*", "");
    }

    /// <summary>
    /// Determines if the specified type is a dynamic type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is an anonymous type, otherwise false.</returns>
    public static bool IsDynamicType(Type type)
    {
      if (type == null)
        throw new ArgumentNullException("type");

      return (_dynamicType.IsAssignableFrom(type)
              || _expandoType.IsAssignableFrom(type)
              || IsAnonymousType(type));
    }

    /// <summary>
    /// Gets the public or protected constructors of the specified type.
    /// </summary>
    /// <param name="type">The target type.</param>
    /// <returns>An enumerable of constructors.</returns>
    public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
    {
      if (type == null)
        throw new ArgumentNullException("type");

      var constructors = type
          .GetConstructors(BindingFlags.Public | BindingFlags.Instance);

      return constructors;
    }

    /// <summary>
    /// Gets an enumerable of all assemblies loaded in the current domain.
    /// </summary>
    /// <returns>An enumerable of loaded assemblies.</returns>
    public static IEnumerable<Assembly> GetLoadedAssemblies()
    {
      var domain = AppDomain.CurrentDomain;
      return domain.GetAssemblies();
    }
  }
}
