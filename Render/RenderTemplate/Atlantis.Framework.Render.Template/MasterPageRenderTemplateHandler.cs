using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.Template.Interface;

namespace Atlantis.Framework.Render.Template
{
  internal class MasterPageRenderTemplateHandler : IRenderTemplateHandler
  {
    private readonly IRenderTemplateContent _renderTemplateContent;

    internal MasterPageRenderTemplateHandler(IRenderTemplateContent renderTemplateContent)
    {
      _renderTemplateContent = renderTemplateContent;
    }

    private Page GetCurrentPage()
    {
      if (HttpContext.Current == null || HttpContext.Current.Handler == null)
      {
        throw new Exception("Masterpage render templates can only be used in a web context.");
      }

      Page page = HttpContext.Current.Handler as Page;
      if (page == null)
      {
        throw new Exception("Masterpage render templates can only be used in a web page context.");
      }

      return page;
    }

    private void SetMasterPage(Page currentPage)
    {
      currentPage.MasterPageFile = _renderTemplateContent.RenderTemplateData.Location;
      if (currentPage.Master == null)
      {
        throw new Exception("Unable to load Master Page, make sure you are processing your Render Template in the PreInit page event. Location: " + _renderTemplateContent.RenderTemplateData.Location);
      }
    }

    private void ProcessTemplateSections(Page currentPage)
    {
      if (currentPage.Master != null)
      {
        foreach (Control control in currentPage.Master.Controls)
        {
          ContentPlaceHolder contentPlaceHolder = control as ContentPlaceHolder;
          IRenderTemplateSection renderTemplateSection;

          if (contentPlaceHolder != null &&
              _renderTemplateContent.TryGetSection(contentPlaceHolder.ID, out renderTemplateSection))
          {
            contentPlaceHolder.Controls.Add(new LiteralControl(renderTemplateSection.Content));
          }
        }
      }
    }

    public void ProcessTemplate()
    {
      try
      {
        Page currentPage = GetCurrentPage();
        SetMasterPage(currentPage);
        ProcessTemplateSections(currentPage);
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("ProcessTemplate()",
                                                     "0",
                                                     ex.Message + " | " + ex.StackTrace,
                                                     "Render Template Type: " + _renderTemplateContent.RenderTemplateData.Type,
                                                     null,
                                                     null); 
        Engine.Engine.LogAtlantisException(aex);
      }
    }
  }
}