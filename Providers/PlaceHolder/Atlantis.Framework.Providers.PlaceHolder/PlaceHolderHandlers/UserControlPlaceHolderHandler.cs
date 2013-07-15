using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class UserControlPlaceHolderHandler : IPlaceHolderHandler
  {
    public string Type { get { return PlaceHolderTypes.UserControl; } }

    public string GetPlaceHolderContent(string type, string data, IDictionary<string, IPlaceHolderData> placeHolderSharedData, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string renderContent = string.Empty;

      try
      {
        PlaceHolderData placeHolderData = new PlaceHolderData(data);

        Control userControl = InitializeUserControl(placeHolderData);

        placeHolderSharedData[userControl.ID] = placeHolderData;

        renderContent = RenderControlToHtml(userControl);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("{0} place holder error. {1}", Type, ex.Message);

        debugContextErrors.Add(errorMessage);
        ErrorLogger.LogException(errorMessage, "UserControlPlaceHolderHandler.GetPlaceHolderContent()", data);
      }

      return renderContent;
    }

    private Control InitializeUserControl(PlaceHolderData placeHolderData)
    {
      Control userControl;

      try
      {
        string location;
        if (!placeHolderData.TryGetAttribute(PlaceHolderAttributes.Location, out location))
        {
          throw new Exception("Attribute \"" + PlaceHolderAttributes.Location + "\" is required.");
        }

        Page currentPage = HttpContext.Current.Handler == null ? new Page() : (Page)HttpContext.Current.Handler;
        userControl = currentPage.LoadControl(location);
        
        if (userControl == null)
        {
          throw new Exception("Unhandled exception loading user control.");
        }

        string id;
        userControl.ID = placeHolderData.TryGetAttribute(PlaceHolderAttributes.Id, out id) ? id : userControl.GetType().ToString();
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error loading user control. {0}.", ex.Message));
      }

      return userControl;
    }

    private string RenderControlToHtml(Control userControl)
    {
      string html = string.Empty;

      if (userControl != null)
      {
        StringBuilder htmlStringBuilder = new StringBuilder();
        StringWriter stringWriter = new StringWriter(htmlStringBuilder);
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

        userControl.RenderControl(htmlTextWriter);

        html = htmlStringBuilder.ToString();
      }

      return html;
    }
  }
}
