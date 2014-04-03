using Atlantis.Framework.Tokens.Interface;
using System.Linq;

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
    public string NoStrikeTemplate { get; private set; }
    public string StrikeTemplate
    {
      get;
      private set;
    }
    public string WrapTagName
    {
      get;
      private set;
    }

    public string WrapCssClass
    {
      get;
      private set;
    }

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
      WrapTagName = GetAttributeText("symboltagname", string.Empty);
      WrapCssClass = GetAttributeText("symbolcssclass", string.Empty);

      if (TokenData.Elements("nostrike").Any())
      {
        NoStrikeTemplate = TokenData.Element("nostrike").Value;
      }
      else
      {
        NoStrikeTemplate = string.Empty;
      }

      if (TokenData.Elements("strike").Any())
      {
        StrikeTemplate = TokenData.Element("strike").Value;
      }
      else
      {
        StrikeTemplate = string.Empty;
      }

    }
  }
}
