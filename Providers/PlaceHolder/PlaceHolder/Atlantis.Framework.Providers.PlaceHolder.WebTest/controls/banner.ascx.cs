using System.Collections.Generic;
using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest.controls
{
  public partial class banner : UserControl, IPlaceHolderControl
  {
    private IDictionary<string, string> _parameters;
    public IDictionary<string, string> Parameters 
    { 
      get
      {
        if (_parameters == null)
        {
          _parameters = new Dictionary<string, string>(32);
        }
        return _parameters;
      }
    }
  }
}