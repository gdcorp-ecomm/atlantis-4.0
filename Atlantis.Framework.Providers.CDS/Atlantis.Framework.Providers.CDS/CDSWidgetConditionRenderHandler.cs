using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Render.MarkupParser;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Providers.CDS
{
  internal class CDSWidgetConditionRenderHandler : IRenderHandler
  {
    private const string PRE_PROCESSOR_PREFIX = "##";
    private const string CARRAIGE_RETURN_ENCODED = "\\r\\n";
    private const string CARRAIGE_RETURN_DECODED = "\r\n";

    public void ProcessContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      ExpressionParserManager expressionParserManager = new ExpressionParserManager(providerContainer);
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      renderContent.Content = renderContent.Content.Replace(CARRAIGE_RETURN_ENCODED, CARRAIGE_RETURN_DECODED);

      string modifiedContent = MarkupParserManager.ParseAndEvaluate(renderContent.Content, PRE_PROCESSOR_PREFIX, expressionParserManager.EvaluateExpression);

      renderContent.Content = modifiedContent.Replace(CARRAIGE_RETURN_DECODED, CARRAIGE_RETURN_ENCODED);
    }
  }
}