using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Personalization
{
  /// <summary>
  /// Example token: [@T[targetmessagename:<messagetag name="EngmtActNewCustSurveyMobileDLP" appid="2" interactionpoint="Homepage"></messagetag>]@T]
  /// </summary>
  public class TargetMessageTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "targetmessagename"; }
    }

    public override IToken CreateToken(string tokenData, string fullTokenString)
    {
      return new TargetMessageToken(TokenKey, tokenData, fullTokenString);
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Success;
      TargetMessageRenderContext contextRenderer = new TargetMessageRenderContext(container);

      foreach (var token in tokens)
      {
        if (!contextRenderer.RenderToken(token))
        {
          result = TokenEvaluationResult.Errors;
        }
      }

      return result;
    }
  }
}
