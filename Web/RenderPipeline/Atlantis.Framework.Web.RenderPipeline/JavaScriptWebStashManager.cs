using System;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Web.RenderPipeline
{
  internal static class JavaScriptWebStashManager
  {
    private const string JAVASCRIPT_MATCH_NAME = "javascript";

    private static readonly Regex _javaScriptRegex = new Regex(@"<atlantis:javascriptwebstash>(?<javascript>.*?)</atlantis:javascriptwebstash>", RegexOptions.Compiled | RegexOptions.Singleline);
    private const string END_BODY_TAG = "</body>";

    internal static string ProcessScript(string content)
    {
      var originalContent = content ?? string.Empty;
      var finalContent = string.Empty;

      try
      {
        if (originalContent != string.Empty)
        {
          finalContent = CheckJavaScriptRegexMatches(originalContent);
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("JavaScriptWebStashManager.ProcessScript", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return finalContent;
    }

    private static string CheckJavaScriptRegexMatches(string originalContent)
    {
      var finalContent = originalContent;

      MatchCollection javascriptMatches = _javaScriptRegex.Matches(originalContent);

      if (javascriptMatches.Count > 0)
      {
        finalContent = ProcessJavaScriptRegexMatches(javascriptMatches, originalContent);
      }

      return finalContent;
    }

    private static string ProcessJavaScriptRegexMatches(MatchCollection javaScriptMatches, string originalContent)
    {
      var finalContent = new StringBuilder(originalContent);

      foreach (Match javascriptMatch in javaScriptMatches)
      {
        string matchValue = javascriptMatch.Value;
        string javaScript = javascriptMatch.Groups[JAVASCRIPT_MATCH_NAME].Captures[0].Value;

        if (!string.IsNullOrEmpty(javaScript))
        {
          finalContent.Replace(END_BODY_TAG, javaScript + END_BODY_TAG);
          finalContent.Replace(matchValue, string.Empty);
        }
      }

      return finalContent.ToString();
    }
  }
}
