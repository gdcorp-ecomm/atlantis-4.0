
using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Shared_Captcha_Captcha : UserControl
{

  private const string SESSION_PREFIX = "Captcha";
  private const string SESSION_DELIM = ".";

  #region PublicProperties

  private string _instanceID = string.Empty;
  public string InstanceID
  {
    get
    {
      return _instanceID;
    }
    set
    {
      _instanceID = value;
    }
  }

  #endregion

  #region PublicMethods

  private bool _saveValidationDataInSession;
  public bool SaveValidationDataInSession
  {
    get
    {
      return _saveValidationDataInSession;
    }
    set
    {
      _saveValidationDataInSession = true;
    }
  }

  public bool IsCaptchaValid(string captchaString)
  {
    bool isValid = false;
    try
    {
     //Validation of Captcha from Session data
      string captchaValue = Request.QueryString["captchaValue"]as string;
      return captchaTemplate1.IsCaptchaValid(captchaValue);
    }
    catch (Exception ex)
    {
     
    }
    return isValid;
  }

  private string _captchaLable = string.Empty;
  public string CaptchaLable
  {
    get
    {
      return _captchaLable;
    }
    set
    {
      _captchaLable = value;
    }
  }

  #endregion PublicMethods

  #region PageEvents

  protected override void OnInit(EventArgs e)
  {
    try
    {
      Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~/web.config");
      SessionStateSection sessionConfigSection = (SessionStateSection)configuration.GetSection("system.web/sessionState");
      string sessionCookieName = sessionConfigSection.CookieName;
      HttpCookie sessionCookie = Request.Cookies[sessionCookieName];
      captchaTemplate1.CaptchaID = InstanceID;
      captchaTemplate1.CaptchaValueID = "cart_captcha_value_" + InstanceID;
      captchaTemplate1.ReloadLinkID = "change_code_" + InstanceID;
      captchaTemplate1.PlaySoundLinkID="speak_code_"+InstanceID;
      captchaTemplate1.SaveValidationDataInSession = SaveValidationDataInSession;
      if (sessionCookie != null)
      {
        string checkForScript = HttpUtility.UrlDecode(sessionCookie.Value).ToLower();
        if (checkForScript.Contains("<script"))
        {
          this.Visible = false;
        }
      }
    }
    catch (System.Threading.ThreadAbortException)
    {
    }
    catch (System.Exception ex)
    {

    }

    base.OnInit(e);
  }

  protected override void OnPreRender(EventArgs e)
  {
    if (string.IsNullOrEmpty(CaptchaLable))
    {
      PlaceHolder plcCaptchaLable = captchaTemplate1.FindControl("plcCaptchaLable") as PlaceHolder;
      if (plcCaptchaLable != null)
      {
        plcCaptchaLable.Visible = false;
      }
    }
    base.OnPreRender(e);
  }

  #endregion


}