using System;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest.controls
{
  public partial class banner : UserControl
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

    public override bool Visible
    {
      get
      {
        return !string.IsNullOrEmpty(Title) && 
               !string.IsNullOrEmpty(Text);
      }
    }

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
  }
}