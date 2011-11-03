using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;

namespace Atlantis.Framework.CDS.Tokenizer.Tokens
{
  public class BaseToken : IUniversalToken
  {
    protected const string OFFER = "offer";
    protected const string PROMO = "promo";
    protected const string PRODUCT = "product";
    protected const string PRICE = "price";
    protected const string SYMBOL = "symbol";
    protected const string QUICKHELP = "quickhelp";
    protected const string LINK = "link";

    protected string _originalToken = null;
    protected ITokenizerStrategy _strategy;
    protected string _renderedValue = null;

    protected List<string> tokens = new List<string>();

    public string OriginalToken
    {
      get { return _originalToken; }
    }

    public BaseToken(string originalToken)
    {
      _originalToken = originalToken;
      tokens.AddRange(originalToken.ToLowerInvariant().Replace("{{", "").Replace("}}", "").Split(new string[] { "::" }, StringSplitOptions.None));
    }

    public static IUniversalToken GetUniversalToken(string originalToken)
    {
      string tempToken = originalToken.Replace("{{", "").Replace("}}", "");

      if (tempToken.StartsWith(PRODUCT))
        return new ProductToken(originalToken);

      if (tempToken.StartsWith(PRICE))
        return new PriceToken(originalToken);

      if (tempToken.StartsWith(QUICKHELP, StringComparison.InvariantCultureIgnoreCase))
        return new QuickHelpToken(originalToken);

      if (tempToken.StartsWith(LINK))
        return new LinkToken(originalToken);

      return null;
    }

    public override string ToString()
    {
      if (_renderedValue == null)
      {
        if (_strategy == null)
          throw new InvalidProgramException("The strategy is null and execution cannot continue.");

        _renderedValue = _strategy.Process(tokens);
      }
      return _renderedValue;
    }

    public virtual string GetToken()
    {
      return _originalToken;
    }
  }
}
