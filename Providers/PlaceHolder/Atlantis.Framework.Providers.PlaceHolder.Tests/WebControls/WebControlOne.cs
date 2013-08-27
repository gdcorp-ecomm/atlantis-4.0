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

    public int intVal { get; set; }
    public bool boolVal { get; set; }
    public DateTime dateVal { get; set; }

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
      
      if (intVal>0)
      writer.Write(string.Format("int value is {0}!", intVal));
      
      if (boolVal)
      writer.Write(string.Format("bool value is {0}!", boolVal));
      
      if (dateVal >DateTime.MinValue)
      writer.Write(string.Format("datetime value is {0}!", dateVal));
      
      writer.Write(TextSetOnInit);
      writer.Write(TextSetOnLoad);
      writer.Write(TextSetOnPreRender);
    }
  }
}
