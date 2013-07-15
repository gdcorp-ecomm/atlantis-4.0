using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.Tests.WebControls
{
  public class WebControlOne : Control
  {
    private IPlaceHolderData _placeHolderData;
    protected IPlaceHolderData PlaceHolderData
    {
      get
      {
        if (_placeHolderData == null)
        {
          IPlaceHolderProvider placeHolderProvider = PlaceHolderProviderTests.ProviderContainer.Resolve<IPlaceHolderProvider>();
          _placeHolderData = placeHolderProvider.GetPlaceHolderData(ID);
        }

        return _placeHolderData;
      }
    }

    protected override void Render(HtmlTextWriter writer)
    {
      writer.Write("Web Control One!");

      string title;
      if (PlaceHolderData.TryGetParameter("title", out title))
      {
        writer.Write(title);
      }

      string text;
      if (PlaceHolderData.TryGetParameter("text", out text))
      {
        writer.Write(text);
      }
    }
  }
}
