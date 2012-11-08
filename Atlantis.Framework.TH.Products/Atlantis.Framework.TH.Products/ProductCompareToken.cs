using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Products
{
  class ProductCompareToken : XmlToken
  {
    public string RenderType
    {
      get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
    }

    public const string CALCULATE_TIMES = "X";
    public const string CALCULATE_PERCENT = "%";

    public int PrimaryProductId { get; private set; }
    public int PrimaryPrice { get; private set; }
    public int SecondaryProductId { get; private set; }
    public int SecondaryPrice { get; private set; }
    public string Calculate { get; private set; }
    public string Html { get; private set; }
    public int HideBelow { get; private set; }

    public ProductCompareToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
      PrimaryProductId = GetAttributeInt("primaryproductid", 0);
      PrimaryPrice = GetAttributeInt("primaryprice", 0);
      SecondaryProductId = GetAttributeInt("secondaryproductid", 0);
      SecondaryPrice = GetAttributeInt("secondaryprice", 0);
      Calculate = GetAttributeText("calculate", string.Empty);
      Html = GetAttributeText("html", string.Empty);
      HideBelow = GetAttributeInt("hidebelow", 0);
    }
  }
}
