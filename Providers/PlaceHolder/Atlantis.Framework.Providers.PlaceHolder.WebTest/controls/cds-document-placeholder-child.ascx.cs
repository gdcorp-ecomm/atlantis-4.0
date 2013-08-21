using System;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest.controls
{
  public partial class cds_document_placeholder_child : UserControl
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

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      _textSetOnInit = "CDS Document Placeholder child OnInit";
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      _textSetOnLoad = "CDS Document Placeholder child OnInit";
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      _textSetOnPreRender = "CDS Document Placeholder child OnInit";
    }
  }
}