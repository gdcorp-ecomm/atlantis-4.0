
using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Atlantis.Framework.Web.DebugControl
{
  public class DebugControl : CompositeControl
  {
    private const string ENABLE_QA_DEBUGCONTROL = "QA_DEBUGCONTROL_ENABLED";

    private HtmlGenericControl _containerDiv;

    protected string BrowserInfoValue = string.Empty;
    protected string SessionInfoValue = string.Empty;

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

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
      string appSettingValue = DataCache.DataCache.GetAppSetting(ENABLE_QA_DEBUGCONTROL);
      if (appSettingValue.ToLower() == "true")
      {
        GetBrowserInfo();
        GetSessionInfo();

        writer.Write("<span>");

        StringBuilder debugInfo = new StringBuilder();
        foreach (var debugItem in DebugData)
        {
          debugInfo.AppendLine(debugItem.Key + ": " + debugItem.Value);
        }

        debugInfo =
          debugInfo.Replace("'", "")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("&quot;", "")
            .Replace("\"", "");

        writer.Write("<style>.debugBtn{font-family: sans-serif; background: #70ac00; border-bottom: 3px solid #5a8c00; border: 0; color: #fff; display: " 
        + "inline-block; font-size: 14px; line-height: 14px; padding: 13px 35px 11px; text-align: center; text-decoration: none;} " 
        + ".debugBtn:hover{background: #97c534; border-color: #78aa17;} </style>");
        
        writer.Write("<script language=\"javascript\" type=\"text/javascript\">" +
                     "function GetSessionDebugInfo() {" +
                     " return '" + SessionInfoValue + "';" +
                     "}" +
                     "function GetBrowserDebugInfo() {" +
                     " return '" + BrowserInfoValue + "';" +
                     "}" +
                     "function PostForm() {" +
                     "var form = document.createElement(\"form\");" +
                     "form.setAttribute(\"method\", \"post\");" +
                     "form.setAttribute(\"target\", \"_blank\");" +
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


        writer.Write(
          "<div id=\"submit\" class=\"debugBtn\" style=\"border: none; cursor: pointer\" onclick=\"PostForm(GetBrowserDebugInfo() , GetSessionDebugInfo())\">Report Feedback</div>");

        writer.Write("</span>");
      }
    }

    private void GetBrowserInfo()
    {
      try
      {
        var browser = HttpContext.Current.Request.Browser;

        StringBuilder browserInfo = new StringBuilder();

        browserInfo.AppendLine("Browser: " + browser.Browser);
        browserInfo.AppendLine("Platform: " + browser.Platform);
        browserInfo.AppendLine("Version: " + browser.Version);
        browserInfo.AppendLine("IsMobileDevice: " + browser.IsMobileDevice);

        BrowserInfoValue = browserInfo.ToString().Replace("\r\n", ",");
      }
      catch
      {
        BrowserInfoValue = "Unable to determine Browser Info.";
      }
    }

    private void GetSessionInfo()
    {
      try
      {
        var session = HttpContext.Current.Session;

        StringBuilder sessionInfo = new StringBuilder();

        foreach (var key in session.Keys)
        {
          sessionInfo.AppendLine(key + ": " + session[key.ToString()]);
        }

        SessionInfoValue = sessionInfo.ToString().Replace("\r\n", ",");

        SessionInfoValue =
          SessionInfoValue.Replace("'", "")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("&quot;", "")
            .Replace("\"", "");
      }
      catch
      {
        SessionInfoValue = "Unable to determine Session Info";
      }
    }
  }
}
