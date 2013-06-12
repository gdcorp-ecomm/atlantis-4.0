using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Render.ContentInjection;
using Atlantis.Framework.Render.ContentInjection.RenderHandlers;
using Atlantis.Framework.Render.Pipeline.Interface;
using Atlantis.Framework.Web.RenderPipeline;

namespace Atlantis.Framework.Web.CDSContent
{
  public abstract class CDSContentPageControllerBase : RenderPipelineBasePage
  {
    private static readonly IList<string> _emptyUserControlPathList = new List<string>(0);
 
    private ICDSContentProvider _cdsContentProvider;
    protected ICDSContentProvider CdsContentProvider
    {
      get { return _cdsContentProvider ?? (_cdsContentProvider = ProviderContainer.Resolve<ICDSContentProvider>()); }
    }

    protected abstract string DocumentRoute { get; }

    protected abstract string ApplicationName { get; }

    protected abstract IProviderContainer ProviderContainer { get; }

    protected abstract IList<IRenderHandler> RenderHandlers { get; }

    protected virtual IList<string> HeadBeginUserControlPaths { get { return _emptyUserControlPathList; } }

    protected virtual IList<string> HeadEndUserControlPaths { get { return _emptyUserControlPathList; } }

    protected virtual IList<string> BodyBeginUserControlPaths { get { return _emptyUserControlPathList; } }

    protected virtual IList<string> BodyEndUserControlPaths { get { return _emptyUserControlPathList; } }

    private bool _useInjectionRenderHandler;
    protected virtual bool UseInjectionRenderHandler
    {
      get { return _useInjectionRenderHandler; }
    }

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
      _useInjectionRenderHandler = whitelistResult.UrlData.Style.Equals("f", StringComparison.OrdinalIgnoreCase);

      return HandleWhiteListResult(whitelistResult);
    }

    private void ConfigureRenderPipeline()
    {
      ConfigureContentInjectionRenderHandler();
      AddRenderHandlers(RenderHandlers);
    }

    private void ConfigureContentInjectionRenderHandler()
    {
      if (UseInjectionRenderHandler)
      {
        IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();

        IList<IContentInjectionItem> contentInjectionItems = new List<IContentInjectionItem>(4);

        if (HeadBeginUserControlPaths != null && HeadBeginUserControlPaths.Count > 0)
        {
          string headBeginMarkup = BuildInjectionItemMarkup(placeHolderProvider, HeadBeginUserControlPaths);
          contentInjectionItems.Add(new HtmlHeadBeginContentInjectionItem(headBeginMarkup));
        }

        if (HeadEndUserControlPaths != null && HeadEndUserControlPaths.Count > 0)
        {
          string headEndMarkup = BuildInjectionItemMarkup(placeHolderProvider, HeadEndUserControlPaths);
          contentInjectionItems.Add(new HtmlHeadEndContentInjectionItem(headEndMarkup));
        }

        if (BodyBeginUserControlPaths != null && BodyBeginUserControlPaths.Count > 0)
        {
          string bodyBeginMarkup = BuildInjectionItemMarkup(placeHolderProvider, BodyBeginUserControlPaths);
          contentInjectionItems.Add(new HtmlBodyBeginContentInjectionItem(bodyBeginMarkup));
        }

        if (BodyEndUserControlPaths != null && BodyEndUserControlPaths.Count > 0)
        {
          string bodyEndMarkup = BuildInjectionItemMarkup(placeHolderProvider, BodyEndUserControlPaths);
          contentInjectionItems.Add(new HtmlBodyEndContentInjectionItem(bodyEndMarkup));
        }

        if (contentInjectionItems.Count > 0)
        {
          IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);
          RenderHandlers.Insert(0, new ContentInjectionRenderHandler(context));
        }
      }
    }

    private string BuildInjectionItemMarkup(IPlaceHolderProvider placeHolderProvider, IList<string> userControlPaths)
    {
      StringBuilder markupBuilder = new StringBuilder();

      foreach (string userControlPath in userControlPaths)
      {
        markupBuilder.Append(placeHolderProvider.GetPlaceHolderMarkup(PlaceHolderTypes.UserControl, userControlPath, null));
      }

      return markupBuilder.ToString();
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

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      ConfigureRenderPipeline();
    }
  }
}
