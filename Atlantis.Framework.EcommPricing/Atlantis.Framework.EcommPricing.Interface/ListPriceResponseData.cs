using Atlantis.Framework.EcommPricing.Interface.Base;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class ListPriceResponseData : PriceResponseData, IResponseData
  {
    private static ListPriceResponseData _noPriceFoundResponse;

    public static ListPriceResponseData NoPriceFoundResponse
    {
      get { return _noPriceFoundResponse; }
    }

    #region Constructors
    static ListPriceResponseData()
    {
      _noPriceFoundResponse = new ListPriceResponseData();
    }

    private ListPriceResponseData() : base() { }    

    private ListPriceResponseData(bool isPriceFound, int price, bool isEstimate) : base(isPriceFound, price, isEstimate) { }

    public static ListPriceResponseData FromPrice(int price, bool isEstimate)
    {
      ListPriceResponseData result = new ListPriceResponseData(true, price, isEstimate);
      return result;
    }
    #endregion
  }
}
