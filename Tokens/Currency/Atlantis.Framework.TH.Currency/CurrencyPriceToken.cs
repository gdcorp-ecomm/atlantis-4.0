using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Currency
{
    public class CurrencyPriceToken : XmlToken
    {
      public string RenderType
      {
        get { return (TokenData != null) ? TokenData.Name.ToString() : string.Empty; }
      }

      public int UsdAmount { get; private set; }
      public string CurrencyType { get; private set; }
      public bool DropDecimal { get; private set; }
      public bool DropSymbol { get; private set; }
      public bool HtmlSymbol { get; private set; }
      public string NegativeFormat { get; private set; }

      public CurrencyPriceToken(string key, string data, string fullTokenString)
        : base(key, data, fullTokenString)
      {
        UsdAmount = GetAttributeInt("usdamount", 0);
        NegativeFormat = GetAttributeText("negative", "minus");
        HtmlSymbol = GetAttributeBool("htmlsymbol", true);
        DropDecimal = GetAttributeBool("dropdecimal", false);
        DropSymbol = GetAttributeBool("dropsymbol", false);
        CurrencyType = GetAttributeText("currencytype", null);
      }
    }
}
