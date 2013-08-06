using System;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest.controls
{
  public partial class child2 : UserControl
  {
    private string _childText = "ERROR: Child2 DID NOT fire OnLoad";
    private string ChildText
    {
      get { return _childText; }
      set { _childText = value; }
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      ChildText = "Child2 fired OnLoad!!";
    }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.WriteLine("<div>{0}</div>", ChildText);
    }
  }
}