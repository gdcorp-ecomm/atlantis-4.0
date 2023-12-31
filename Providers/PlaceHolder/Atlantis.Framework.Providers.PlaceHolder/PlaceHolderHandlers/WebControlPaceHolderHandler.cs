﻿using System;
using System.Threading;
using System.Web.UI;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder.PlaceHolderHandlers
{
  internal class WebControlPaceHolderHandler : WebControlPlaceHolderHandlerBase
  {
    internal WebControlPaceHolderHandler(IPlaceHolderHandlerContext context) : base(context)
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

          control = WebControlManager.Construct(type, placeHolderData);
        }
        else
        {
          throw new Exception(string.Format("Attributes \"{0}\" and \"{1}\" are required.", PlaceHolderAttributes.Assembly, PlaceHolderAttributes.Type));
        }
        
      }
      catch (ThreadAbortException)
      {
      }
      catch (Exception ex)
      {
        HandleError(string.Format("PlaceHolder error loading control. Type: {0}, Message: {1}", Context.Type, ex.Message), "WebControlPaceHolderHandler.Initialize()");
      }

      return control;
    }
  }
}