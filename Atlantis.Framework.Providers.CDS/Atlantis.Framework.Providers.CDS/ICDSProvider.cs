using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Interface.CDS
{
  public interface ICDSProvider
  {
    T GetModel<T>(string query) where T : new ();
    T GetModel<T>(string query, Dictionary<string, string> customTokens) where T : new();
    string GetJSON(string query);
    string GetJSON(string query, Dictionary<string, string> customTokens);
    string GetUnparsedJSON(string query);
  }
}
