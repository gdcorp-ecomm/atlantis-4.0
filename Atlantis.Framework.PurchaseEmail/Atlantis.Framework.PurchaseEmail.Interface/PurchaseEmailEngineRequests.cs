
namespace Atlantis.Framework.PurchaseEmail.Interface
{
  public static class PurchaseEmailEngineRequests
  {
    private static int _getShopper = 1;
    private static int _linkInfo = 12;
    private static int _messagingProcess = 66;
    private static int _productOffer = 24;
    private static int _shopperPriceType = 25;
    private static int _dataProvider = 35;

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
