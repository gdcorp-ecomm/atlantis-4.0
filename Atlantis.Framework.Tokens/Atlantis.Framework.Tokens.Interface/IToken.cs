namespace Atlantis.Framework.Tokens.Interface
{
  /// <summary>
  /// All token classes must implement the IToken interface
  /// </summary>
  public interface IToken
  {
    string FullTokenString { get; }
    string TokenKey { get; }
    string RawTokenData { get; }
    string TokenResult { get; set; }
    string TokenError { get; set; }
  }
}
