namespace Atlantis.Framework.PurchaseEmail.Interface
{
  public static class PurchaseEmailEngineRequests
  {
    private static int _getShopper = 1;
    private static int _linkInfo = 12;
    private static int _productOffer = 24;
    private static int _shopperPriceType = 25;
    private static int _dataProvider = 35;
    private static int _paymentProfiles = 59;
    private static int _messagingProcess = 66;
    private static int _shopperFirstOrderGet = 348;
    private static int _paymentProfileAlternateId = 381;
    private static int _myaMirageData = 386;
    private static int _myaMirageStatus = 448;
    private static int _myaRecentNamespaces = 449;

    public static int PaymentProfiles
    {
      get { return _paymentProfiles; }
      set { _paymentProfiles = value; }
    }

    public static int PaymentProfileAlternateId
    {
      get { return _paymentProfileAlternateId; }
      set { _paymentProfileAlternateId = value; }
    }

    public static int MyaMirageStatus
    {
      get { return _myaMirageStatus; }
      set { _myaMirageStatus = value; }
    }

    public static int MyaRecentNamespaces
    {
      get { return _myaRecentNamespaces; }
      set { _myaRecentNamespaces = value; }
    }

    public static int MyaMirageData
    {
      get { return _myaMirageData; }
      set { _myaMirageData = value; }
    }

    public static int ShopperFirstOrderGet
    {
      get { return _shopperFirstOrderGet; }
      set { _shopperFirstOrderGet = value; }
    }

    public static int GetShopper
    {
      get { return _getShopper; }
      set { _getShopper = value; }
    }

    public static int LinkInfo
    {
      get { return _linkInfo; }
      set { _linkInfo = value; }
    }

    public static int MessagingProcess
    {
      get { return _messagingProcess; }
      set { _messagingProcess = value; }
    }

    public static int ProductOffer
    {
      get { return _productOffer; }
      set { _productOffer = value; }
    }

    public static int ShopperPriceType
    {
      get { return _shopperPriceType; }
      set { _shopperPriceType = value; }
    }

    public static int DataProvider
    {
      get { return _dataProvider; }
      set { _dataProvider = value; }
    }
  }
}
