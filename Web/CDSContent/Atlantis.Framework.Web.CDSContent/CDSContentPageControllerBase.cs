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
    private static readonly IList<IPlaceHolder> _emptyPlaceHolderList = new List<IPlaceHolder>(0);
 
    private ICDSContentProvider _cdsContentProvider;
    protected ICDSContentProvider CdsContentProvider
    {
      get { return _cdsContentProvider ?? (_cdsContentProvider = ProviderContainer.Resolve<ICDSContentProvider>()); }
    }

    protected abstract string DocumentRoute { get; }

    protected abstract string ApplicationName { get; }

    protected abstract IProviderContainer ProviderContainer { get; }

    protected abstract IList<IRenderHandler> RenderHandlers { get; }

    protected virtual IList<IPlaceHolder> HeadBeginPlaceHolders { get { return _emptyPlaceHolderList; } }

    protected virtual IList<IPlaceHolder> HeadEndPlaceHolders { get { return _emptyPlaceHolderList; } }

    protected virtual IList<IPlaceHolder> BodyBeginPlaceHolders { get { return _emptyPlaceHolderList; } }

    protected virtual IList<IPlaceHolder> BodyEndPlaceHolders { get { return _emptyPlaceHolderList; } }

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
        IList<IContentInjectionItem> contentInjectionItems = new List<IContentInjectionItem>(4);

        if (HeadBeginPlaceHolders != null && HeadBeginPlaceHolders.Count > 0)
        {
          string headBeginMarkup = BuildInjectionItemMarkup(HeadBeginPlaceHolders);
          contentInjectionItems.Add(new HtmlHeadBeginContentInjectionItem(headBeginMarkup));
        }

        if (HeadEndPlaceHolders != null && HeadEndPlaceHolders.Count > 0)
        {
          string headEndMarkup = BuildInjectionItemMarkup(HeadEndPlaceHolders);
          contentInjectionItems.Add(new HtmlHeadEndContentInjectionItem(headEndMarkup));
        }

        if (BodyBeginPlaceHolders != null && BodyBeginPlaceHolders.Count > 0)
        {
          string bodyBeginMarkup = BuildInjectionItemMarkup(BodyBeginPlaceHolders);
          contentInjectionItems.Add(new HtmlBodyBeginContentInjectionItem(bodyBeginMarkup));
        }

        if (BodyEndPlaceHolders != null && BodyEndPlaceHolders.Count > 0)
        {
          string bodyEndMarkup = BuildInjectionItemMarkup(BodyEndPlaceHolders);
          contentInjectionItems.Add(new HtmlBodyEndContentInjectionItem(bodyEndMarkup));
        }

        if (contentInjectionItems.Count > 0)
        {
          IContentInjectionContext context = new ContentInjectionContext(contentInjectionItems);
          RenderHandlers.Insert(0, new ContentInjectionRenderHandler(context));
        }
      }
    }

    private string BuildInjectionItemMarkup(IList<IPlaceHolder> placeHolderList)
    {
      StringBuilder markupBuilder = new StringBuilder();

      foreach (IPlaceHolder placeHolder in placeHolderList)
      {
        markupBuilder.Append(placeHolder.ToMarkup());
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
