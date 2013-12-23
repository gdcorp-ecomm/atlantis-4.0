namespace Atlantis.Framework.Warmup.Runner.TypeHelpers
{
  using System;
  using System.Collections.Generic;
  using System.Reflection;

  public class ClassFinder
  {
    /// <summary>
    /// Returns all classes in assembly list that has the specified attribute.  If
    /// the attribute is null, all classes will be added from the assembly list.
    /// If excludeGAC is true, then classes in the GAC will be excluded.
    /// </summary>
    /// <param name="assemblyList"></param>
    /// <param name="excludeGAC"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static List<Type> GetClasses(IEnumerable<Assembly> assemblyList, bool excludeGAC, Type attr)
    {
      return GetClasses(assemblyList, excludeGAC, new List<Type> { attr });
    }

    /// <summary>
    /// Returns all classes in assembly list that has the specified attribute.  If
    /// the attribute is null, all classes will be added from the assembly list.
    /// By default, this class excludes classes in the GAC.
    /// </summary>
    /// <param name="assemblyList"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static List<Type> GetClasses(IEnumerable<Assembly> assemblyList, Type attr)
    {
      return GetClasses(assemblyList, true, attr);
    }

    /// <summary>
    /// Returns all classes in assembly list that has at last one attribute.  If the attribute list
    /// is null, then all classes will be added.  If the attribute list is empty, then no classes
    /// will be added (the assembly and class lists will still be traversed).  If excludeGAC is
    /// true, then classes in the GAC are excluded.
    /// </summary>
    /// <param name="assemblyList"></param>
    /// <param name="excludeGAC"></param>
    /// <param name="attrs"></param>
    /// <returns></returns>
    public static List<Type> GetClasses(IEnumerable<Assembly> assemblyList, bool excludeGAC, IEnumerable<Type> attrs)
    {
      var classesInSet = new List<Type>();
      foreach (var asm in assemblyList)
      {
        if (excludeGAC && asm.GlobalAssemblyCache)
        {
          continue;
        }
        try
        {  // the try/catch maybe unnecessary w/GetExportedTypes, but since GetTypes sometimes failed...
          var exportedTypes = asm.GetExportedTypes();
          foreach (var classType in exportedTypes)
          {
            bool add = (attrs == null);
            if (!add)
            {
              foreach (var attr in attrs)
              {
                if (Attribute.GetCustomAttribute(classType, attr) != null)
                {
                  add = true;
                  break;
                }
              }
            }
            if (add)
            {
              classesInSet.Add(classType);
            }
          }
        }
        catch (Exception)
        {
        }
      }
      return classesInSet;
    }

    /// <summary>
    /// Returns all classes in assembly list that has at last one attribute.  If the attribute list
    /// is null, then all classes will be added.  If the attribute list is empty, then no classes
    /// will be added (the assembly and class lists will still be traversed).
    /// </summary>
    /// <param name="assemblyList"></param>
    /// <param name="attrs"></param>
    /// <returns></returns>
    public static List<Type> GetClasses(IEnumerable<Assembly> assemblyList, IEnumerable<Type> attrs)
    {
      return GetClasses(assemblyList, true, attrs);
    }

    public static List<Type> GetClasses(IEnumerable<string> assemblyList, bool excludeGAC, Type attr)
    {
      var asmList = new List<Assembly>();
      foreach (string asm in assemblyList)
      {
        asmList.Add(Assembly.Load(asm));
      }
      return GetClasses(asmList, excludeGAC, attr);
    }

    public static List<Type> GetClasses(IEnumerable<string> assemblyList, Type attr)
    {
      return GetClasses(assemblyList, true, attr);
    }

    public static List<Type> GetClasses(IEnumerable<string> assemblyList, bool excludeGAC)
    {
      return GetClasses(assemblyList, excludeGAC, null);
    }

    public static List<Type> GetClasses(IEnumerable<string> assemblyList)
    {
      return GetClasses(assemblyList, true, null);
    }

    /// <summary>
    /// Returns all the assembly's classes which have the attribute.  If the attribute is null, all classes will be
    /// added.  If the assembly is in the GAC, all classes are still returned.
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <param name="attr"></param>
    /// <returns></returns>
    public static List<Type> GetClasses(string assemblyName, Type attr)
    {
      return GetClasses(new List<string> { assemblyName }, false, attr);
    }

    /// <summary>
    /// Returns all the assembly's classes.  If the assembly is in the GAC, all classes are still returned.
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static List<Type> GetClasses(string assemblyName)
    {
      return GetClasses(assemblyName, null);
    }

  }
}