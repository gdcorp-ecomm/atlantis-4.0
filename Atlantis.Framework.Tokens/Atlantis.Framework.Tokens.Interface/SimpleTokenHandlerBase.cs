namespace Atlantis.Framework.Tokens.Interface
{
  public abstract class SimpleTokenHandlerBase : TokenHandlerBase
  {
    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new SimpleToken(TokenKey, tokenData, fullTokenString);
    }
  }
}
