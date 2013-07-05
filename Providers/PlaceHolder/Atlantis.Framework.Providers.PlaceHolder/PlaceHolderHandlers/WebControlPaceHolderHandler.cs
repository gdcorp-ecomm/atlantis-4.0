using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal class WebControlPaceHolderHandler : IPlaceHolderHandler
  {
    private readonly XmlDataSerializer _xmlDataSerializer = new XmlDataSerializer();

    public string Type { get { return PlaceHolderTypes.WebControl; } }

    public string GetPlaceHolderContent(string name, string data, IDictionary<string, IPlaceHolderData> placeHolderSharedData, ICollection<string> debugContextErrors, IProviderContainer providerContainer)
    {
      string renderContent = string.Empty;

      try
      {
        IWebControlPlaceHolderData placeHolderData = DeserializeData(data);
        Control webControl = InitializeWebControl(placeHolderData);
        if (!string.IsNullOrEmpty(placeHolderData.Id))
        {
          webControl.ID = placeHolderData.Id;
        }
        else
        {
          webControl.ID = webControl.GetType().ToString();
        }
        placeHolderSharedData[webControl.ID] = placeHolderData;
        renderContent = RenderControlToHtml(webControl);
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("{0} place holder error. {1}", Type, ex.Message);

        debugContextErrors.Add(errorMessage);
        ErrorLogger.LogException(errorMessage, "RenderPlaceHolderContent()", data);
      }

      return renderContent;
    }

    private IWebControlPlaceHolderData DeserializeData(string data)
    {
      IWebControlPlaceHolderData placeHolderData = _xmlDataSerializer.Deserialize<XmlWebControlPlaceHolderData>(data);

      if (placeHolderData == null)
      {
        throw new Exception("Unhandled deserialization error, null returned.");
      }

      return placeHolderData;
    }

    private Control InitializeWebControl(IWebControlPlaceHolderData placeHolderData)
    {
      Control webControl;

      try
      {
        Page currentPage = HttpContext.Current.Handler == null ? new Page() : (Page)HttpContext.Current.Handler;
        Type type = GetType(placeHolderData.AssemblyName, placeHolderData.Location);
        if (type != null)
        {
          webControl = currentPage.LoadControl(type, null);

          if (webControl == null)
          {
            throw new Exception("Unhandled exception loading web control.");
          }
        }
        else
        {
          throw new Exception(string.Format("Type {0} not found.", placeHolderData.Location));
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error loading web control. {0}.", ex.Message));
      }

      return webControl;
    }

    private string RenderControlToHtml(Control webControl)
    {
      string html = string.Empty;

      if (webControl != null)
      {
        StringBuilder htmlStringBuilder = new StringBuilder();
        StringWriter stringWriter = new StringWriter(htmlStringBuilder);
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

        webControl.RenderControl(htmlTextWriter);

        html = htmlStringBuilder.ToString();
      }

      return html;
    }

    private Type GetType(string assemblyName, string typeName)
    {
      Type type = null;

      try
      {
        type = Assembly.Load(assemblyName).GetType(typeName);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error finding the type {0}.  {1}", typeName, ex.Message));
      }

      return type;
    }
  }
}
