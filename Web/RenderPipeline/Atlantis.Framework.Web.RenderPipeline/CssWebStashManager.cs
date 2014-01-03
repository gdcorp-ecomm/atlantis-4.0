using System;
using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Web.RenderPipeline
{
  internal static class CssWebStashManager
  {
    private const string CSS_MATCH_NAME = "css";
    private const string END_HEAD_TAG = "</head>";

    private static readonly Regex _cssRegex = new Regex(@"<atlantis:csswebstash>(?<css>.*?)</atlantis:csswebstash>", RegexOptions.Compiled | RegexOptions.Singleline);

    internal static string ProcessCss(string content)
    {
      var finalContent = string.Empty;

      try
      {
        if (!string.IsNullOrEmpty(content))
        {
          StringBuilder cssStash = new StringBuilder();

          finalContent = _cssRegex.Replace(content, match => ProcessCssRegexMatch(match, cssStash));

          finalContent = finalContent.Replace(END_HEAD_TAG, cssStash + END_HEAD_TAG);
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException("CssWebStashManager.ProcessCss", "0", ex.Message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return finalContent;
    }

    private static string ProcessCssRegexMatch(Match cssMatch, StringBuilder cssStash)
    {
      string css = cssMatch.Groups[CSS_MATCH_NAME].Captures[0].Value;

      if (!string.IsNullOrEmpty(css))
      {
        cssStash.AppendLine(css);
      }

      return string.Empty;
    }
  }
}