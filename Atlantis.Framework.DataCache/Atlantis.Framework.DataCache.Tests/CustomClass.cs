using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.DataCache.Tests
{

  public class CustomClass
  {
    private string _name;

    public CustomClass(string name)
    {
      _name = name;
    }

    public string Name
    {
      get { return _name; }
    }

    public static CustomClass GetCustomClass(string name)
    {
      CustomClass result = new CustomClass(name);
      return result;
    }

  }
}
