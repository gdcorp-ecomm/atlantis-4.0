using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.SessionCache.Tests
{
  public class NonFrameworkTestItem : ISessionSerializableResponse
  {
    public string Name1 { get; set; }
    public string Name2 { get; set; }

    public NonFrameworkTestItem()
    {
      Name1 = string.Empty;
      Name2 = string.Empty;
    }

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      string result = Name1 + "|" + Name2;
      return result;
    }

    public void DeserializeSessionData(string sessionData)
    {
      string[] names = sessionData.Split('|');
      Name1 = names[0];
      if (names.Length > 1)
      {
        Name2 = names[1];
      }
    }

    #endregion
  }
}
