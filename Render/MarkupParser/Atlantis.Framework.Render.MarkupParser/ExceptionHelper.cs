using System.Data;

namespace Atlantis.Framework.Render.MarkupParser
{
  internal static class ExceptionHelper
  {
    internal static void ThrowParseError(string message, int lineNumber, string lineText)
    {
      throw new InvalidExpressionException(string.Format("Parse error. LineNumber: {0}, Message: {1}, Text: \"{2}\".", lineNumber, message, lineText));
    }
  }
}
