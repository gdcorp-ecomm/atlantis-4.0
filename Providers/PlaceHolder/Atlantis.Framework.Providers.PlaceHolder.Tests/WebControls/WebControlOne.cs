using System;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls
{
  public class WebControlOne : Control
  {
    private string _textSetOnInit = "ERROR: Init event DID NOT fire.";
    protected string TextSetOnInit
    {
      get { return _textSetOnInit; }
    }

    private string _textSetOnLoad = "ERROR: Load event DID NOT fire.";
    protected string TextSetOnLoad
    {
      get { return _textSetOnLoad; }
    }

    private string _textSetOnPreRender = "ERROR: PreRender event DID NOT fire.";
    protected string TextSetOnPreRender
    {
      get { return _textSetOnPreRender; }
    }

    public string Title { get; set; }

    public string Text { get; set; }

    protected override void OnInit(EventArgs e)
    {
      _textSetOnInit = "Init event fired!!!";
    }

    protected override void OnLoad(EventArgs e)
    {
      _textSetOnLoad = "Load event fired!!!";
    }

    protected override void OnPreRender(EventArgs e)
    {
      _textSetOnPreRender = "PreRender event fired!!!";
    }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.Write("Web Control One!");
      writer.Write(Title ?? string.Empty);
      writer.Write(Text ?? string.Empty);
      writer.Write(TextSetOnInit);
      writer.Write(TextSetOnLoad);
      writer.Write(TextSetOnPreRender);
    }
  }
}
