using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.CDS.Interface;

namespace Atlantis.Framework.Providers.CDSContent.Interface
{
  public interface ICDSContentProvider
  {
    IWhitelistResult CheckWhiteList(string appName, string relativePath);

    IRedirectResult CheckRedirectRules(string appName, string relativePath);

    IRenderContent GetContent(string appName, string relativePath);
  }
}
