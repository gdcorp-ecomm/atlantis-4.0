using System.Diagnostics;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Tokens.Interface
{
  internal class TokenHandlerManager
  {
    private const string NO_HANDLER = "No handler loaded.";
    private const string HANDLER_ERROR = "Handler Exception running EvaluateTokens";

    private readonly ITokenHandler _tokenHandler;
    private IToken _token;

    private readonly TokenHandlerStats _tokenHandlerStats;

    public TokenHandlerManager(string tokenKey, string tokenData, string fullTokenString, ITokenHandler tokenHandler, TokenHandlerStats tokenHandlerStats)
    {
      _tokenHandler = tokenHandler;
      _tokenHandlerStats = tokenHandlerStats;

      InitializeToken(tokenKey, tokenData, fullTokenString);
    }

    private void InitializeToken(string tokenKey, string tokenData, string fullTokenString)
    {
      if (_tokenHandler != null)
      {
        _token = _tokenHandler.CreateToken(tokenData, fullTokenString);
      }
      else
      {
        _token = new SimpleToken(tokenKey, tokenData, fullTokenString);
      }
    }

    private void SetErrorOnTokens(string errorMessage)
    {
      _token.TokenError = errorMessage;
      _token.TokenResult = string.Empty;
    }

    internal TokenEvaluationResult EvaluateToken(IProviderContainer container)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Errors;

      if (_tokenHandler != null)
      {
        Stopwatch callTimer = null;

        try
        {
          callTimer = Stopwatch.StartNew();
          result = _tokenHandler.EvaluateTokens(new[] { _token }, container);
          callTimer.Stop();

          if (_tokenHandlerStats != null)
          {
            _tokenHandlerStats.LogSuccess(callTimer);
          }
        }
        catch (Exception ex)
        {
          if (callTimer != null)
          {
            callTimer.Stop();
          }

          if (_tokenHandlerStats != null)
          {
            _tokenHandlerStats.LogFailure(callTimer);
          }

          SetErrorOnTokens(HANDLER_ERROR);

          ErrorLogHelper.LogErrors("TokenHandlerManager.EvaluateTokens." + _token.TokenKey,
                                   ex.Message,
                                   "TokenHandlerManager.EvaluateToken()",
                                   _token.RawTokenData,
                                   container);
        }
      }
      else
      {
        SetErrorOnTokens(NO_HANDLER);

        ErrorLogHelper.LogErrors("TokenHandlerManager.EvaluateTokens." + _token.TokenKey, 
                                 "No ITokenHandler found for tokenkey=" + _token.TokenKey,
                                 "TokenHandlerManager.EvaluateToken()", 
                                 _token.RawTokenData, 
                                 container);
      }

      return result;
    }

    internal string RenderToken(ITokenEncoding tokenEncoding)
    {
      string tokenResult = _token.TokenResult;

      if (tokenEncoding != null)
      {
        tokenResult = tokenEncoding.EncodeTokenResult(tokenResult);
      }

      return tokenResult;
    }
  }
}