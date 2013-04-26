using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Interface.CDS
{
  public interface ICDSProvider
  {
    T GetModel<T>(string query, IProviderContainer providerContainer) where T : new();

    T GetModel<T>(string query, IProviderContainer providerContainer, Dictionary<string, string> customTokens) where T : new();

    string GetJson(string query, IProviderContainer providerContainer);

    string GetJson(string query, IProviderContainer providerContainer, Dictionary<string, string> customTokens);
  }
}
