using System.Collections.Generic;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  internal static class HashSetExtensions
  {
    public static List<T> IntersectToList<T>(this HashSet<T> validItems, IEnumerable<T> itemsToValidate)
    {
      List<T> result = new List<T>();

      if (itemsToValidate != null)
      {
        foreach (T item in itemsToValidate)
        {
          if (validItems.Contains(item))
          {
            result.Add(item);
          }
        }
      }

      return result;
    }
  }
}
