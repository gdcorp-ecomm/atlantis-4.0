using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Render.MarkupParser;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Conditions.Interface
{
  public class ConditionRenderHandler : IRenderHandler
  {
    private const string PRE_PROCESSOR_PREFIX = "##";

    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      ExpressionParserManager expressionParserManager = new ExpressionParserManager(providerContainer);
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      string modifiedContent = MarkupParserManager.ParseAndEvaluate(processRenderContent.Content, PRE_PROCESSOR_PREFIX, expressionParserManager.EvaluateExpression);

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
