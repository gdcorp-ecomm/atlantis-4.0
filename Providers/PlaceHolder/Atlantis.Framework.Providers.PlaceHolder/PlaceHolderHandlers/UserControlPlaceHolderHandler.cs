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
    private readonly XmlDataSerializer _xmlDataSerializer = new XmlDataSerializer();

    public string Type { get { return PlaceHolderTypes.UserControl; } }

    public string GetPlaceHolderContent(string name, string data, IDictionary<string, IPlaceHolderData> placeHolderSharedData, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string renderContent = string.Empty;

      try
      {
        IPlaceHolderData placeHolderData = DeserializeData(data);
        Control userControl = InitializeUserControl(placeHolderData);
        if (!string.IsNullOrEmpty(placeHolderData.Id))
        {
          userControl.ID = placeHolderData.Id;
        }
        else
        {
          userControl.ID = userControl.GetType().ToString();
        }
        placeHolderSharedData[userControl.ID] = placeHolderData;

        renderContent = RenderControlToHtml(userControl);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("{0} place holder error. {1}", Type, ex.Message);

        debugContextErrors.Add(errorMessage);
        ErrorLogger.LogException(errorMessage, "RenderPlaceHolderContent()", data);
      }

      return renderContent;
    }

    private IPlaceHolderData DeserializeData(string data)
    {
      IPlaceHolderData placeHolderData = _xmlDataSerializer.Deserialize<XmlPlaceHolderData>(data);
      
      if (placeHolderData == null)
      {
        throw new Exception("Unhandled deserialization error, null returned.");
      }

      return placeHolderData;
    }

    private Control InitializeUserControl(IPlaceHolderData placeHolderData)
    {
      Control userControl;

      try
      {
        Page currentPage = HttpContext.Current.Handler == null ? new Page() : (Page)HttpContext.Current.Handler;
        userControl = currentPage.LoadControl(placeHolderData.Location);
        
        if (userControl == null)
        {
          throw new Exception("Unhandled exception loading user control.");
        }
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
