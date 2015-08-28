using System.Xml;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Tokens.Interface
{
  public static class TokenManager
  {
    private const string TOKEN_KEY_GROUP = "tokenkey";
    private const string TOKEN_DATA_GROUP = "tokendata";
    private const string DEFAULT_TOKEN_PATTERN = @"\[@T\[(?<tokenkey>[a-zA-Z]*?):(?<tokendata>.*?)\]@T\]";

    private static readonly Dictionary<string, ITokenHandler> _tokenHandlers;
    private static readonly Dictionary<string, TokenHandlerStats> _tokenHandlersStats;
    private static readonly List<Regex> _tokenExpressions;

    public delegate void TokenReplacedEventHandler(string tokenKey, string tokenData, TokenEvaluationResult result);

    public static event TokenReplacedEventHandler TokenReplaced;

    static TokenManager()
    {
      _tokenHandlers = new Dictionary<string, ITokenHandler>(StringComparer.OrdinalIgnoreCase);
      _tokenHandlersStats = new Dictionary<string, TokenHandlerStats>(StringComparer.OrdinalIgnoreCase);

      _tokenExpressions = new List<Regex>();

      Regex defaultTokenEx = new Regex(DEFAULT_TOKEN_PATTERN, RegexOptions.Singleline | RegexOptions.Compiled);
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

      if (tokenkeyFound && tokendataFound)
      {
        _tokenExpressions.Add(tokenExpression);
      }
      else
      {
        throw new ArgumentException("tokenExpression must contain a group capture for 'tokenkey' and 'tokendata'.");
      }
    }

    internal static void ClearHandlers()
    {
       _tokenHandlers.Clear();
    }

    internal static List<Match> ParseTokenStrings(string inputText)
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
      List<ITokenHandler> handlers = new List<ITokenHandler>();

      foreach (ITokenHandler handler in _tokenHandlers.Values)
      {
        handlers.Add(handler);
      }

      return handlers.AsReadOnly();
    }

    public static void ReplaceTokens(string inputText, IProviderContainer container, out string resultText)
    {
      ReplaceTokens(inputText, container, null, out resultText);
    }

    public static void ReplaceTokens(string inputText, IProviderContainer container, ITokenEncoding tokenDataEncoding, out string resultText)
    {
      resultText = inputText;

      foreach (Regex expression in _tokenExpressions)
      {
        resultText = expression.Replace(resultText, match => ProcessTokenMatch(match, container, tokenDataEncoding));
      }
    }

    private static string ProcessTokenMatch(Match tokenMatch, IProviderContainer container, ITokenEncoding tokenEncoding)
    {
      string replacementValue;

      try
      {
        string matchValue = tokenMatch.Value;
        string tokenKey = tokenMatch.Groups[TOKEN_KEY_GROUP].Captures[0].Value;
        string tokenData = tokenMatch.Groups[TOKEN_DATA_GROUP].Captures[0].Value;

        if (tokenEncoding != null)
        {
          tokenData = tokenEncoding.DecodeTokenData(tokenData);
        }

        ITokenHandler tokenHandler;
        _tokenHandlers.TryGetValue(tokenKey, out tokenHandler);

        TokenHandlerStats tokenHandlerStats;
        _tokenHandlersStats.TryGetValue(tokenKey, out tokenHandlerStats);

        TokenHandlerManager tokenHandlerManager = new TokenHandlerManager(tokenKey, tokenData, matchValue, tokenHandler, tokenHandlerStats);

        TokenEvaluationResult result = tokenHandlerManager.EvaluateToken(container);
        if (result == TokenEvaluationResult.Errors)
        {
          replacementValue = string.Empty;
        }
        else
        {
          replacementValue = tokenHandlerManager.RenderToken(tokenEncoding);
        }

        BubbleTokenReplacedEvent(tokenKey, tokenData, result);
      }
      catch (XmlException)
      {
        replacementValue = string.Empty;

        BubbleTokenReplacedEvent(tokenMatch.Groups[TOKEN_KEY_GROUP].Captures[0].Value, 
                                 tokenMatch.Groups[TOKEN_DATA_GROUP].Captures[0].Value, 
                                 TokenEvaluationResult.Errors);

        ErrorLogHelper.LogErrors( tokenMatch.Groups[TOKEN_KEY_GROUP].Captures[0].Value,
                                  "Malformed token data in XmlToken: "+tokenMatch.Value,
                                  "TokenHandlerManager.EvaluateToken()",
                                  tokenMatch.Groups[TOKEN_DATA_GROUP].Captures[0].Value,
                                  container);
      }
      catch (Exception ex)
      {
        replacementValue = string.Empty;

        BubbleTokenReplacedEvent(tokenMatch.Groups[TOKEN_KEY_GROUP].Captures[0].Value,
                                 tokenMatch.Groups[TOKEN_DATA_GROUP].Captures[0].Value,
                                 TokenEvaluationResult.Errors);

        ErrorLogHelper.LogErrors("TokenManager.ProcessTokenMatch." + tokenMatch.Value,
                                  ex.Message,
                                  "TokenHandlerManager.EvaluateToken()",
                                  tokenMatch.Value,
                                  container);
      }

      return replacementValue;
    }

    private static void BubbleTokenReplacedEvent(string tokenKey, string tokenData, TokenEvaluationResult result)
    {
      if (TokenReplaced != null)
      {
        TokenReplaced(tokenKey, tokenData, result);
      }
    }
  }
}