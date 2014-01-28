using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.AppSettings.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Web.RenderPipeline
{
  public class StripWhiteSpaceRenderHandler : IRenderHandler
  {
    private const string APP_SETTING_KEY = "RenderPipeline_StripWhiteSpace";

    private static readonly Regex _lineBreaksRegex = new Regex(@"(\s*(\r)?\n\s*)+", RegexOptions.Compiled);

    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      if (StripWhiteSpaceTurnedOn(providerContainer))
      {
        string modifiedContent = _lineBreaksRegex.Replace(processRenderContent.Content, "\n").Trim();

        processRenderContent.OverWriteContent(modifiedContent);
      }
    }

    private bool StripWhiteSpaceTurnedOn(IProviderContainer providerContainer)
    {
      bool turnedOn = true;
      IAppSettingsProvider appSettings;
      if (providerContainer.TryResolve<IAppSettingsProvider>(out appSettings))
      {
        string value = appSettings.GetAppSetting(APP_SETTING_KEY);
        if (value != null)
        {
          turnedOn = value.Equals("true", System.StringComparison.OrdinalIgnoreCase);
        }
      }

      return turnedOn;
    }
  }
}
