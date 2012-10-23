using System;
using System.Reflection;
using System.Web;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal static class AssemblyHelper
  {
    internal static Assembly GetApplicationAssembly()
    {
      Assembly applicationAssembly = null;

      if(HttpContext.Current != null)
      {
        Type assemblyType = HttpContext.Current.ApplicationInstance.GetType();
        while (assemblyType != null && assemblyType.Namespace == "ASP")
        {
          assemblyType = assemblyType.BaseType;
        }

        if (assemblyType != null)
        {
          applicationAssembly = assemblyType.Assembly;
        }
      }
      
      if(applicationAssembly == null)
      {
        applicationAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
      }

      return applicationAssembly;
    }
  }
}
