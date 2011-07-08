using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Currency
{
  public class MultiCurrencyContexts
  {
    private HashSet<int> _mcpContextIds = new HashSet<int>();

    private MultiCurrencyContexts()
    {
      string delimitedContexts = DataCache.DataCache.GetAppSetting("ATLANTIS_MCP_ACTIVE_CONTEXTS");
      if (!string.IsNullOrEmpty(delimitedContexts))
      {
        string[] contextArray = delimitedContexts.Split(',');
        foreach (string contextString in contextArray)
        {
          int contextId;
          if (int.TryParse(contextString, out contextId))
          {
            _mcpContextIds.Add(contextId);
          }
        }
      }
    }

    private bool IsContextIdActive(int contextId)
    {
      return _mcpContextIds.Contains(contextId);
    }

    private static MultiCurrencyContexts GetMCPContexts(string key)
    {
      return new MultiCurrencyContexts();
    }

    public static bool GetIsContextIdActive(int contextId)
    {
      MultiCurrencyContexts mcpContexts = DataCache.DataCache.GetCustomCacheData<MultiCurrencyContexts>("MCPContexts", GetMCPContexts);
      return mcpContexts.IsContextIdActive(contextId);
    }
  }
}
