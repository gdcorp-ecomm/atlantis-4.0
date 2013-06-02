using Atlantis.Framework.EcommPricing.Interface.Base;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class PromoPriceResponseData : PriceResponseData, IResponseData
  {
    private static PromoPriceResponseData _noPriceFoundResponse;

    public static PromoPriceResponseData NoPriceFoundResponse
    {
      get { return _noPriceFoundResponse; }
    }

    #region Constructors
    static PromoPriceResponseData()
    {
      _noPriceFoundResponse = new PromoPriceResponseData();
    }

    private PromoPriceResponseData() : base() { }

    private PromoPriceResponseData(bool isPriceFound, int price, bool isEstimate) : base(isPriceFound, price, isEstimate) { }

    public static PromoPriceResponseData FromPrice(int price, bool isEstimate)
    {
      PromoPriceResponseData result = new PromoPriceResponseData(true, price, isEstimate);
      return result;
    }

    #endregion    
  }
}
