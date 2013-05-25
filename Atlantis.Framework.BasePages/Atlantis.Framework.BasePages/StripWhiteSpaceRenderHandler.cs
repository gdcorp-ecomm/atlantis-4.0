using System.Text.RegularExpressions;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;

namespace Atlantis.Framework.BasePages
{
  internal class StripWhiteSpaceRenderHandler : IRenderHandler
  {
    private static readonly Regex _lineBreaksRegex = new Regex(@"(\s*(\r)?\n\s*)+", RegexOptions.Compiled);

    public void ProcessContent(IProcessedRenderContent processRenderContent, IProviderContainer providerContainer)
    {
      string modifiedContent = _lineBreaksRegex.Replace(processRenderContent.Content, "\n").Trim();

      processRenderContent.OverWriteContent(modifiedContent);
    }
  }
}
