using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class WebControlPaceHolderHandler : WebControlPlaceHolderHandlerBase
  {
    public override string Type { get { return PlaceHolderTypes.WebControl; } }

    internal WebControlPaceHolderHandler(string placeHolderDataRaw, ICollection<string> debugContextErrors) : base(placeHolderDataRaw, debugContextErrors)
    {
    }

    protected override Control InitializeControl(string placeHolderDataRaw)
    {
      Control control = null;

      try
      {
        PlaceHolderData placeHolderData = new PlaceHolderData(placeHolderDataRaw);

        string assemblyName;
        string typeName;
        
        if (placeHolderData.TryGetAttribute(PlaceHolderAttributes.Assembly, out assemblyName) &&
            placeHolderData.TryGetAttribute(PlaceHolderAttributes.Type, out typeName))
        {
          Type type = WebControlTypeCache.GetType(assemblyName, typeName);

          control = WebControlManager.Contruct(type, placeHolderData);
        }
        else
        {
          throw new Exception(string.Format("Attributes \"{0}\" and \"{1}\" are required.", PlaceHolderAttributes.Assembly, PlaceHolderAttributes.Type));
        }
        
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder error loading control. Type: {0}, Message: {1}", Type, ex.Message),
                    "WebControlPaceHolderHandler.InitializeControl()");
      }

      return control;
    }
  }
}