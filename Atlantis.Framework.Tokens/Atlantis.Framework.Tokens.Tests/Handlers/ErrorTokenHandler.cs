using Atlantis.Framework.Tokens.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Tokens.Tests.Handlers
{
  [PartNotDiscoverable()]
  public class ErrorTokenHandler : SimpleTokenHandlerBase
  {
    public override string TokenKey
    {
      get { return "testerror"; }
    }

    public override TokenEvaluationResult EvaluateTokens(IEnumerable<IToken> tokens, Framework.Interface.IProviderContainer container)
    {
      throw new ArgumentException("OMG Fail!");
    }
  }
}
