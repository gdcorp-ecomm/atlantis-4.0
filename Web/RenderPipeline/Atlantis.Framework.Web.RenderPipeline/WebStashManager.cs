using System;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Web.RenderPipeline
{
  internal static class WebStashManager
  {
    private const string WEBSTASH_MATCH_NAME = "stashcontent";
    private const string TYPE_MATCH_NAME = "type";
    private const string END_HEAD_TAG = "</head>";
    private const string END_BODY_TAG = "</body>";
    private const string STASH_TYPE_CSS = "css";
    private const string STASH_TYPE_JS = "js";

    private static readonly Regex _webStashRegex = new Regex(@"<atlantis:webstash\stype=""(?<type>[a-zA-Z]+?)"">(?<stashcontent>.*?)</atlantis:webstash>", RegexOptions.Compiled | RegexOptions.Singleline);

    internal static string ProcessScript(string content)
    {
      var stashContent = string.Empty;
      var stashType = string.Empty;

      try
      {
        if (!string.IsNullOrEmpty(content))
        {
          StringBuilder webStash = new StringBuilder();

          stashContent = _webStashRegex.Replace(content, match => ProcessWebStashRegexMatch(match, webStash, out stashType));

          if (stashType.ToLowerInvariant() == STASH_TYPE_JS)
          {
            stashContent = stashContent.Replace(END_BODY_TAG, webStash + END_BODY_TAG);
          }
          else if (stashType.ToLowerInvariant() == STASH_TYPE_CSS)
          {
            stashContent = stashContent.Replace(END_HEAD_TAG, webStash + END_HEAD_TAG);
          }
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("WebStashManager.ProcessScript", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return stashContent;
    }

    private static string ProcessWebStashRegexMatch(Match webStashMatch, StringBuilder webStash, out string stashType)
    {
      string stashContent = webStashMatch.Groups[WEBSTASH_MATCH_NAME].Captures[0].Value;
      stashType = webStashMatch.Groups[TYPE_MATCH_NAME].Captures[0].Value;

      if (!string.IsNullOrEmpty(stashContent))
      {
        webStash.AppendLine(stashContent);
      }

      return string.Empty;
    }
  }
}