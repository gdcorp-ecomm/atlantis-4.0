using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Products
{
  public class ProductPriceToken : XmlToken
  {
    public string RenderType
    {
      get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
    }

    public bool AllowMask { get; private set; }
    public string CurrencyType { get; private set; }
    public int ProductId { get; private set; }
    public bool DropDecimal { get; private set; }
    public bool DropSymbol { get; private set; }
    public string Period { get; private set; }
    public bool HtmlSymbol { get; private set; }
    public string NegativeFormat { get; private set; }
    public int PriceType { get; private set; }
    public string ISC { get; private set; }

    public ProductPriceToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
      ISC = GetAttributeText("isc", null);
      PriceType = GetAttributeInt("pricetype", -1);
      NegativeFormat = GetAttributeText("negative", "minus");
      HtmlSymbol = GetAttributeBool("htmlsymbol", true);
      DropDecimal = GetAttributeBool("dropdecimal", false);
      DropSymbol = GetAttributeBool("dropsymbol", false);
      ProductId = GetAttributeInt("productid", 0);
      CurrencyType = GetAttributeText("currencytype", null);
      Period = GetAttributeText("period", string.Empty);
      AllowMask = GetAttributeBool("allowmask", true);
    }
  }
}
