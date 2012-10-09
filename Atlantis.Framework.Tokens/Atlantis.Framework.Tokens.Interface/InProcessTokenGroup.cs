using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atlantis.Framework.Tokens.Interface
{
  internal class InProcessTokenGroup
  {
    private const string _NOHANDLER = "No handler loaded.";
    public ITokenHandler TokenHandler { get; private set; }

    List<IToken> _inProcessTokens;
    HashSet<string> _uniqueReplacements;

    public InProcessTokenGroup(ITokenHandler handler)
    {
      TokenHandler = handler;
      _inProcessTokens = new List<IToken>();
      _uniqueReplacements = new HashSet<string>(StringComparer.Ordinal);
    }

    public void AddInProcessToken(string tokenKey, string tokenData, string fullTokenString)
    {
      if (!_uniqueReplacements.Contains(fullTokenString))
      {
        IToken token;
        if (TokenHandler != null)
        {
          token = TokenHandler.CreateToken(tokenData, fullTokenString);
        }
        else
        {
          token = new SimpleToken(tokenKey, tokenData, fullTokenString);
        }

        _inProcessTokens.Add(token);
        _uniqueReplacements.Add(token.FullTokenString);
      }
    }

    public TokenEvaluationResult EvaluateTokens(IProviderContainer container)
    {
      TokenEvaluationResult result;

      if (TokenHandler != null)
      {
        result = TokenHandler.EvaluateTokens(_inProcessTokens, container);
      }
      else
      {
        result = TokenEvaluationResult.Errors;
        foreach (var inProcessToken in _inProcessTokens)
        {
          if (TokenHandler == null)
          {
            inProcessToken.TokenError = _NOHANDLER;
            inProcessToken.TokenResult = string.Empty;
          }
        }

        if ((_inProcessTokens.Count > 0) && (container.CanResolve<IDebugContext>()))
        {
          IToken first = _inProcessTokens[0];
          IDebugContext debug = container.Resolve<IDebugContext>();
          string message = "No ITokenHandler found for tokenkey=" + first.TokenKey;
          debug.LogDebugTrackingData("InProcessTokenGroup.EvaluateTokens." + first.TokenKey, message);
        }
      }

      return result;
    }

    public void ExecuteReplacements(StringBuilder replacementText)
    {
      foreach (IToken token in _inProcessTokens)
      {
        replacementText.Replace(token.FullTokenString, token.TokenResult);
      }
    }

    public IEnumerable<IToken> InProcessTokens
    {
      get { return _inProcessTokens; }
    }
  }
}
