using Atlantis.Framework.Interface;
using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class PriceEstimateResponseData : IResponseData
  {
    //    Request Format
    //    <PriceEstimateRequest privateLabelID="1" membershipLevel="0" transactionCurrency="USD" source_code="EXAMPLECODE">
    //      <Item pf_id="54" priceGroupID="1"/>
    //    </PriceEstimateRequest>

    //    Response Format
    //    <PriceEstimate membershipLevel="0" privateLabelID="1" transactionCurrency="USD" source_code="EXAMPLECODE">
    //      <Item pf_id="54" name="Deluxe Hosting with ASP" list_price="999" _oadjust_adjustedprice="999" _icann_fee_adjusted="0"/>
    //    </PriceEstimate>

    private static PriceEstimateResponseData _noPriceFoundResponse;

    static PriceEstimateResponseData()
    {
      _noPriceFoundResponse = new PriceEstimateResponseData();
    }

    public static PriceEstimateResponseData NoPriceFoundResponse
    {
      get { return _noPriceFoundResponse; }
    }

    public static PriceEstimateResponseData FromXml(string cacheDataXml)
    {
      PriceEstimateResponseData responseObject = NoPriceFoundResponse;

      try
      {
        if (!string.IsNullOrEmpty(cacheDataXml))
        {
          XElement data = XElement.Parse(cacheDataXml);
          XElement item = data.Descendants("Item").FirstOrDefault();
          if (item != null)
          {
            XAttribute transactionCurrencyAtt = data.Attribute("transactionCurrency");
            XAttribute adjustedpriceAtt = item.Attribute("_oadjust_adjustedprice");
            XAttribute icannFeeAtt = item.Attribute("_icann_fee_adjusted");

            int price = int.Parse(adjustedpriceAtt.Value);
            int icannFee = int.Parse(icannFeeAtt.Value);

            // The adjustedPrice returned by the PriceEstimate WS includes the icannFee, for simplicity we are removing the icannFee and displaying it seperately.
            price = price - icannFee;

            responseObject = new PriceEstimateResponseData(transactionCurrencyAtt.Value, price);
          }
          else
          {
            item = data.Descendants("Error").FirstOrDefault();
            if (item != null)
            {
              item = data.Descendants("Description").FirstOrDefault();
              if (item != null && !item.IsEmpty)
              {
                responseObject = new PriceEstimateResponseData(item.Value);
              }
              else
              {
                responseObject = new PriceEstimateResponseData();
              }
            }
          }
        }
        else
        {
          var aex = new AtlantisException("Atlantis.Framework.EcommPricing.PriceEstimateResponseData.FromXml", "0", "WS Response was empty string.", string.Empty, null, null);
          Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        var aex = new AtlantisException("Atlantis.Framework.EcommPricing.PriceEstimateResponseData.FromXml", "0", "WS Response contains invalid XML or invalid price values.", message, null, null);
        Engine.EngineLogging.EngineLogger.LogAtlantisException(aex);
      }

      return responseObject;
    }

    public static PriceEstimateResponseData FromException(AtlantisException exception)
    {
      return new PriceEstimateResponseData(exception);
    }

    private AtlantisException _exception;

    private PriceEstimateResponseData()
    {
      IsPriceFound = false;
      CurrencyType = string.Empty;
      AdjustedPrice = 0;
    }

    private PriceEstimateResponseData(AtlantisException exception)
      : this()
    {
      _exception = exception;
    }

    private PriceEstimateResponseData(string errorDescription)
    {
      IsPriceFound = false;
      CurrencyType = string.Empty;
      AdjustedPrice = 0;
      ErrorDescription = errorDescription;
    }

    private PriceEstimateResponseData(string currencyType, int adjustedPrice)
    {
      IsPriceFound = true;
      CurrencyType = currencyType;
      AdjustedPrice = adjustedPrice;
    }

    public bool IsPriceFound { get; private set; }
    public string CurrencyType { get; private set; }
    public int AdjustedPrice { get; private set; }
    public string ErrorDescription { get; private set; }

    public string ToXML()
    {
      var element = new XElement("PriceEstimateResponseData");
      element.Add(
        new XAttribute("pricefound", IsPriceFound.ToString()),
        new XAttribute("currencytype", CurrencyType),
        new XAttribute("adjustedprice", AdjustedPrice.ToString(CultureInfo.InvariantCulture)),
        new XAttribute("errordescription", ErrorDescription));

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
