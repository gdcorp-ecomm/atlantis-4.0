using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest.controls
{
  public partial class banner : UserControl
  {
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
  }
}