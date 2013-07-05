using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Providers.PlaceHolder.WebTest;
using System.Web.UI;

namespace WebControls
{
  public class WebControlOne : Control
  {
    private IWebControlPlaceHolderData _placeHolderData;
    protected IWebControlPlaceHolderData PlaceHolderData
    {
      get
      {
        if (_placeHolderData == null)
        {
          IPlaceHolderProvider placeHolderProvider = ProviderContainerHelper.Instance.Resolve<IPlaceHolderProvider>();
          _placeHolderData = (IWebControlPlaceHolderData)placeHolderProvider.GetPlaceHolderData(ID);
        }

        return _placeHolderData;
      }
    }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.WriteLine(string.Format("<h1>{0}</h1>", PlaceHolderData.Parameters["text"].Value));
    }
  }
}