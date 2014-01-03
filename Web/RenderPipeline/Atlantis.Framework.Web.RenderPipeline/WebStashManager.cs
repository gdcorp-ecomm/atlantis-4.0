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

    internal static string ProcessStash(string content)
    {
      var stashContent = string.Empty;

      try
      {
        if (!string.IsNullOrEmpty(content))
        {
          StringBuilder jsWebStash = new StringBuilder();
          StringBuilder cssWebStash = new StringBuilder();

          stashContent = _webStashRegex.Replace(content, match => ProcessWebStashRegexMatch(match, jsWebStash, cssWebStash));

          if (jsWebStash.Length > 0)
          {
            stashContent = stashContent.Replace(END_BODY_TAG, jsWebStash + END_BODY_TAG);
          }
          if (cssWebStash.Length > 0)
          {
            stashContent = stashContent.Replace(END_HEAD_TAG, cssWebStash + END_HEAD_TAG);
          }
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("WebStashManager.ProcessStash", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return stashContent;
    }

    private static string ProcessWebStashRegexMatch(Match webStashMatch, StringBuilder jsWebStash, StringBuilder cssWebStash)
    {
      string stashContent = webStashMatch.Groups[WEBSTASH_MATCH_NAME].Captures[0].Value;
      string stashType = webStashMatch.Groups[TYPE_MATCH_NAME].Captures[0].Value;

      if (!string.IsNullOrEmpty(stashContent))
      {
        if (stashType.ToLowerInvariant() == STASH_TYPE_JS)
        {
          jsWebStash.AppendLine(stashContent);
        }
        else if (stashType.ToLowerInvariant() == STASH_TYPE_CSS)
        {
          cssWebStash.AppendLine(stashContent);
        }
      }

      return string.Empty;
    }
  }
}