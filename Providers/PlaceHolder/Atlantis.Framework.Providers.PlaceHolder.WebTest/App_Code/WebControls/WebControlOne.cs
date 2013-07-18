using System;
using System.Web.UI;

namespace WebControls
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

    public string Text { get; set; }

    protected override void OnInit(EventArgs e)
    {
      _textSetOnInit = "Init event Fired!!!";
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
      writer.WriteLine("<h1>{0}</h1>", !string.IsNullOrEmpty(Text) ? Text : "ERROR: \"Text\" parameter not found.");
      writer.WriteLine("<div>{0}</div>", TextSetOnInit);
      writer.WriteLine("<div>{0}</div>", TextSetOnLoad);
      writer.WriteLine("<div>{0}</div>", TextSetOnPreRender);
    }
  }
}