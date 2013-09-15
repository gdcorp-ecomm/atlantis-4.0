using System.Xml.Linq;
using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Currency.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  public class CurrencyTests
  {
    [TestMethod]
    public void CurrencyTypesRequestDataCacheKey()
    {
      CurrencyTypesRequestData request1 = new CurrencyTypesRequestData();
      CurrencyTypesRequestData request2 = new CurrencyTypesRequestData();
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void CurrencyInfoCreateValid()
    {
      string currencyItem = "<currency gdshop_currencyType=\"AUD\" description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"B$\" isTransactional=\"1\" exchangeRate=\"1.0025\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0000\"/>";
      XElement currencyElement = XElement.Parse(currencyItem);
      CurrencyInfo currencyInfo = CurrencyInfo.FromCacheElement(currencyElement);

      Assert.IsNotNull(currencyInfo);
      Assert.AreEqual("AUD", currencyInfo.CurrencyType);
      Assert.AreEqual("Australian Dollar", currencyInfo.Description);
      Assert.AreEqual("Australian Dollars", currencyInfo.DescriptionPlural);
      Assert.AreEqual("A$", currencyInfo.Symbol);
      Assert.AreEqual("B$", currencyInfo.SymbolHtml);
      Assert.AreEqual(CurrencySymbolPositionType.Prefix, currencyInfo.SymbolPosition);
      Assert.AreEqual(2, currencyInfo.DecimalPrecision);
      Assert.AreEqual(".", currencyInfo.DecimalSeparator);
      Assert.AreEqual(",", currencyInfo.ThousandsSeparator);
      Assert.AreEqual(true, currencyInfo.IsTransactional);
      Assert.AreEqual(1.0025, currencyInfo.ExchangeRate);
      Assert.AreEqual(1.0, currencyInfo.ExchangeRateOperating);
      Assert.AreEqual(0.9733, currencyInfo.ExchangeRatePricing);
    }

    [TestMethod]
    public void CurrencyInfoCreateInvalid()
    {
      string currencyItem = "<currency description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"B$\" isTransactional=\"1\" exchangeRate=\"1.0025\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0000\"/>";
      XElement currencyElement = XElement.Parse(currencyItem);
      CurrencyInfo currencyInfo = CurrencyInfo.FromCacheElement(currencyElement);
      Assert.IsNull(currencyInfo);
    }

    [TestMethod]
    public void CurrencyInfoCreateDefaults()
    {
      string currencyItem = "<currency gdshop_currencyType=\"AUD\" />";
      XElement currencyElement = XElement.Parse(currencyItem);
      CurrencyInfo currencyInfo = CurrencyInfo.FromCacheElement(currencyElement);

      Assert.IsNotNull(currencyInfo);
      Assert.AreEqual("AUD", currencyInfo.CurrencyType);
      Assert.AreEqual(string.Empty, currencyInfo.Description);
      Assert.AreEqual(string.Empty, currencyInfo.DescriptionPlural);
      Assert.AreEqual("$", currencyInfo.Symbol);
      Assert.AreEqual("$", currencyInfo.SymbolHtml);
      Assert.AreEqual(CurrencySymbolPositionType.Prefix, currencyInfo.SymbolPosition);
      Assert.AreEqual(2, currencyInfo.DecimalPrecision);
      Assert.AreEqual(".", currencyInfo.DecimalSeparator);
      Assert.AreEqual(",", currencyInfo.ThousandsSeparator);
      Assert.AreEqual(false, currencyInfo.IsTransactional);
      Assert.AreEqual(0, currencyInfo.ExchangeRate);
      Assert.AreEqual(0, currencyInfo.ExchangeRateOperating);
      Assert.AreEqual(0, currencyInfo.ExchangeRatePricing);
    }

    [TestMethod]
    public void CurrencyNotTransactionalAndSuffix()
    {
      string currencyItem = "<currency gdshop_currencyType=\"AUD\" isTransactional=\"0\" currencySymbolPosition=\"Suffix\" />";
      XElement currencyElement = XElement.Parse(currencyItem);
      CurrencyInfo currencyInfo = CurrencyInfo.FromCacheElement(currencyElement);

      Assert.AreEqual(false, currencyInfo.IsTransactional);
      Assert.AreEqual(CurrencySymbolPositionType.Suffix, currencyInfo.SymbolPosition);
    }

    [TestMethod]
    public void CurrencyInfoUSDAlwaysTransactional()
    {
      string currencyItem = "<currency gdshop_currencyType=\"USD\" isTransactional=\"0\" />";
      XElement currencyElement = XElement.Parse(currencyItem);
      CurrencyInfo currencyInfo = CurrencyInfo.FromCacheElement(currencyElement);

      Assert.IsNotNull(currencyInfo);
      Assert.AreEqual(true, currencyInfo.IsTransactional);
    }

    [TestMethod]
    public void CurrencyInfoEquals()
    {
      string currencyItem = "<currency gdshop_currencyType=\"USD\" isTransactional=\"0\" />";
      XElement currencyElement = XElement.Parse(currencyItem);
      CurrencyInfo currencyInfo = CurrencyInfo.FromCacheElement(currencyElement);

      string currencyItem2 = "<currency gdshop_currencyType=\"usd\" isTransactional=\"0\" />";
      XElement currencyElement2 = XElement.Parse(currencyItem2);
      CurrencyInfo currencyInfo2 = CurrencyInfo.FromCacheElement(currencyElement2);

      Assert.IsTrue(currencyInfo.Equals(currencyInfo2));
    }

    [TestMethod]
    public void CurrencyInfoBadNumbers()
    {
      string currencyItem = "<currency gdshop_currencyType=\"AUD\" description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"blue\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"B$\" isTransactional=\"yellow\" exchangeRate=\"green\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0000\"/>";
      XElement currencyElement = XElement.Parse(currencyItem);
      CurrencyInfo currencyInfo = CurrencyInfo.FromCacheElement(currencyElement);

      Assert.AreEqual(2, currencyInfo.DecimalPrecision);
      Assert.AreEqual(0, currencyInfo.ExchangeRate);
      Assert.IsFalse(currencyInfo.IsTransactional);
    }

    [TestMethod]
    public void CurrencyTypesTrimmedSymbols()
    {
      string currencyItem = "<currency gdshop_currencyType=\"BRL\" description=\"Brazilian Real\" currencySymbol=\" R$ \" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Brazilian Reais\" currencySymbolHtml=\" R$ \" isTransactional=\"1\" exchangeRate=\"0.4948\" exchangeRatePricing=\"0.4804\" exchangeRateOperating=\"0.4948\"/>";
      XElement currencyElement = XElement.Parse(currencyItem);
      CurrencyInfo currencyInfo = CurrencyInfo.FromCacheElement(currencyElement);
      Assert.AreEqual("R$", currencyInfo.Symbol);
      Assert.AreEqual("R$", currencyInfo.SymbolHtml);
    }

    [TestMethod]
    public void CurrencyTypesResponseValid()
    {
      string currencyData = "<Cache count=\"33\" expires=\"5/13/2013 3:47:26 PM\" methods=\"getcurrencydata\"><currency gdshop_currencyType=\"ARS\" description=\"Argentine Peso\" currencySymbol=\"$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Argentine Pesos\" currencySymbolHtml=\"$\" isTransactional=\"1\" exchangeRate=\"0.1912\" exchangeRatePricing=\"0.1856\" exchangeRateOperating=\"0.1912\"/><currency gdshop_currencyType=\"AUD\" description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"A$\" isTransactional=\"1\" exchangeRate=\"1.0025\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0025\"/><currency gdshop_currencyType=\"BRL\" description=\"Brazilian Real\" currencySymbol=\"R$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Brazilian Reais\" currencySymbolHtml=\"R$\" isTransactional=\"1\" exchangeRate=\"0.4948\" exchangeRatePricing=\"0.4804\" exchangeRateOperating=\"0.4948\"/></Cache>";
      var response = CurrencyTypesResponseData.FromCacheXml(currencyData);
      Assert.AreEqual(3, response.Count);

      var currencyInfo = response["ars"];
      Assert.IsNotNull(currencyInfo);

      currencyInfo = response["usd"];
      Assert.IsNull(currencyInfo);

      int count = 0;
      foreach (ICurrencyInfo info in response)
      {
        count++;
      }
      Assert.AreEqual(3, count);
    }

    [TestMethod]
    public void CurrencyTypesResponseMethods()
    {
      string currencyData = "<Cache count=\"33\" expires=\"5/13/2013 3:47:26 PM\" methods=\"getcurrencydata\"><currency gdshop_currencyType=\"ARS\" description=\"Argentine Peso\" currencySymbol=\"$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Argentine Pesos\" currencySymbolHtml=\"$\" isTransactional=\"1\" exchangeRate=\"0.1912\" exchangeRatePricing=\"0.1856\" exchangeRateOperating=\"0.1912\"/><currency gdshop_currencyType=\"AUD\" description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"A$\" isTransactional=\"1\" exchangeRate=\"1.0025\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0025\"/><currency gdshop_currencyType=\"BRL\" description=\"Brazilian Real\" currencySymbol=\"R$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Brazilian Reais\" currencySymbolHtml=\"R$\" isTransactional=\"1\" exchangeRate=\"0.4948\" exchangeRatePricing=\"0.4804\" exchangeRateOperating=\"0.4948\"/></Cache>";
      var response = CurrencyTypesResponseData.FromCacheXml(currencyData);

      Assert.IsNull(response.GetException());
      XElement.Parse(response.ToXML());

      var objectEnumerator = ((System.Collections.IEnumerable)response).GetEnumerator();
    }

    [TestMethod]
    public void CurrencyTypesResponseBadXml()
    {
      string currencyData = "<Cache count=\"33\" expires=\"5/13/2013 3:47:26 PM\" methods=\"getcurrencydata\"><currency gdshop_currencyType=\"ARS\" description=\"Argentine Peso\" currencySymbol=\"$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Argentine Pesos\" currencySymbolHtml=\"$\" isTransactional=\"1\" exchangeRate=\"0.1912\" exchangeRatePricing=\"0.1856\" exchangeRateOperating=\"0.1912\"/><currency gdshop_currencyType=\"AUD\" description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"A$\" isTransactional=\"1\" exchangeRate=\"1.0025\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0025\"/><currency gdshop_currencyType=\"BRL\" description=\"Brazilian Real\" currencySymbol=\"R$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Brazilian Reais\" currencySymbolHtml=\"R$\" isTransactional=\"1\" exchangeRate=\"0.4948\" exchangeRatePricing=\"0.4804\" exchangeRateOperating=\"0.4948\"/>";
      var response = CurrencyTypesResponseData.FromCacheXml(currencyData);
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    public void CurrencyTypesResponseEmptyXml()
    {
      string currencyData = "<Cache />";
      var response = CurrencyTypesResponseData.FromCacheXml(currencyData);
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    public void CurrencyTypesResponseNoGoodElements()
    {
      string currencyData = "<Cache count=\"33\" expires=\"5/13/2013 3:47:26 PM\" methods=\"getcurrencydata\"><currency description=\"Argentine Peso\" currencySymbol=\"$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Argentine Pesos\" currencySymbolHtml=\"$\" isTransactional=\"1\" exchangeRate=\"0.1912\" exchangeRatePricing=\"0.1856\" exchangeRateOperating=\"0.1912\"/><currency description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"A$\" isTransactional=\"1\" exchangeRate=\"1.0025\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0025\"/><currency description=\"Brazilian Real\" currencySymbol=\"R$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Brazilian Reais\" currencySymbolHtml=\"R$\" isTransactional=\"1\" exchangeRate=\"0.4948\" exchangeRatePricing=\"0.4804\" exchangeRateOperating=\"0.4948\"/></Cache>";
      var response = CurrencyTypesResponseData.FromCacheXml(currencyData);
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    public void CurrencyTypesResponseInValidSomeGoodElements()
    {
      string currencyData = "<Cache count=\"33\" expires=\"5/13/2013 3:47:26 PM\" methods=\"getcurrencydata\"><currency gdshop_currencyType=\"\" description=\"Argentine Peso\" currencySymbol=\"$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Argentine Pesos\" currencySymbolHtml=\"$\" isTransactional=\"1\" exchangeRate=\"0.1912\" exchangeRatePricing=\"0.1856\" exchangeRateOperating=\"0.1912\"/><currency gdshop_currencyType=\"AUD\" description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"A$\" isTransactional=\"1\" exchangeRate=\"1.0025\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0025\"/><currency gdshop_currencyType=\"BRL\" description=\"Brazilian Real\" currencySymbol=\"R$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Brazilian Reais\" currencySymbolHtml=\"R$\" isTransactional=\"1\" exchangeRate=\"0.4948\" exchangeRatePricing=\"0.4804\" exchangeRateOperating=\"0.4948\"/></Cache>";
      var response = CurrencyTypesResponseData.FromCacheXml(currencyData);
      Assert.AreEqual(2, response.Count);
    }

    [TestMethod]
    public void CurrencyTypesResponseContains()
    {
      string currencyData = "<Cache count=\"33\" expires=\"5/13/2013 3:47:26 PM\" methods=\"getcurrencydata\"><currency gdshop_currencyType=\"ARS\" description=\"Argentine Peso\" currencySymbol=\"$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Argentine Pesos\" currencySymbolHtml=\"$\" isTransactional=\"1\" exchangeRate=\"0.1912\" exchangeRatePricing=\"0.1856\" exchangeRateOperating=\"0.1912\"/><currency gdshop_currencyType=\"AUD\" description=\"Australian Dollar\" currencySymbol=\"A$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\".\" thousandsSeparator=\",\" fixedFormatMask=\"000.00\" repeatingFormatMask=\"000,\" description2=\"Australian Dollars\" currencySymbolHtml=\"A$\" isTransactional=\"1\" exchangeRate=\"1.0025\" exchangeRatePricing=\"0.9733\" exchangeRateOperating=\"1.0025\"/><currency gdshop_currencyType=\"BRL\" description=\"Brazilian Real\" currencySymbol=\"R$\" currencySymbolPosition=\"Prefix\" decimalPrecision=\"2\" decimalSeparator=\",\" thousandsSeparator=\".\" fixedFormatMask=\"000,00\" repeatingFormatMask=\"000.\" description2=\"Brazilian Reais\" currencySymbolHtml=\"R$\" isTransactional=\"1\" exchangeRate=\"0.4948\" exchangeRatePricing=\"0.4804\" exchangeRateOperating=\"0.4948\"/></Cache>";
      var response = CurrencyTypesResponseData.FromCacheXml(currencyData);

      Assert.IsTrue(response.Contains("ars"));
      Assert.IsFalse(response.Contains("usq"));
    }

    const int REQUESTTYPE = 693;

    [TestMethod]
    public void CurrencyTypesRequest()
    {
      var request = new CurrencyTypesRequestData();
      var response = (CurrencyTypesResponseData)Engine.Engine.ProcessRequest(request, REQUESTTYPE);
      Assert.IsNotNull(response);
      Assert.IsTrue(response.Count > 0);
    }

    [TestMethod]
    public void CurrencyTypesIsActiveCheckRequest()
    {
      var request = new CurrencyTypesRequestData();
      var response = (CurrencyTypesResponseData)Engine.Engine.ProcessRequest(request, REQUESTTYPE);
      Assert.IsNotNull(response);
      var isActive = true;
      foreach (var currInfo in response)
      {
        isActive = isActive && currInfo.IsActive;
      }
      Assert.IsTrue(isActive);
    }


  }
}
