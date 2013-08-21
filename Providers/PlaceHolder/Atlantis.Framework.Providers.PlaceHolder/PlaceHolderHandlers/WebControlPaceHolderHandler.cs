using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class WebControlPaceHolderHandler : WebControlPlaceHolderHandlerBase
  {
    internal WebControlPaceHolderHandler(string markup, string data, ICollection<string> debugContextErrors) : base(markup, data, debugContextErrors)
    {
    }

    public override string Type { get { return PlaceHolderTypes.WebControl; } }

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

          control = WebControlManager.Construct(type, placeHolderData);
        }
        else
        {
          throw new Exception(string.Format("Attributes \"{0}\" and \"{1}\" are required.", PlaceHolderAttributes.Assembly, PlaceHolderAttributes.Type));
        }
        
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder error loading control. Type: {0}, Message: {1}", Type, ex.Message),
                    "WebControlPaceHolderHandler.Initialize()");
      }

      return control;
    }
  }
}