using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atlantis.Framework.Tokens.Interface
{
  internal class InProcessTokenGroup
  {
    private const string _NOHANDLER = "No handler loaded.";
    private const string _HANDLERERROR = "Handler Exception running EvaluateTokens";

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

    private void SetErrorOnTokens(string errorMessage)
    {
      foreach (var inProcessToken in _inProcessTokens)
      {
        inProcessToken.TokenError = errorMessage;
        inProcessToken.TokenResult = string.Empty;
      }
    }

    public TokenEvaluationResult EvaluateTokens(IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Errors;

      IDebugContext debug = null;
      if (container.CanResolve<IDebugContext>())
      {
        debug = container.Resolve<IDebugContext>();
      }

      if (_inProcessTokens.Count == 0)
      {
        result = TokenEvaluationResult.Success;
      }
      else if (TokenHandler != null)
      {
        try
        {
          result = TokenHandler.EvaluateTokens(_inProcessTokens, container);
        }
        catch (Exception ex)
        {
          SetErrorOnTokens(_HANDLERERROR);
          if (debug != null)
          {
            debug.LogDebugTrackingData("InProcessTokenGroup.EvaluateTokens." + TokenHandler.TokenKey, ex.Message);
          }
        }
      }
      else // no handler exists
      {
        SetErrorOnTokens(_NOHANDLER);
        IToken first = _inProcessTokens[0];
        string message = "No ITokenHandler found for tokenkey=" + first.TokenKey;
        debug.LogDebugTrackingData("InProcessTokenGroup.EvaluateTokens." + first.TokenKey, message);
      }

      return result;
    }

    public void ExecuteReplacements(StringBuilder replacementText, ITokenEncoding tokenEncoding)
    {
      foreach (IToken token in _inProcessTokens)
      {
        string tokenResult = token.TokenResult;
        if (tokenEncoding != null)
        {
          tokenResult = tokenEncoding.EncodeTokenResult(tokenResult);
        }

        replacementText.Replace(token.FullTokenString, tokenResult);
      }
    }

    public IEnumerable<IToken> InProcessTokens
    {
      get { return _inProcessTokens; }
    }
  }
}
