using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ProductUpgradePath.Interface
{
  public class FieldReader
  {
    public static VT ReadField<VT>(IDataReader currentReader, string columnName)
    {
      try
      {
        int idx = currentReader.GetOrdinal(columnName);
        if (currentReader.IsDBNull(idx))
          return default(VT);
        else
          return (VT)currentReader[idx];
      }
      catch (System.Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
      return default(VT);
    }

    public static VT ReadField<VT>(IDataReader currentReader, string columnName, VT defaultValue)
    {
      try
      {
        int idx = currentReader.GetOrdinal(columnName);
        if (currentReader.IsDBNull(idx))
          return defaultValue;
        else
          return (VT)currentReader[idx];
      }
      catch (System.Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
      return defaultValue;
    }
  }
}
