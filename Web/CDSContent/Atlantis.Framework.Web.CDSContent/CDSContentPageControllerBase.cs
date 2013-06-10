using System;
using System.Web.UI;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Web.RenderPipeline;

namespace Atlantis.Framework.Web.CDSContent
{
  public abstract class CDSContentPageControllerBase : RenderPipelineBasePage
  {
    private ICDSContentProvider _cdsContentProvider;
    protected ICDSContentProvider CdsContentProvider
    {
      get { return _cdsContentProvider ?? (_cdsContentProvider = ProviderContainer.Resolve<ICDSContentProvider>()); }
    }

    protected abstract string DocumentRoute { get; }

    protected abstract string ApplicationName { get; }

    protected abstract IProviderContainer ProviderContainer { get; }

    private void ProcessContent()
    {
      IRenderContent renderContent = CdsContentProvider.GetContent(ApplicationName, DocumentRoute);
      Controls.Add(new LiteralControl(renderContent.Content));
    }

    private bool RedirectRequest()
    {
      bool redirectRequest = false;

      IRedirectResult redirectResult = CdsContentProvider.CheckRedirectRules(ApplicationName, DocumentRoute);
      if (redirectResult.RedirectRequired)
      {
        redirectRequest = true;
        switch (redirectResult.RedirectData.Type)
        {
          case "301":
            Response.Redirect(redirectResult.RedirectData.Location, true);
            break;
          case "302":
            Response.RedirectPermanent(redirectResult.RedirectData.Location, true);
            break;
        }
      }

      return redirectRequest;
    }

    private bool WhiteListCheck()
    {
      IWhitelistResult whitelistResult = CdsContentProvider.CheckWhiteList(ApplicationName, DocumentRoute);
      return HandleWhiteListResult(whitelistResult);
    }

    protected virtual bool HandleWhiteListResult(IWhitelistResult whiteListResult)
    {
      bool success = true;

      if (!whiteListResult.Exists)
      {
        success = false;
        HandleDocumentNotFound();
      }

      return success;
    }

    protected abstract void HandleDocumentNotFound();

    protected override void OnPreInit(EventArgs e)
    {
      base.OnPreInit(e);

      if (WhiteListCheck() && !RedirectRequest())
      {
        ProcessContent();  
      }
    }
  }
}
