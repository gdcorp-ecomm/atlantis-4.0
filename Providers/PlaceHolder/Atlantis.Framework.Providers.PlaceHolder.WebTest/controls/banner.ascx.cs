using System.Collections.Generic;
using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.WebTest.controls
{
  public partial class banner : UserControl, IPlaceHolderControl
  {
    private readonly IDictionary<string, string> _parameters = new Dictionary<string, string>(32);
    public IDictionary<string, string> Parameters 
    {
      get { return _parameters; }
    }

    public bool ValidateParameters(out string errorLogMessage)
    {
      bool isValid = true;

      errorLogMessage = string.Empty;

      if (!Parameters.ContainsKey("title"))
      {
        errorLogMessage = "Parameter \"title\" is required.";
        isValid = false;
      }
      else if (!Parameters.ContainsKey("text"))
      {
        errorLogMessage = "Parameter \"text\" is required.";
        isValid = false;
      }

      return isValid;
    }
  }
}