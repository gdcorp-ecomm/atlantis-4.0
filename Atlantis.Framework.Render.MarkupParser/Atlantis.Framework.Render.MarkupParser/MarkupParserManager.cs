using System.Text;

namespace Atlantis.Framework.Render.MarkupParser
{
  public static class MarkupParserManager
  {
    public delegate bool EvaluateExpressionDelegate(string expression);

    public static StringBuilder ParseAndEvaluate(string markup, string preProcessorPrefix, EvaluateExpressionDelegate evaluateExpressionDelegate)
    {
      StringBuilder resultingMarkup;

      using (Parser parser = new Parser())
      {
        resultingMarkup = parser.ParseAndEvaluate(markup, preProcessorPrefix, evaluateExpressionDelegate);
      }

      return resultingMarkup;
    }
  }
}
