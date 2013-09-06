using System;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Web.RenderPipeline
{
  internal static class JavaScriptWebStashManager
  {
    private const string JAVASCRIPT_MATCH_NAME = "javascript";
    private const string END_BODY_TAG = "</body>";

    private static readonly Regex _javaScriptRegex = new Regex(@"<atlantis:javascriptwebstash>(?<javascript>.*?)</atlantis:javascriptwebstash>", RegexOptions.Compiled | RegexOptions.Singleline);

    internal static string ProcessScript(string content)
    {
      var finalContent = string.Empty;

      try
      {
        if (!string.IsNullOrEmpty(content))
        {
          StringBuilder scriptStash = new StringBuilder();

          finalContent = _javaScriptRegex.Replace(content, match => ProcessJavaScriptRegexMatch(match, scriptStash));

          finalContent = finalContent.Replace(END_BODY_TAG, scriptStash + END_BODY_TAG);
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("JavaScriptWebStashManager.ProcessScript", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return finalContent;
    }

    private static string ProcessJavaScriptRegexMatch(Match javaScriptMatch, StringBuilder scriptStash)
    {
      string javaScript = javaScriptMatch.Groups[JAVASCRIPT_MATCH_NAME].Captures[0].Value;

      if (!string.IsNullOrEmpty(javaScript))
      {
        scriptStash.AppendLine(javaScript);
      }

      return string.Empty;
    }
  }
}