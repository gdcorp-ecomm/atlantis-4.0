using System;
using System.ComponentModel;

namespace Atlantis.Framework.RegDotTypeProductIds.Interface
{
  public static class EnumHelper
  {
    public static T GetValueFromDescription<T>(string description)
    {
      var type = typeof(T);
      if (!type.IsEnum) throw new InvalidOperationException();
      foreach (var field in type.GetFields())
      {
        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        if (attribute != null)
        {
          if (attribute.Description.ToLowerInvariant() == description.ToLowerInvariant())
            return (T)field.GetValue(null);
        }
        else
        {
          if (field.Name.ToLowerInvariant() == description.ToLowerInvariant())
            return (T)field.GetValue(null);
        }
      }
      return default(T);
    }
  }
}
