
namespace Atlantis.Framework.Render.MarkupParser
{
  public static class MarkupParserManager
  {
    private const string IF_PRE_PROCESSOR_START = "if(";

    public delegate bool EvaluateExpressionDelegate(string expression);

    private static bool MarkupContainsExpression(string markup, string preProcessorPrefix)
    {
      return markup.Contains(preProcessorPrefix + IF_PRE_PROCESSOR_START);
    }

    public static string ParseAndEvaluate(string markup, string preProcessorPrefix, EvaluateExpressionDelegate evaluateExpressionDelegate)
    {
      string resultingMarkup;

      if (!string.IsNullOrEmpty(markup) && MarkupContainsExpression(markup, preProcessorPrefix))
      {
        using (Parser parser = new Parser())
        {
          resultingMarkup = parser.ParseAndEvaluate(markup, preProcessorPrefix, evaluateExpressionDelegate);
        }
      }
      else
      {
        resultingMarkup = markup;
      }

      return resultingMarkup;
    }
  }
}
