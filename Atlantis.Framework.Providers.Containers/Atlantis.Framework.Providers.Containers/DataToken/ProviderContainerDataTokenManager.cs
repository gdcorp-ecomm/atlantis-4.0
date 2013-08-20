using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Providers.Containers.DataToken
{
  internal static class ProviderContainerDataTokenManager
  {
    private const string DATATOKEN_MATCH_KEY = "datakey";

    private static readonly Regex _dataTokenRegex = new Regex(@"\[@D\[(?<datakey>[a-zA-z0-9]*?)\]@D\]", RegexOptions.Compiled | RegexOptions.Singleline);

    internal static string ReplaceDataTokens(string content, IProviderContainer providerContainer)
    {
      var originalContent = content ?? string.Empty;
      var finalContent = string.Empty;

      try
      {
        if (originalContent != string.Empty)
        {
          finalContent = ProcessDataTokenMatches(originalContent, providerContainer);
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("ProviderContainerDataTokenManager.ReplaceDataTokens", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return finalContent;
    }

    private static string ProcessDataTokenMatches(string originalContent, IProviderContainer providerContainer)
    {
      var finalContent = originalContent;

      MatchCollection dataTokenMatches = _dataTokenRegex.Matches(originalContent);

      if (dataTokenMatches.Count > 0)
      {
        var debugContextErrors = new Collection<string>();

        finalContent = ProcessDataTokens(dataTokenMatches, originalContent, providerContainer, debugContextErrors);

        LogDebugContextData(providerContainer, debugContextErrors);
      }

      return finalContent;
    }

    private static string ProcessDataTokens(MatchCollection dataTokenMatches, string originalContent, 
                                            IProviderContainer providerContainer, ICollection<string> errors)
    {
      var finalContent = new StringBuilder(originalContent);

      foreach (Match dataTokenMatch in dataTokenMatches)
      {
        string matchValue = dataTokenMatch.Value;
        string dataTokenKey = dataTokenMatch.Groups[DATATOKEN_MATCH_KEY].Captures[0].Value;

        var dataTokenValue = providerContainer.GetData<string>(dataTokenKey, null);
        if (dataTokenValue != null)
        {
          finalContent.Replace(matchValue, dataTokenValue);
        }
        else
        {
          errors.Add("Could not get IProviderContainer data value for Key: " + dataTokenKey);
          finalContent.Replace(matchValue, string.Empty);
        }
      }

      return finalContent.ToString();
    }

    private static void LogDebugContextData(IProviderContainer providerContainer, ICollection<string> errors)
    {
      IDebugContext debugContext;
      if (errors.Count > 0 && providerContainer.TryResolve(out debugContext))
      {
        var dataTokenDebugBuilder = new StringBuilder();
        foreach (string debugContextError in errors)
        {
          dataTokenDebugBuilder.AppendLine(debugContextError);
        }

        debugContext.LogDebugTrackingData("DataTokenManager Errors", dataTokenDebugBuilder.ToString());
      }
    }
  }
}
