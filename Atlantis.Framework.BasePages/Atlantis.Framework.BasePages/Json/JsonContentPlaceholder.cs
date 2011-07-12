using System.Web.UI.WebControls;

namespace Atlantis.Framework.BasePages.Json
{

  public class JsonContentPlaceholder : PlaceHolder
  {
    public const string JsonContentPlaceHolderContextKey = "Atlantis.Framework.JsonContentPlaceHolder";

    public JsonContentPlaceholder()
    {
      this.Init += new System.EventHandler(JsonContentPlaceholder_Init);
    }

    void JsonContentPlaceholder_Init(object sender, System.EventArgs e)
    {
      Context.Items[JsonContentPlaceHolderContextKey] = this;
    }
  }
}
