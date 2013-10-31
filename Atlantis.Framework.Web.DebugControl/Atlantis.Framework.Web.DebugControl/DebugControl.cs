
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Text;
using Atlantis.Framework.Providers.Interface.Links;

namespace Atlantis.Framework.Web.DebugControl
{
  public class DebugControl : CompositeControl
  {
    private HtmlGenericControl _containerDiv;

    protected string BrowserInfoValue;
    protected string SessionInfoValue;

    protected List<KeyValuePair<string, string>> DebugData
    {
      get
      {
        IDebugContext debug = HttpProviderContainer.Instance.Resolve<IDebugContext>();
        return debug.GetDebugTrackingData();
      }
    }

    private ILinkProvider _links;
    private ILinkProvider Links
    {
      get
      {
        if (_links == null)
        {
          _links = HttpProviderContainer.Instance.Resolve<ILinkProvider>();
        }

        return _links;
      }
    }

    private string _siteAdminLink;
    private string SiteAdminLink
    {
      get
      {
        if (_siteAdminLink == null)
        {
          _siteAdminLink = Links.GetUrl("SITEADMINURL", "DevTools/IssueCollector/IssueCollector.aspx");
        }
        if (String.IsNullOrEmpty(_siteAdminLink) || HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("debug"))
        {
          _siteAdminLink = "http://siteadmin.debug.intranet.gdg/DevTools/IssueCollector/IssueCollector.aspx";
        }
        return _siteAdminLink;
      }
    }
    
    protected override void CreateChildControls()
    {
      this.CreateControlCollection();
      
      GetBrowserInfo();
      GetSessionInfo();
      
      _containerDiv = new HtmlGenericControl();
      
      StringBuilder debugInfo = new StringBuilder();
      foreach (var debugItem in DebugData)
      {
        debugInfo.AppendLine(debugItem.Key + ": " + debugItem.Value);
      }

      var script =
        new LiteralControl("<script language=\"javascript\" type=\"text/javascript\">" +
        "function PostForm() {"+
            "var form = document.createElement(\"form\");"+
            "form.setAttribute(\"method\", \"post\");"+
            "form.setAttribute(\"action\", \"" + SiteAdminLink + "\" );"+
            "var hiddenField = document.createElement(\"input\");" +
            "hiddenField.setAttribute(\"type\", \"hidden\");" +
            "hiddenField.setAttribute(\"name\", \"debugInfo\");"+
            "hiddenField.setAttribute(\"value\", \"" + debugInfo.ToString().Replace("\r\n", ",") + "\");" +
            "form.appendChild(hiddenField);"+

            "var browserInfo = document.createElement(\"input\");" +
            "browserInfo.setAttribute(\"type\", \"hidden\");" +
            "browserInfo.setAttribute(\"name\", \"browserInfo\");" +
            "browserInfo.setAttribute(\"value\", arguments[0]);" +
            "form.appendChild(browserInfo);" +
            
            "var sessionInfo = document.createElement(\"input\");" +
            "sessionInfo.setAttribute(\"type\", \"hidden\");" +
            "sessionInfo.setAttribute(\"name\", \"sessionInfo\");" +
            "sessionInfo.setAttribute(\"value\", arguments[1]);" +
            "form.appendChild(sessionInfo);" +

            "document.body.appendChild(form);"+
            "form.submit();"+
        "}"+
    "</script>");

      _containerDiv.Controls.Add(script);
      
      var submitButton = new HtmlGenericControl("div");
      
      submitButton.ID = "submit";
      submitButton.InnerHtml = "Report Feedback";
      submitButton.Attributes.Add("class", "g-btn-lg g-btn-prg");
      submitButton.Attributes.Add("style", "border: none; cursor: pointer");
      submitButton.Attributes.Add("onclick", "PostForm('" + BrowserInfoValue + "' , '" + SessionInfoValue + "')");
      _containerDiv.Controls.Add(submitButton);
      
      Controls.Add(_containerDiv);

      ChildControlsCreated = true;
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
      //_containerDiv.RenderControl(writer);

      GetBrowserInfo();
      GetSessionInfo();

      writer.Write("<span>");

      StringBuilder debugInfo = new StringBuilder();
      foreach (var debugItem in DebugData)
      {
        debugInfo.AppendLine(debugItem.Key + ": " + debugItem.Value);
      }

      writer.Write("<script language=\"javascript\" type=\"text/javascript\">" +
        "function PostForm() {" +
            "var form = document.createElement(\"form\");" +
            "form.setAttribute(\"method\", \"post\");" +
            "form.setAttribute(\"action\", \"" + SiteAdminLink + "\" );" +
            "var hiddenField = document.createElement(\"input\");" +
            "hiddenField.setAttribute(\"type\", \"hidden\");" +
            "hiddenField.setAttribute(\"name\", \"debugInfo\");" +
            "hiddenField.setAttribute(\"value\", \"" + debugInfo.ToString().Replace("\r\n", ",") + "\");" +
            "form.appendChild(hiddenField);" +

            "var browserInfo = document.createElement(\"input\");" +
            "browserInfo.setAttribute(\"type\", \"hidden\");" +
            "browserInfo.setAttribute(\"name\", \"browserInfo\");" +
            "browserInfo.setAttribute(\"value\", arguments[0]);" +
            "form.appendChild(browserInfo);" +

            "var sessionInfo = document.createElement(\"input\");" +
            "sessionInfo.setAttribute(\"type\", \"hidden\");" +
            "sessionInfo.setAttribute(\"name\", \"sessionInfo\");" +
            "sessionInfo.setAttribute(\"value\", arguments[1]);" +
            "form.appendChild(sessionInfo);" +

            "document.body.appendChild(form);" +
            "form.submit();" +
        "}" +
    "</script>");


      writer.Write("<div id=\"submit\" class=\"g-btn-lg g-btn-prg\" style=\"border: none; cursor: pointer\" onclick=\"PostForm(&quot;" + BrowserInfoValue + "&quot; , &quot;" + SessionInfoValue + "&quot;)\">Report Feedback</div>");
      
      writer.Write("</span>");
    }

    private void GetBrowserInfo()
    {
      var browser = HttpContext.Current.Request.Browser;

      StringBuilder browserInfo = new StringBuilder();

      browserInfo.AppendLine("Browser: " + browser.Browser);
      browserInfo.AppendLine("Platform: " + browser.Platform);
      browserInfo.AppendLine("Version: " + browser.Version);
      browserInfo.AppendLine("IsMobileDevice: " + browser.IsMobileDevice);
      
      BrowserInfoValue = browserInfo.ToString().Replace("\r\n", ",");
    }

    private void GetSessionInfo()
    {
      var session = HttpContext.Current.Session;

      StringBuilder sessionInfo = new StringBuilder();

      foreach (var key in session.Keys)
      {
        sessionInfo.AppendLine(key + ": " + session[key.ToString()]);
      }

      SessionInfoValue = sessionInfo.ToString().Replace("\r\n", ",");
    }
  }
}
