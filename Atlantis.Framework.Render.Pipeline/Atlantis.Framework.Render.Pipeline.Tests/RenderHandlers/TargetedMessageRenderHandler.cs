using System.Text;
using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.Render.Pipeline.Tests.RenderHandlers
{
  public class TargetedMessageRenderHandler : IRenderHandler
  {
    private static readonly Regex _targetedMessageRegex = new Regex(@"\[@TargetedMessage\[(?<propertyname>.*?)\]@TargetedMessage\]", RegexOptions.Singleline | RegexOptions.Compiled);

    public void ProcessContent(IRenderContent renderContent, IProviderContainer providerContainer)
    {
      MatchCollection matches = _targetedMessageRegex.Matches(renderContent.Content);
      StringBuilder targetedMessageMarkupBuilder = new StringBuilder(renderContent.Content);

      foreach (Match match in matches)
      {
        string matchValue = match.Value;
        string propertyName = match.Groups["propertyname"].Captures[0].Value;

        string replaceValue = string.Empty;

        switch (propertyName)
        {
          case "message":
            replaceValue = "Targeted Message Here!!!!";
            break;
          case "imageUrl":
            replaceValue = "http://img1.wsimg.com/fos/lp/hosting/intro-v2.jpg";
            break;
        }

        targetedMessageMarkupBuilder = targetedMessageMarkupBuilder.Replace(matchValue, replaceValue);
      }

      renderContent.Content = targetedMessageMarkupBuilder.ToString();
    }
  }
}
