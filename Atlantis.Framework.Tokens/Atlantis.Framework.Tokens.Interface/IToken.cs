namespace Atlantis.Framework.Tokens.Interface
{
  public interface IToken
  {
    string FullTokenString { get; }
    string TokenKey { get; }
    string RawTokenData { get; }
    string TokenResult { get; set; }
    string TokenError { get; set; }
  }
}
