using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class ControlMethodInfoManager
  {
    private static readonly Type _controlType = typeof (Control);
    private static readonly IList<string> _controlEventNames = new List<string> { "OnInit", "OnLoad", "OnPreRender" };

    private readonly IDictionary<string, MethodInfo> _cache = new Dictionary<string, MethodInfo>(8);

    internal ControlMethodInfoManager()
    {
      foreach (string eventName in _controlEventNames)
      {
        MethodInfo methodInfo = Load(eventName);
        if (methodInfo != null)
        {
          _cache[eventName] = methodInfo;
        }
      }
    }

    private MethodInfo Load(string eventName)
    {
      return _controlType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.NonPublic);
    }

    private bool TryGet(string eventName, out MethodInfo methodInfo)
    {
      return _cache.TryGetValue(eventName, out methodInfo);
    }

    internal void FireControlEvents(Control control)
    {
      foreach (string eventName in _controlEventNames)
      {
        MethodInfo methodInfo;
        if (TryGet(eventName, out methodInfo))
        {
          methodInfo.Invoke(control, new object[] { EventArgs.Empty });
        }
      }
    }
  }
}
