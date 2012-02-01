using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atlantis.Framework.HDVD.Interface.Helpers
{

  public static class HDVDObjectConverter<T> where T : new()
  {
    public static IList<T> ConvertAll(IList<object> fromObjects, System.Type toType)
    {
      if (fromObjects != null)
      {
        return fromObjects.Select(obj => (T)Convert(obj, toType)).ToList();
      }
      return null;
    }

    public static object Convert(object fromObject, System.Type toType)
    {
      if (fromObject == null)
        return null;

      object returnObject = Activator.CreateInstance(toType);

      IList<PropertyInfo> toObjectInfos = returnObject.GetType().GetProperties();
      IList<PropertyInfo> tempFromObjectInfos = fromObject.GetType().GetProperties();
      List<PropertyInfo> fromObjectInfos = tempFromObjectInfos.ToList();
      tempFromObjectInfos = null;

      foreach (PropertyInfo property in toObjectInfos)
      {
        PropertyInfo tempPropInfo = null;
        foreach (PropertyInfo fromInfo in fromObjectInfos)
        {
          if (fromInfo.Name.ToLowerInvariant() == property.Name.ToLowerInvariant())
          {

            if (fromInfo.PropertyType.IsArray)
            {
              property.SetValue(returnObject, ConvertAll(((IList<object>)fromInfo.GetValue(fromObject, null)), property.GetType()), null);
            }
            else
            {
              property.SetValue(returnObject, fromInfo.GetValue(fromObject, null), null);
            }
            tempPropInfo = fromInfo;
            break;
          }

        }

        if (tempPropInfo != null)
        {
          fromObjectInfos.Remove(tempPropInfo);
          tempPropInfo = null;
        }

      }

      return returnObject;
    }

  }
}



