using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.TargetedMessages
{
  /// <summary>
  /// Example token: [@T[targetmessage:<tokendata messagetag="EngmtActNewCustSurveyMobileDLP"></tokendata>@T]
  /// </summary>
  public class TargetMessageTokenHandler : SimpleTokenHandlerBase
  {
    private const string ATTR_MESSAGE_TAG = "messagetag";

    public override string TokenKey
    {
      get { return "targetmessage"; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Success;
      const string errorSource = "TargetMessages.EvaluateTokens";

      foreach (var token in tokens)
      {
        SimpleToken simpleToken = token as SimpleToken;
        XElement xmlTokenData = null;
        string tokenResult = null;

        if (simpleToken != null && !String.IsNullOrWhiteSpace(simpleToken.RawTokenData))
        {
          try
          {
            xmlTokenData = XElement.Parse(simpleToken.RawTokenData);
          }
          catch (Exception ex)
          {
            result = TokenEvaluationResult.Errors;
            LogDebugMessage(container, ex.ToString(), errorSource);
          }

          if (xmlTokenData != null)
          {
            var personalizationProvider = container.Resolve<IPersonalizationProvider>();
            var targetedMessages = personalizationProvider.GetTargetedMessages();

            foreach (var message in targetedMessages.Messages)
            {
              if (!String.IsNullOrEmpty(tokenResult)) { break; }

              foreach (MessageTag messagetag in message.MessageTags)
              {
                if (xmlTokenData.Attribute(ATTR_MESSAGE_TAG).Value == messagetag.Name)
                {
                  tokenResult = message.MessageId;
                  break;
                }
              }
            }
          }
        }

        token.TokenResult = tokenResult ?? String.Empty;
      }

      return result;
    }

    private void LogDebugMessage(IProviderContainer container, string message, string errorSource)
    {
      IDebugContext debugContext = null;
      if (container.CanResolve<IDebugContext>())
      {
        debugContext = container.Resolve<IDebugContext>();
        debugContext.LogDebugTrackingData(errorSource, message);
      }
    }

  }
}
