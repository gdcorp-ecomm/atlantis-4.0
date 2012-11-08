namespace Atlantis.Framework.Tokens.Interface
{
  /// <summary>
  /// Used to provide methods to decode tokendata that may be in an arbirtry encoded format
  /// and subsequently reencode the token result that will replace the token
  /// </summary>
  public interface ITokenEncoding
  {
    string DecodeTokenData(string rawTokenData);
    string EncodeTokenResult(string tokenResult);
  }
}
