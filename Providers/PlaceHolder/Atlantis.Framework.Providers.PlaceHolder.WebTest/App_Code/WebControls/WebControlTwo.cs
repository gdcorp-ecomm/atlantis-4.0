using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.WebTest;
using System.Web.UI;

namespace WebControls
{
  public class WebControlTwo : Control
  {
    private IPlaceHolderData _placeHolderData;
    protected IPlaceHolderData PlaceHolderData
    {
      get
      {
        if (_placeHolderData == null)
        {
          IPlaceHolderProvider placeHolderProvider = ProviderContainerHelper.Instance.Resolve<IPlaceHolderProvider>();
          _placeHolderData = placeHolderProvider.GetPlaceHolderData(ID);
        }

        return _placeHolderData;
      }
    }


    protected override void Render(HtmlTextWriter writer)
    {
      string text;
      writer.WriteLine("<h1>{0}</h1>", PlaceHolderData.TryGetParameter("text", out text) ? text : "Failed to retreive \"text\" parameter.");
    }
  }
}