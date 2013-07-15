using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest.controls
{
  public partial class banner : UserControl
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

    private string _title;
    protected string Title
    {
      get
      {
        if (_title == null)
        {
          if (!PlaceHolderData.TryGetParameter("title", out _title))
          {
            _title = "Failed to retreive \"title\" parameter.";
          }
        }
        return _title;
      }
    }

    private string _text;
    protected string Text
    {
      get
      {
        if (_text == null)
        {
          if (!PlaceHolderData.TryGetParameter("text", out _text))
          {
            _text = "Failed to retreive \"text\" parameter.";
          }
        }
        return _text;
      }
    }

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