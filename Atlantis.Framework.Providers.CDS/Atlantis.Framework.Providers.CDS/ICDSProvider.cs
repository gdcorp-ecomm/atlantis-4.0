using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Interface.CDS
{
  public interface ICDSProvider
  {
    string GetJSON(string query);
    string GetJSON(string query, Dictionary<string, string> customTokens);
  }
}
