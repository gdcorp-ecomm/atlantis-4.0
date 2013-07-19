using System;
using System.Collections.Generic;
using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class UserControlPlaceHolderHandler : WebControlPlaceHolderHandlerBase
  {
    public override string Type { get { return PlaceHolderTypes.UserControl; } }

    internal UserControlPlaceHolderHandler(string placeHolderDataRaw, ICollection<string> debugContextErrors) : base(placeHolderDataRaw, debugContextErrors)
    {
    }

    protected override Control InitializeControl(string placeHolderDataRaw)
    {
      Control control = null;

      try
      {
        PlaceHolderData placeHolderData = new PlaceHolderData(placeHolderDataRaw);

        string location;
        if (!placeHolderData.TryGetAttribute(PlaceHolderAttributes.Location, out location))
        {
          throw new Exception("Attribute \"" + PlaceHolderAttributes.Location + "\" is required.");
        }

        Type type = UserControlTypeCache.GetType(location);

        control = WebControlManager.Contruct(type, placeHolderData);
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder error loading control. Type: {0}, Message: {1}", Type, ex.Message),
                    "UserControlPlaceHolderHandler.InitializeControl()");
      }

      return control;
    }
  }
}
