using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using System.Text.RegularExpressions;

namespace Atlantis.Framework.Web.RenderPipeline
{
  public class StripWhiteSpaceRenderHandler : IRenderHandler
  {
    private static readonly Regex _lineBreaksRegex = new Regex(@"(\s*(\r)?\n\s*)+", RegexOptions.Compiled);

    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent = _lineBreaksRegex.Replace(processRenderContent.Content, "\n").Trim();

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
