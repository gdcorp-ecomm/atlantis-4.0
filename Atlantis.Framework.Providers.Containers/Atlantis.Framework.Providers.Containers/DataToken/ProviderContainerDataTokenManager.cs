using System;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Containers.DataToken
{
  public static class ProviderContainerDataTokenManager
  {
    private const string DATATOKEN_MATCH_KEY = "datakey";

    private static readonly Regex _dataTokenRegex = new Regex(@"\[@D\[(?<datakey>[a-zA-z0-9\.\-]*?)\]@D\]", RegexOptions.Compiled | RegexOptions.Singleline);

    public static string ReplaceDataTokens(string content, IProviderContainer providerContainer)
    {
      var finalContent = string.Empty;

      try
      {
        if (!string.IsNullOrEmpty(content))
        {
          finalContent = _dataTokenRegex.Replace(content, match => ProcessDataTokenMatch(match, providerContainer));
        }
      }
      catch (Exception ex)
      {
        LogError(ex.Message, "ProviderContainerDataTokenManager.ReplaceDataTokens()", string.Empty, providerContainer);
      }

      return finalContent;
    }

    private static string ProcessDataTokenMatch(Match dataTokenMatch, IProviderContainer providerContainer)
    {
      string replaceValue;

      string matchValue = dataTokenMatch.Value;
      string dataTokenKey = dataTokenMatch.Groups[DATATOKEN_MATCH_KEY].Captures[0].Value;

      var dataTokenValue = providerContainer.GetData<string>(dataTokenKey, null);
      if (dataTokenValue != null)
      {
        replaceValue = dataTokenValue;
      }
      else
      {
        replaceValue = string.Empty;

        LogError("IProviderContainer data value not present.", "ProviderContainerDataTokenManager.ProcessDataTokenMatch()", matchValue, providerContainer);
      }

      return replaceValue;
    }

    private static void LogError(string errorMessage, string sourceFunction, string key, IProviderContainer providerContainer)
    {
      IDebugContext debugContext;
      if (providerContainer.TryResolve(out debugContext))
      {
        debugContext.LogDebugTrackingData("IProviderContainer Data Errors - " + key, errorMessage);
      }

      Engine.Engine.LogAtlantisException(new AtlantisException(sourceFunction, 0, errorMessage, key));
    }
  }
}
