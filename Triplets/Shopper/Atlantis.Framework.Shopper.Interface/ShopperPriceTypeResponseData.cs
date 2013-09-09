using Atlantis.Framework.Interface;
using System.Text;

namespace Atlantis.Framework.Shopper.Interface
{
  public class ShopperPriceTypeResponseData : IResponseData
  {
    public static ShopperPriceTypeResponseData Standard { get; private set; }

    static ShopperPriceTypeResponseData()
    {
      Standard = new ShopperPriceTypeResponseData(ShopperPriceTypes.Standard, 0);
    }

    public static ShopperPriceTypeResponseData FromRawPriceType(int priceType, int privateLabelId)
    {
      return new ShopperPriceTypeResponseData(priceType, privateLabelId);
    }

    private int _maskedPriceType = 0;
    private int _activePriceType = 0;

    private ShopperPriceTypeResponseData(int priceType, int privateLabelId)
    {
      _maskedPriceType = priceType;
      _activePriceType = DeterminePriceType(priceType, privateLabelId);
    }

    public int MaskedPriceType
    {
      get { return _maskedPriceType; }
    }

    public int ActivePriceType
    {
      get { return _activePriceType; }
    }

    private int DeterminePriceType(int rawPriceType, int privateLabelId)
    {
      int result = ShopperPriceTypes.Standard;

      if (privateLabelId == 1)
      {
        if ((rawPriceType & ShopperPriceTypes.EmployeeShopper) > 0)
        {
          result = ShopperPriceTypes.EmployeeShopper;
        }
        else if ((rawPriceType & ShopperPriceTypes.GoDaddyDiscountDomainClub) > 0)
        {
          result = ShopperPriceTypes.GoDaddyDiscountDomainClub;
        }
        else if ((rawPriceType & ShopperPriceTypes.CostcoShopper) > 0)
        {
          result = ShopperPriceTypes.CostcoShopper;
        }
      }
      else if (privateLabelId == 2)
      {
        if ((rawPriceType & ShopperPriceTypes.BlueRazorMember) > 0)
        {
          result = ShopperPriceTypes.BlueRazorMember;
        }
      }
      else if ((privateLabelId > 2) && (privateLabelId != 1387))
      {
        if ((rawPriceType & ShopperPriceTypes.ResellerDiscountShopper) > 0)
        {
          result = ShopperPriceTypes.ResellerDiscountShopper;
        }
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return null;
    }

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      sbResult.AppendFormat("<ShopperPriceType masked=\"{0}\" active=\"{1}\" />", _maskedPriceType, _activePriceType);
      return sbResult.ToString();
    }
  }
}
