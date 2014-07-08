
namespace Atlantis.Framework.Render.ExpressionParser
{
  public interface IExpressionParserManager
  {
    ExpressionParserManager.EvaluateFunctionDelegate EvaluateExpressionHandler { get; set; }
    bool EvaluateExpression(string rawExpression);
  }
}