using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.RenderPipeline.Interface;
using Atlantis.Framework.Render.ContentInjection;
using Atlantis.Framework.Render.ContentInjection.RenderHandlers;
using Atlantis.Framework.Web.RenderPipeline;
using Atlantis.Framework.Providers.Containers;
using System.IO;

namespace Atlantis.Framework.Web.CDSContent
{
  public abstract class CDSContentPageControllerBase : RenderPipelineBasePage
  {
    private const string BodyEndTag = @"</body>";
    private static readonly IList<IPlaceHolder> _emptyPlaceHolderList = new List<IPlaceHolder>(0);
    private IWhitelistResult _whitelistResult;

    private readonly Control _debugInfoControl = null;

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

    protected virtual Control DebugInfoControl { get { return _debugInfoControl; } }

    private ISiteContext _siteContext;
    protected ISiteContext SiteContext
    {
      get { return _siteContext ?? (_siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>()); }
    }

    private bool _useInjectionRenderHandler = true;
    protected bool UseInjectionRenderHandler
    {
      get { return _useInjectionRenderHandler; }
      set { _useInjectionRenderHandler = value; }
    }

    private void SetNoCacheHeaders()
    {
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.Cache.SetNoStore();
      Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
    }

    private void ProcessContent()
    {
      IRenderContent renderContent = CdsContentProvider.GetContent(ApplicationName, DocumentRoute);

      if (string.IsNullOrEmpty(renderContent.Content))
      {
        HandleDocumentNotFound();
      }
      else
      {
        Controls.Add(new LiteralControl(renderContent.Content)); 
      }
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
            Response.RedirectPermanent(redirectResult.RedirectData.Location, true);
            break;
          case "302":
            Response.Redirect(redirectResult.RedirectData.Location, true);
            break;
        }
      }

      return redirectRequest;
    }

    private bool WhiteListCheck()
    {
      _whitelistResult = CdsContentProvider.CheckWhiteList(ApplicationName, DocumentRoute);

      return HandleWhiteListResult(_whitelistResult);
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

      SetNoCacheHeaders();

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

    protected override void Render(HtmlTextWriter writer)
    {
      if (UseInjectionRenderHandler && SiteContext.IsRequestInternal && DebugInfoControl != null)
      {
        AppendDebugInfoControl(writer);
      }
      else
      {
        base.Render(writer);
      }
    }

    private void AppendDebugInfoControl(HtmlTextWriter writer)
    {
      using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new StringWriter()))
      {
        base.Render(htmlwriter);
        StringBuilder contentBuilder = new StringBuilder(htmlwriter.InnerWriter.ToString());
        string html = RenderControl(DebugInfoControl);
        if (!string.IsNullOrEmpty(html))
        {
          contentBuilder.Replace(BodyEndTag, html + BodyEndTag);
        }
        writer.Write(contentBuilder.ToString());
      }
    }

    private string RenderControl(Control ctrl)
    {
      string html = string.Empty;

      if (ctrl != null && ctrl.Visible)
      {
        StringBuilder htmlStringBuilder = new StringBuilder();

        using (StringWriter stringWriter = new StringWriter(htmlStringBuilder))
        {
          using (HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter))
          {
            ctrl.RenderControl(htmlTextWriter);
            html = htmlStringBuilder.ToString();
          }
        }
      }

      return html;
    }
  }
}
