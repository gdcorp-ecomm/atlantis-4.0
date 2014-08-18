using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Tokens.Interface;

namespace Atlantis.Framework.TH.Personalization
{
  internal class TargetMessageRenderContext
  {
    private IPersonalizationProvider _personalizationProvider;

    internal TargetMessageRenderContext(IProviderContainer container)
    {
      _personalizationProvider = container.Resolve<IPersonalizationProvider>();
    }

    internal bool RenderToken(IToken token)
    {
      bool result = true;

      TargetMessageToken targetMessageToken = token as TargetMessageToken;

      if (targetMessageToken != null)
      {
        switch (targetMessageToken.RenderType)
        {
          case "messagetag":
            result = RenderMessageTag(targetMessageToken);
            break;
          default:
            result = false;
            break;
        }
      }

      return result;
    }

    internal bool RenderMessageTag(TargetMessageToken token)
    {
      bool result = false;
      string tokenResult = null;

      if (token != null && !String.IsNullOrEmpty(token.RawTokenData))
      {
        var targetMessages = _personalizationProvider.GetTargetedMessages(token.InteractionPoint);

        if (targetMessages != null && targetMessages.ResultCode == 0)
        {
          foreach (var message in targetMessages.Messages)
          {
            foreach (var tag in message.MessageTags)
            {
              if (String.Compare(token.MessageTagName, tag.Name, StringComparison.OrdinalIgnoreCase) == 0)
              {
                tokenResult = message.MessageName;
                break;
              }

              if (!String.IsNullOrEmpty(tokenResult)) { break; }
            }
          }

          token.TokenResult = tokenResult ?? String.Empty;
          result = true;
        }
      }

      return result;
    }
  }
}
