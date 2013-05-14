using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Currency.Interface
{
  public class CurrencyTypesRequestData : RequestData
  {
    public override string GetCacheMD5()
    {
      return "{all}";
    }
  }
}
