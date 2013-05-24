using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Currency.Interface
{
  public class MultiCurrencyContextsRequestData : RequestData
  {
    public override string GetCacheMD5()
    {
      return "mcpcontexts";
    }
  }
}
