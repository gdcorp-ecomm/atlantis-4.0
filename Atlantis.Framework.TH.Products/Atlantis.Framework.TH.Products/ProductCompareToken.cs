using Atlantis.Framework.Tokens.Interface;
using System.Linq;

namespace Atlantis.Framework.TH.Products
{
  class ProductCompareToken : XmlToken
  {
    public string RenderType
    {
      get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
    }

    public int PrimaryProductId { get; private set; }
    public int SecondaryProductId { get; private set; }
    public int SecondaryPrice { get; private set; }
    public string SecondaryPeriod { get; private set; }
    public int HideBelow { get; private set; }
    public string Html { get; private set; }

    public ProductCompareToken(string key, string data, string fullTokenString)
      : base(key, data, fullTokenString)
    {
      PrimaryProductId = GetAttributeInt("primaryproductid", 0);
      SecondaryProductId = GetAttributeInt("secondaryproductid", 0);
      SecondaryPrice = GetAttributeInt("secondaryprice", 0);
      SecondaryPeriod = GetAttributeText("secondaryperiod", string.Empty);
      HideBelow = GetAttributeInt("hidebelow", 0);

      if (TokenData.Elements("html").Any())
      {
        Html = TokenData.Element("html").Value;
      }
      else
      {
        Html = string.Empty;
      }
    }
  }
}
