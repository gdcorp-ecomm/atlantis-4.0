using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Tokens.Monitor.WebTest.TokenHandlers
{
  public class XmlTokenHandler : XmlTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "testxml"; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, Framework.Interface.IProviderContainer container)
    {
      foreach (IToken token in tokens)
      {
        XmlToken xmlToken = token as XmlToken;
        if (xmlToken != null)
        {
          xmlToken.TokenResult = xmlToken.TokenData.Value;
        }
      }

      return TokenEvaluationResult.Success;
    }
  }
}
