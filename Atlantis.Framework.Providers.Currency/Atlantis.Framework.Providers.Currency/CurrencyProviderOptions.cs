
namespace Atlantis.Framework.Providers.Currency
{
  public static class CurrencyProviderOptions
  {
    private static bool _useHtmlCurrencySymbols = true;

    public static bool UseHtmlCurrencySymbols
    {
      get { return _useHtmlCurrencySymbols; }
      set { _useHtmlCurrencySymbols = value; }
    }
  }
}
