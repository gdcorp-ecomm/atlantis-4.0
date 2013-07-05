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

    public override bool Visible
    {
      get
      {
        return _placeHolderData.Parameters.ContainsKey("title") && 
               _placeHolderData.Parameters.ContainsKey("text");
      }
    }
  }
}