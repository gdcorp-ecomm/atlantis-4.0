using System;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest.controls
{
  public partial class banner2 : UserControl
  {
    private string _preInitText = "ERROR: Page_Init NOT FIRED!!!!";
    protected string PreInitText
    {
      get { return _preInitText; }
      private set { _preInitText = value; }
    }

    private string _pageLoadText = "ERROR: Page_Load NOT FIRED!!!!";
    protected string PageLoadText
    {
      get { return _pageLoadText; }
      private set { _pageLoadText = value; }
    }

    private string _preRenderText = "ERROR: Page_PreRender NOT FIRED!!!!";
    protected string PreRenderText
    {
      get { return _preRenderText; }
      private set { _preRenderText = value; }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
      PreInitText = "Page_Init fired!!!!";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      PageLoadText = "Page_Load fired!!!!";
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
      PreRenderText = "Page_PreRender fired!!!!";
    }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.WriteLine("<div>{0}</div>", PreInitText);
      writer.WriteLine("<div>{0}</div>", PageLoadText);
      writer.WriteLine("<div>{0}</div>", PreRenderText);
    }
  }
}