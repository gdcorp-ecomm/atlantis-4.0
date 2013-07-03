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
  /// Example token: [@T[targetmessageid:EngmtActNewCustSurveyMobileDLP]@T]
  /// </summary>
  public class TargetMessageTokenHandler : SimpleTokenHandlerBase
  {
    private const string ATTR_MESSAGE_TAG = "messagetag";

    public override string TokenKey
    {
      get { return "targetmessageid"; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Success;
      foreach (var token in tokens)
      {
        SimpleToken simpleToken = token as SimpleToken;

        string tokenResult = null;

        if (simpleToken != null && !String.IsNullOrEmpty(simpleToken.RawTokenData))
        {
          var personalizationProvider = container.Resolve<IPersonalizationProvider>();
          var targetedMessages = personalizationProvider.GetTargetedMessages();

          foreach (var message in targetedMessages.Messages)
          {
            //Ignore Case
            foreach (MessageTag messagetag in message.MessageTags)
            {
              if (String.Compare(simpleToken.TokenData, messagetag.Name, StringComparison.OrdinalIgnoreCase) == 0)
              {
                tokenResult = message.MessageId;
                break;
              }
            }
            if (!String.IsNullOrEmpty(tokenResult)) { break; }
          }
        }

        token.TokenResult = tokenResult ?? String.Empty;
      }

      return result;
    }
  }
}
