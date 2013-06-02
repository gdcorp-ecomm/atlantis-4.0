namespace Atlantis.Framework.Providers.Currency
{
  public static class CurrencyProviderEngineRequests
  {
    private static int _plSignupInfo = 522;
    public static int PLSignupInfo
    {
      get { return _plSignupInfo; }
      set { _plSignupInfo = value; }
    }

    private static int _listPriceRequest = 690;
    public static int ListPriceRequest
    {
      get { return _listPriceRequest; }
      set { _listPriceRequest = value; }
    }

    private static int _promoPriceRequest = 691;
    public static int PromoPriceRequest
    {
      get { return _promoPriceRequest; }
      set { _promoPriceRequest = value; }
    }

    private static int _productIsOnSaleRequest = 692;
    public static int ProductIsOnSaleRequest
    {
      get { return _productIsOnSaleRequest; }
      set { _productIsOnSaleRequest = value; }
    }

    private static int _currencyTypesRequest = 693;
    public static int CurrencyTypesRequest
    {
      get { return _currencyTypesRequest; }
      set { _currencyTypesRequest = value; }
    }

    private static int _validateNonOrderRequest = 644;
    public static int ValidateNonOrderRequest
    {
      get { return _validateNonOrderRequest; }
      set { _validateNonOrderRequest = value; }
    }

    private static int _priceEstimateRequest = 657;
    public static int PriceEstimateRequest
    {
      get { return _priceEstimateRequest; }
      set { _priceEstimateRequest = value; }
    }

    private static int _appSettingRequest = 658;
    public static int AppSettingRequest
    {
      get { return _appSettingRequest; }
      set { _appSettingRequest = value; }
    }

    private static int _multiCurrencyContexts = 707;
    public static int MultiCurrencyContexts
    {
      get { return _multiCurrencyContexts; }
      set { _multiCurrencyContexts = value; }
    }
  }
}
