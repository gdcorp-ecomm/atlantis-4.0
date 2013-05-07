using System;
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

    public string Name { get { return "userControl"; } }

    public string GetPlaceHolderContent(string name, string data, IProviderContainer providerContainer)
    {
      string renderContent = string.Empty;

      try
      {
        PlaceHolderData placeHolderData = DeserializeData(data);
        Control userControl = InitializeUserControl(placeHolderData);

        renderContent = RenderControlToHtml(userControl);
      }
      catch (Exception ex)
      {
        ErrorLogger.LogException(string.Format("{0} place holder error. {1}", Name, ex.Message), "RenderPlaceHolderContent()", data);
      }

      return renderContent;
    }

    private PlaceHolderData DeserializeData(string data)
    {
      PlaceHolderData placeHolderData = _xmlDataSerializer.Deserialize<PlaceHolderData>(data);
      
      if (placeHolderData == null)
      {
        throw new Exception("Unhandled deserialization error, null returned.");
      }

      return placeHolderData;
    }

    private Control InitializeUserControl(PlaceHolderData placeHolderData)
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

        SetUserControlParameters(userControl, placeHolderData);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error loading user control. {0}.", ex.Message));
      }

      return userControl;
    }

    private void SetUserControlParameters(Control userControl, PlaceHolderData placeHolderData)
    {
      IPlaceHolderControl placeHolderControl = userControl as IPlaceHolderControl;
      if (placeHolderControl == null)
      {
        throw new Exception("User control must implement IPlaceHolderControl.");
      }

      foreach (PlaceHolderParameter placeHolderParameter in placeHolderData.Parameters)
      {
        placeHolderControl.Parameters[placeHolderParameter.Key] = placeHolderParameter.Value;
      }
    }

    private string RenderControlToHtml(Control userControl)
    {
      StringBuilder htmlStringBuilder = new StringBuilder();
      StringWriter stringWriter = new StringWriter(htmlStringBuilder);
      HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

      userControl.RenderControl(htmlTextWriter);
      return htmlStringBuilder.ToString();
    }
  }
}
