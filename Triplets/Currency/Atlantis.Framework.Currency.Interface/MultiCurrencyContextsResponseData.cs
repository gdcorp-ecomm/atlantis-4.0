using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Currency.Interface
{
  public class MultiCurrencyContextsResponseData : IResponseData
  {
    private HashSet<int> _mcpContexts;

    public static MultiCurrencyContextsResponseData FromDelimitedSetting(string delimitedSetting)
    {
      HashSet<int> contextIds = new HashSet<int>();
      if (!string.IsNullOrEmpty(delimitedSetting))
      {
        string[] contextArray = delimitedSetting.Split(',');
        foreach(string contextString in contextArray)
        {
          int contextId;
          if (int.TryParse(contextString, out contextId))
          {
            contextIds.Add(contextId);
          }
        }
      }

      return new MultiCurrencyContextsResponseData(contextIds);
    }

    private MultiCurrencyContextsResponseData(HashSet<int> contextIds)
    {
      _mcpContexts = contextIds;
    }

    public bool IsContextIdActive(int contextId)
    {
      return _mcpContexts.Contains(contextId);
    }

    public string ToXML()
    {
      XElement element = new XElement("MultiCurrencyContexts");
      foreach (int contextId in _mcpContexts)
      {
        element.Add(new XElement("id", contextId.ToString()));
      }
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
