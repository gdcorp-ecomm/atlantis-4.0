using System.Diagnostics;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Tokens.Interface
{
  public static class TokenManager
  {
    const string _TOKENKEY = "tokenkey";
    const string _TOKENDATA = "tokendata";
    const string _DEFAULTTOKENPATTERN = @"\[@T\[(?<tokenkey>[a-zA-z0-9]*?):(?<tokendata>.*?)\]@T\]";
    private static Dictionary<string, ITokenHandler> _tokenHandlers;
    private static readonly Dictionary<string, TokenHandlerStats> _tokenHandlersStats;
    private static List<Regex> _tokenExpressions;

    static TokenManager()
    {
      _tokenHandlers = new Dictionary<string, ITokenHandler>(StringComparer.OrdinalIgnoreCase);
      _tokenHandlersStats = new Dictionary<string, TokenHandlerStats>(StringComparer.OrdinalIgnoreCase);

      _tokenExpressions = new List<Regex>();

      Regex defaultTokenEx = new Regex(_DEFAULTTOKENPATTERN, RegexOptions.Singleline | RegexOptions.Compiled);
      _tokenExpressions.Add(defaultTokenEx);
    }

    public static void AutoRegisterTokenHandlers()
    {
      AutoRegisterTokenHandlers(null);
    }

    public static void AutoRegisterTokenHandlers(params Assembly[] additionalAssemblies)
    {
      using (DynamicTokenHandlerLoader tokenHandlerLoader = new DynamicTokenHandlerLoader(additionalAssemblies))
      {
        foreach (var lazyHandler in tokenHandlerLoader.TokenHandlersFound)
        {
          RegisterTokenHandler(lazyHandler.Value);
        }
      }
    }

    public static void RegisterTokenHandler(ITokenHandler tokenHandler)
    {
      if (tokenHandler == null)
      {
        throw new ArgumentException("tokenHandler cannot be null.");
      }

      if (string.IsNullOrEmpty(tokenHandler.TokenKey))
      {
        throw new ArgumentException("tokenHandler does not have valid TokenKey.");
      }

      _tokenHandlers[tokenHandler.TokenKey] = tokenHandler;
      _tokenHandlersStats[tokenHandler.TokenKey] = new TokenHandlerStats();

    }

    public static void RegisterTokenExpression(Regex tokenExpression)
    {
      if (tokenExpression == null)
      {
        throw new ArgumentException("tokenExpression cannot be null.");
      }

      if (!tokenExpression.Options.HasFlag(RegexOptions.Compiled))
      {
        throw new ArgumentException("tokenExpression must have Compiled option turned on.");
      }

      bool tokenkeyFound = false;
      bool tokendataFound = false;

      string[] groupNames = tokenExpression.GetGroupNames();
      if (groupNames != null)
      {
        foreach (string groupName in groupNames)
        {
          if ("tokenkey".Equals(groupName))
          {
            tokenkeyFound = true;
          }

          if ("tokendata".Equals(groupName))
          {
            tokendataFound = true;
          }
        }
      }

      if (tokenkeyFound && tokendataFound)
      {
        _tokenExpressions.Add(tokenExpression);
      }
      else
      {
        throw new ArgumentException("tokenExpression must contain a group capture for 'tokenkey' and 'tokendata'.");
      }
    }

    private static List<Match> ParseTokenStrings(string inputText)
    {
      List<Match> result = new List<Match>();

      foreach (Regex expression in _tokenExpressions)
      {
        MatchCollection matches = expression.Matches(inputText);
        foreach (Match match in matches)
        {
          result.Add(match);
        }
      }

      return result;
    }

    /// <summary>
    /// Returns a list of all registered token handlers that can be used for debug information
    /// or diagnostics.
    /// </summary>
    /// <returns>List of registered ITokenHandler objects</returns>
    public static IList<ITokenHandler> GetRegisteredTokenHandlers()
    {
      List<ITokenHandler> _handlers = new List<ITokenHandler>();
      foreach (ITokenHandler handler in _tokenHandlers.Values)
      {
        _handlers.Add(handler);
      }
      return _handlers.AsReadOnly();
    }

    /// <summary>
    /// Clears all token handlers. This is used for unit tests only. This is not a threadsafe operation
    /// </summary>
    private static void ClearHandlers()
    {
      _tokenHandlers.Clear();
    }

    public static TokenEvaluationResult ReplaceTokens(string inputText, IProviderContainer container, out string resultText)
    {
      return ReplaceTokens(inputText, container, null, out resultText);
    }

    public static TokenEvaluationResult ReplaceTokens(string inputText, IProviderContainer container, ITokenEncoding tokenDataEncoding, out string resultText)
    {
      TokenEvaluationResult result = TokenEvaluationResult.Success;

      // First, we find all the tokens
      var foundTokens = ParseTokenStrings(inputText);
      if (foundTokens.Count == 0)
      {
        resultText = inputText;
      }
      else
      {
        StringBuilder workingText = new StringBuilder(inputText);

        List<string> errors;
        List<InProcessTokenGroup> groups = InitializeAndGroupTokens(foundTokens, tokenDataEncoding, out errors);

        if ((errors != null) && (errors.Count > 0))
        {
          result = TokenEvaluationResult.Errors;
        }

        foreach(InProcessTokenGroup group in groups)
        {
          TokenEvaluationResult evaluationResult = group.EvaluateTokens(container);
          if (evaluationResult == TokenEvaluationResult.Errors)
          {
            result = TokenEvaluationResult.Errors;
          }

          group.ExecuteReplacements(workingText, tokenDataEncoding);
        }

        resultText = workingText.ToString();
      }

      return result;
    }

    private static List<InProcessTokenGroup> InitializeAndGroupTokens(IEnumerable<Match> tokenMatches, ITokenEncoding tokenEncoding, out List<string> errors)
    {
      List<InProcessTokenGroup> result = new List<InProcessTokenGroup>();
      Dictionary<string, InProcessTokenGroup>  trackingGroups = new Dictionary<string, InProcessTokenGroup>(StringComparer.OrdinalIgnoreCase);
      errors = new List<string>();

      foreach (var tokenMatch in tokenMatches)
      {
        try
        {
          string matchValue = tokenMatch.Value;
          string tokenKey = tokenMatch.Groups[_TOKENKEY].Captures[0].Value;
          string tokenData = tokenMatch.Groups[_TOKENDATA].Captures[0].Value;

          if (tokenEncoding != null)
          {
            tokenData = tokenEncoding.DecodeTokenData(tokenData);
          }

          InProcessTokenGroup group;
          if (trackingGroups.TryGetValue(tokenKey, out group))
          {
            group.AddInProcessToken(tokenKey, tokenData, matchValue);
          }
          else
          {
            ITokenHandler tokenHandler;
            if (!_tokenHandlers.TryGetValue(tokenKey, out tokenHandler))
            {
              tokenHandler = null;
            }

            TokenHandlerStats tokenHandlerStats;
            _tokenHandlersStats.TryGetValue(tokenKey, out tokenHandlerStats);

            group = new InProcessTokenGroup(tokenHandler, tokenHandlerStats);
            trackingGroups[tokenKey] = group;
            result.Add(group);
            group.AddInProcessToken(tokenKey, tokenData, matchValue);
          }
        }
        catch (Exception ex)
        {
          errors.Add(tokenMatch.ToString() + ":" + ex.Message + Environment.NewLine + ex.StackTrace);
        }

      }

      return result;

    }
  }
}
