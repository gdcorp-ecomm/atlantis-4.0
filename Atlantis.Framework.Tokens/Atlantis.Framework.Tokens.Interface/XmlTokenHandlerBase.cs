namespace Atlantis.Framework.Tokens.Interface
{
  public abstract class XmlTokenHandlerBase : TokenHandlerBase
  {
    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new XmlToken(TokenKey, tokenData, fullTokenString);
    }
  }
}
