using System;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.ShopperPriceType.Interface
{
  public class ShopperPriceTypeResponseData : IResponseData, ISessionSerializableResponse
  {
    internal static class ShopperPriceTypes
    {
      public const int Standard = 0;
      public const int BlueRazorMember = 1;
      public const int ResellerDiscountShopper = 2;
      public const int GoDaddyDiscountDomainClub = 8;
      public const int CostcoShopper = 16;
      public const int EmployeeShopper = 32;
    }

    private int _maskedPriceType = 0;
    private int _activePriceType = 0;
    private AtlantisException _ex;

    public ShopperPriceTypeResponseData()
    { }

    public ShopperPriceTypeResponseData(int priceType, int privateLabelId)
    {
      _maskedPriceType = priceType;
      _activePriceType = DeterminePriceType(priceType, privateLabelId);
    }

    public ShopperPriceTypeResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + Environment.NewLine + ex.StackTrace;
      _ex = new AtlantisException(requestData, "ShopperPriceTypeResponseData", message, requestData.ToXML());
    }

    public ShopperPriceTypeResponseData(AtlantisException exAtlantis)
    {
      _ex = exAtlantis;
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

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _ex;
    }

    public string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      sbResult.AppendFormat("<ShopperPriceType masked=\"{0}\" active=\"{1}\" />", _maskedPriceType, _activePriceType);
      return sbResult.ToString();
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (!string.IsNullOrEmpty(sessionData))
      {
        XElement priceTypeElement = XElement.Parse(sessionData);

        {
          int maskedType;
          XAttribute maskedAttribute = priceTypeElement.Attribute("masked");
          if ((maskedAttribute != null) && (int.TryParse(maskedAttribute.Value, out maskedType)))
          {
            _maskedPriceType = maskedType;
          }
        }

        {
          int activeType;
          XAttribute activeAttribute = priceTypeElement.Attribute("active");
          if ((activeAttribute != null) && (int.TryParse(activeAttribute.Value, out activeType)))
          {
            _activePriceType = activeType;
          }
        }
      }
    }

    #endregion
  }
}
