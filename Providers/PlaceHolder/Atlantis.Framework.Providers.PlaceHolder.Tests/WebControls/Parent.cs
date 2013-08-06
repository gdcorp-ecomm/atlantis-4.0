using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls
{
  public class Parent : Control
  {
    public Parent()
    {
      Controls.Add(new Child());
    }
  }
}
