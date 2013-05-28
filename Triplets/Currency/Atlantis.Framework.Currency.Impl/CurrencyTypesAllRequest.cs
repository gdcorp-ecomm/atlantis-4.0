using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Currency.Impl
{
  public class CurrencyTypesAllRequest : IRequest
  {
    private const string CURRENCY_TYPES_STORED_PROCEDURE = "gdshop_currencyTypeGetAll_sp";
    private const string CURRENCY_TYPES_RATES_STORED_PROCEDURE = "gdshop_currencyTypeConversionRateGetAll_sp";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var dictCurrencyElements = new Dictionary<string, XElement>();
      var xCurrencies = new XElement("currencies");
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        cn.Open();

        //Get currency types
        using (var cmd = new SqlCommand(CURRENCY_TYPES_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              string currencyType;
              var xCurrency = GetCurrencyElement(reader, out currencyType);
              dictCurrencyElements.Add(currencyType, xCurrency);
            }
          }
        }

        //Get currency type conversion rates
        using (var cmd = new SqlCommand(CURRENCY_TYPES_RATES_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              var xCurrency = AddCurrencyExchangeAttributes(reader, dictCurrencyElements);
              xCurrencies.Add(xCurrency);
            }
          }
        }

        cn.Close();
      }
      return CurrencyTypesResponseData.FromCacheXml(xCurrencies.ToString());
    }

    private static XElement GetCurrencyElement(IDataRecord reader, out string currencyType)
    {
      currencyType = string.Empty;
      var xCurrency = new XElement("currency");

      for (var i = 0; i < reader.FieldCount; i++)
      {
        var fieldName = reader.GetName(i);
        if (fieldName == "gdshop_currencyType")
          currencyType = reader.GetString(i);
        xCurrency.Add(new XAttribute(fieldName, reader.GetValue(i)));
      }
      return xCurrency;
    }

    private static XElement AddCurrencyExchangeAttributes(IDataRecord reader, Dictionary<string, XElement> dictCurrencyElements)
    {
      var currencyType = string.Empty;
      var conversionRate = 0;
      decimal pricingBias = 0;
      decimal operatingBias = 0;
      for (var i = 0; i < reader.FieldCount; i++)
      {
        var fieldName = reader.GetName(i);

        if (fieldName == "transaction_gdshop_currencyType")
          currencyType = reader.GetString(i);
        else if (fieldName == "conversionRate")
          conversionRate = reader.GetInt32(i);
        else if (fieldName == "pricingBias")
          pricingBias = reader.GetValue(i) != DBNull.Value ? reader.GetDecimal(i) : 0;
        else if (fieldName == "operatingBias")
          operatingBias = reader.GetValue(i) != DBNull.Value ? reader.GetDecimal(i) : 0;
      }
      var exchangeRate = conversionRate / 10000.0;
      var exchangeRatePricing = exchangeRate / (double)(1 + pricingBias);
      var exchangeRateOperating = exchangeRate / (double)(1 + operatingBias);

      XElement xCurrency;
      dictCurrencyElements.TryGetValue(currencyType, out xCurrency);
      if (xCurrency != null)
      {
        xCurrency.Add(new XAttribute("exchangeRate", exchangeRate.ToString("0.0000")));
        xCurrency.Add(new XAttribute("exchangeRatePricing", exchangeRatePricing.ToString("0.0000")));
        xCurrency.Add(new XAttribute("exchangeRateOperating", exchangeRateOperating.ToString("0.0000")));
      }

      return xCurrency;
    }
  }
}
