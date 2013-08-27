using System;
using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class UserControlPlaceHolderHandler : WebControlPlaceHolderHandlerBase
  {
    internal UserControlPlaceHolderHandler(IPlaceHolderHandlerContext context) : base(context)
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

        control = WebControlManager.Construct(type, placeHolderData);
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder error loading control. Type: {0}, Message: {1}", Context.Type, ex.Message),
                    "UserControlPlaceHolderHandler.Initialize()");
      }

      return control;
    }
  }
}
