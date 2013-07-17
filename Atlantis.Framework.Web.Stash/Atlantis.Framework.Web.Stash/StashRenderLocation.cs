using System.Web.UI.WebControls;
using System.Web.UI;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Web.Stash
{
  public class StashRenderLocation : PlaceHolder
  {
    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get { return _providerContainer ?? (_providerContainer = HttpProviderContainer.Instance); }
    }

    private string _location;
    public string Location 
    { 
      get
      {
        if (_location == null)
        {
          IPlaceHolderProvider placeHolderProvider;
          string location;

          if (ProviderContainer.TryResolve(out placeHolderProvider) &&
              placeHolderProvider.GetPlaceHolderData(ID).TryGetParameter("Location", out location))
          {
            _location = location;
          }
        }
        return _location;
      }
      set { _location = value; }
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (!string.IsNullOrEmpty(Location))
      {
        string html = StashContext.GetRenderedStashContent(Location);
        if (!string.IsNullOrEmpty(html))
        {
          writer.WriteLine(html);
        }
      }
    }
  }
}