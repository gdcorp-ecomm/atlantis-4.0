using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Tokens.Interface
{
  [Obsolete("TokenManger exists only for backwards compatability, use ITokenProvider instead")]
  public static class TokenManager
  {
    public static void AutoRegisterTokenHandlers()
    {
      AutoRegisterTokenHandlers(null);
    }

    public static void AutoRegisterTokenHandlers(params Assembly[] additionalAssemblies)
    {
      TokenProvider.AutoRegisterTokenHandlers(additionalAssemblies);
    }

    public static void RegisterTokenHandler(ITokenHandler tokenHandler)
    {
      TokenProvider.RegisterTokenHandler(tokenHandler);
    }

    public static void RegisterTokenExpression(Regex tokenExpression)
    {
      TokenProvider.RegisterTokenExpression(tokenExpression);
    }

    public static IList<ITokenHandler> GetRegisteredTokenHandlers()
    {
      return TokenProvider.GetRegisteredTokenHandlers();
    }

    public static void ReplaceTokens(string inputText, IProviderContainer container, out string resultText)
    {
      ReplaceTokens(inputText, container, null, out resultText);
    }

    public static void ReplaceTokens(string inputText, IProviderContainer container, ITokenEncoding tokenDataEncoding, out string resultText)
    {
      var tokenProvider = container.Resolve<ITokenProvider>();
      tokenProvider.ReplaceTokens(inputText, tokenDataEncoding, out resultText);
    }
  }
}
