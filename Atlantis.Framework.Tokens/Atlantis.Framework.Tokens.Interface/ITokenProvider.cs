namespace Atlantis.Framework.Tokens.Interface
{
  public interface ITokenProvider
  {
    event TokenReplacedEventHandler TokenReplaced;

    void ReplaceTokens(string inputText, out string resultText);

    void ReplaceTokens(string inputText, ITokenEncoding tokenDataEncoding, out string resultText);
  }
}
