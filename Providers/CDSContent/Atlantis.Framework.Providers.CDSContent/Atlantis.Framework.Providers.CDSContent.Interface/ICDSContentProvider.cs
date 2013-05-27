using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.CDS.Interface;

namespace Atlantis.Framework.Providers.CDSContent.Interface
{
  public interface ICDSContentProvider
  {
    IWhitelistResult CheckWhiteList(string appName, string relativePath);
    IRedirectResult CheckRedirectRules(string appName, string relativePath);
    string GetContentPath(string appName, string relativePath);
    string RenderContent(string contentPath, RenderPipelineManager renderPipelineManager);
  }
}
