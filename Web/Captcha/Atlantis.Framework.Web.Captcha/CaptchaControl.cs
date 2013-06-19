using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BotDetect.Web;
using BotDetect.Web.UI;
using System.Web.UI.HtmlControls;
using Atlantis.Framework.Web.Stash;

namespace Atlantis.Framework.Web.CaptchaCtl
{
  [
AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level = AspNetHostingPermissionLevel.Minimal),
AspNetHostingPermission(SecurityAction.Demand,
    Level = AspNetHostingPermissionLevel.Minimal),
ToolboxData(
    "<{0}:CaptchaTemplateControl runat=\"server\"> </{0}:CaptchaTemplateControl>")]
  public class CaptchaTemplateControl : CompositeControl
  {
    protected override void OnInit(EventArgs e)
    {
      SetupChildControlDefaults();
      base.OnInit(e);
    }

    public override void RenderBeginTag(HtmlTextWriter writer)
    {
      writer.Write(" ");
    }
    public override void RenderEndTag(HtmlTextWriter writer)
    {
      writer.Write(" ");
    }

    protected override HtmlTextWriterTag TagKey
    {
      get
      {
        return HtmlTextWriterTag.Div;
      }
    }

    private ITemplate templateValue;
    private TemplateOwner ownerValue;
    private HtmlGenericControl _imageContainer = new HtmlGenericControl();
    private HtmlGenericControl _audioContainer = new HtmlGenericControl();
    private HtmlImage _reloadImage = new HtmlImage();
    private HtmlImage _playSound = new HtmlImage();
    private StashContent _stashLocation = new StashContent();

    private bool _isSoundEnabled;

    private void SetupChildControlDefaults()
    {
      _reloadImage.Alt = "Change the Captcha code";
      _reloadImage.ID = "reloadIcon";
      _reloadImage.Attributes["class"] = "captcha-reload-icon";
      _reloadImage.Src = "/BotDetectCaptcha.ashx?get=ReloadIcon";

      _playSound.Alt = "Play the Captcha code";
      _playSound.ID = "soundIcon";
      _playSound.Attributes["class"] = "captcha-playsound-icon";
      _playSound.Src = "/BotDetectCaptcha.ashx?get=SoundIcon";

      _stashLocation.Location = StashRenderLocation;
      _stashLocation.StashBeforeRender = true;
      _stashLocation.RenderKey = "CaptchaJavascript-" + CaptchaID;

      _imageContainer.TagName = "div";

      _audioContainer.TagName = "div";
      _audioContainer.ID = CaptchaID + "_AudioPlaceholder";
    }

    public HtmlImage ReloadImage
    {
      get
      {
        return _reloadImage;
      }
    }
    public HtmlImage PlaySoundImage
    {
      get
      {
        return _playSound;
      }
    }
    public StashContent StashContentControl
    {
      get
      {
        return _stashLocation;
      }
    }

    private bool IsSoundEnabled
    {
      get
      {
        return _isSoundEnabled;
      }
      set
      {
        _isSoundEnabled = true;
      }
    }
    private string _captchaID = string.Empty;
    public string CaptchaID
    {
      get
      {
        return _captchaID;
      }
      set
      {
        _captchaID = value;
      }
    }

    private string _jqueryDelimiter = "$";
    public string JQueryDelim
    {
      get
      {
        return _jqueryDelimiter;
      }
      set
      {
        _jqueryDelimiter = value;
      }
    }

    private string _captchaInstanceId = string.Empty;
    public string CaptchaInstanceId
    {
      get
      {
        if (string.IsNullOrEmpty(_captchaInstanceId))
        {
          _captchaInstanceId = CaptchaControl.CaptchaControl.CurrentInstanceId;
        }

        return _captchaInstanceId;
      }
    }

    Captcha _currentCaptchaControl;
    public Captcha CaptchaControl
    {
      get
      {
        if (_currentCaptchaControl == null)
        {
          _currentCaptchaControl = new Captcha(CaptchaID);
        }
        return _currentCaptchaControl;
      }
    }

    private string _captchaImagePlaceHolderID;
    public string CaptchaImagePlaceHolderID
    {
      get
      {
        return _captchaImagePlaceHolderID;
      }
      set
      {
        _captchaImagePlaceHolderID = value;
      }
    }

    private string _captchaValueID;
    public string CaptchaValueID
    {
      get
      {
        return _captchaValueID;
      }
      set
      {
        _captchaValueID = value;
      }
    }

    private bool _autoFocusInput;
    public bool AutoFocusInput
    {
      get
      {
        return _autoFocusInput;
      }
      set
      {
        _autoFocusInput = value;
      }
    }

    private bool _autoClearInput;
    public bool AutoClearInput
    {
      get
      {
        return _autoClearInput;
      }
      set
      {
        _autoClearInput = value;
      }
    }

    private bool _ajaxAuthentication;
    public bool AjaxAuthentication
    {
      get
      {
        return _ajaxAuthentication;
      }
      set
      {
        _ajaxAuthentication = value;
      }
    }

    private string _ajaxAuthenticationCallback;
    public string AjaxAuthenticationCallback
    {
      get
      {
        return _ajaxAuthenticationCallback;
      }
      set
      {
        _ajaxAuthenticationCallback = value;
      }
    }


    private string _captchaValidateSelector;
    public string CaptchaValidateSelector
    {
      get
      {
        return _captchaValidateSelector;
      }
      set
      {
        _captchaValidateSelector = value;
      }
    }

    private string _imageReloadPlaceHolderID;
    public string ImageReloadPlaceHolderID
    {
      get
      {
        return _imageReloadPlaceHolderID;
      }
      set
      {
        _imageReloadPlaceHolderID = value;
      }
    }

    private string _playSoundPlaceHolderID;
    public string PlaySoundPlaceHolderID
    {
      get
      {
        return _playSoundPlaceHolderID;
      }
      set
      {
        _playSoundPlaceHolderID = value;
      }
    }

    private string _stashRenderLocation;
    public string StashRenderLocation
    {
      get
      {
        return _stashRenderLocation;
      }
      set
      {
        _stashRenderLocation = value;
      }
    }

    private bool _autobindEvents = true;
    public bool AutoBindEvents
    {
      get
      {
        return _autobindEvents;
      }
      set
      {
        _autobindEvents = value;
      }
    }

    private string _reloadLinkID;
    public string ReloadLinkID
    {
      get
      {
        return _reloadLinkID;
      }
      set
      {
        _reloadLinkID = value;
      }
    }

    private string _playSoundLinkID;
    public string PlaySoundLinkID
    {
      get
      {
        return _playSoundLinkID;
      }
      set
      {
        _playSoundLinkID = value;
      }
    }

    [
    Browsable(false),
    DesignerSerializationVisibility(
        DesignerSerializationVisibility.Hidden)
    ]
    public TemplateOwner Owner
    {
      get
      {
        return ownerValue;
      }
    }

    [
    Browsable(false),
    PersistenceMode(PersistenceMode.InnerProperty),
    DefaultValue(typeof(ITemplate), ""),
    Description("Control template"),
    TemplateInstance(TemplateInstance.Single),
    TemplateContainer(typeof(CaptchaTemplateControl))
    ]
    public virtual ITemplate Template
    {
      get
      {
        return templateValue;
      }
      set
      {
        templateValue = value;
      }
    }

    private void SaveValidationData()
    {
      HttpContext.Current.Session[CaptchaID + ":Captcha_InstanceID"] = CaptchaControl.CaptchaControl.CurrentInstanceId;
      HttpContext.Current.Session[CaptchaID + ":Captcha_CaptchaID"] = CaptchaControl.CaptchaControl.CaptchaId;

    }

    public bool IsCaptchaValid(string captchaValue)
    {
      string instanceID = HttpContext.Current.Session[CaptchaID + ":Captcha_InstanceID"] as string;
      string controlCaptchaID = HttpContext.Current.Session[CaptchaID + ":Captcha_CaptchaID"] as string;
      bool validated = BotDetect.Web.CaptchaControl.AjaxValidate(controlCaptchaID, captchaValue, instanceID);
      return validated;
    }

    public static bool IsCaptchaValid(string captchaValue, string captchaID)
    {
      string instanceID = HttpContext.Current.Session[captchaID + ":Captcha_InstanceID"] as string;
      string controlCaptchaID = HttpContext.Current.Session[captchaID + ":Captcha_CaptchaID"] as string;
      bool validated = BotDetect.Web.CaptchaControl.AjaxValidate(controlCaptchaID, captchaValue, instanceID);
      return validated;
    }

    protected override void CreateChildControls()
    {

      Controls.Clear();
      ownerValue = new TemplateOwner();

      ITemplate temp = templateValue;
      if (temp == null)
      {
        temp = new DefaultTemplate();
      }

      temp.InstantiateIn(ownerValue);

      foreach (Control currentcontrol in ownerValue.Controls)
      {
        if (currentcontrol.ID != null)
        {
          if (currentcontrol.ID.Equals(CaptchaImagePlaceHolderID, StringComparison.OrdinalIgnoreCase))
          {
            currentcontrol.Controls.Add(CaptchaControl);
          }
          else if (currentcontrol.ID.Equals(ImageReloadPlaceHolderID, StringComparison.OrdinalIgnoreCase))
          {
            currentcontrol.Controls.Add(_reloadImage);
          }
          else if (currentcontrol.ID.Equals(PlaySoundPlaceHolderID, StringComparison.OrdinalIgnoreCase))
          {
            currentcontrol.Controls.Add(_playSound);
          }
        }
      }
      this.Controls.Add(ownerValue);
      if (!string.IsNullOrEmpty(StashRenderLocation) && AutoBindEvents)
      {
        _stashLocation.Controls.Add(CaptchaScriptControl());
        this.Controls.Add(_stashLocation);
      }
      if (SaveValidationDataInSession)
      {
        SaveValidationData();
      }
      this.Controls.Add(_audioContainer);

    }

    private string GetReloadLinkID()
    {
      string _reloadLink = string.Empty;
      if (string.IsNullOrEmpty(ReloadLinkID))
      {
        _reloadLink = ReloadImage.ClientID;
      }
      else
      {
        _reloadLink = ReloadLinkID;
      }
      return _reloadLink;
    }

    private string GetPlaySoundLinkID()
    {
      string _playSoundLink = string.Empty;
      if (string.IsNullOrEmpty(PlaySoundLinkID))
      {
        _playSoundLink = PlaySoundImage.ClientID;
      }
      else
      {
        _playSoundLink = PlaySoundLinkID;
      }
      return _playSoundLink;
    }

    private bool _saveValidationDataInSession = false;
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

    private LiteralControl CaptchaScriptControl()
    {
      LiteralControl script = new LiteralControl();
      System.Text.StringBuilder scriptBuilder = new System.Text.StringBuilder(500);
      scriptBuilder.AppendLine("<script type=\"text/javascript\">");
      scriptBuilder.Append(JQueryDelim);
      scriptBuilder.AppendLine("(document).ready(function () {");
      scriptBuilder.AppendLine("var captcha" + CaptchaID + "=new AtlantisCaptcha();");
      scriptBuilder.Append("captcha" + CaptchaID + ".Init('");
      scriptBuilder.Append(CaptchaControl.CaptchaControl.CaptchaId);
      scriptBuilder.Append("', '");
      scriptBuilder.Append(CaptchaInstanceId);
      scriptBuilder.Append("','");
      scriptBuilder.Append(GetReloadLinkID());
      scriptBuilder.Append("','");
      scriptBuilder.Append(GetPlaySoundLinkID());
      scriptBuilder.Append("','");
      scriptBuilder.Append(CaptchaValueID);
      scriptBuilder.Append("','");
      scriptBuilder.Append(CaptchaValidateSelector);
      scriptBuilder.Append("','");
      scriptBuilder.Append(AutoFocusInput);
      scriptBuilder.Append("','");
      scriptBuilder.Append(AutoClearInput);
      scriptBuilder.Append("','");
      scriptBuilder.Append(AjaxAuthentication);
      if (AjaxAuthentication)
      {
        scriptBuilder.Append("',");
        scriptBuilder.Append(AjaxAuthenticationCallback);
      }
      else
      {
        scriptBuilder.Append("'");
      }
      scriptBuilder.Append(");");
      scriptBuilder.AppendLine("});");
      scriptBuilder.AppendLine("</script>");
      System.Diagnostics.Debug.WriteLine(scriptBuilder.ToString());
      return new LiteralControl(scriptBuilder.ToString());
    }

    public override void DataBind()
    {
      CreateChildControls();
      ChildControlsCreated = true;
      base.DataBind();
    }

  }

  [
  ToolboxItem(false)
  ]
  public class TemplateOwner : WebControl
  {
    public override void RenderBeginTag(HtmlTextWriter writer)
    {
      writer.Write(" ");
    }
    public override void RenderEndTag(HtmlTextWriter writer)
    {
      writer.Write(" ");
    }

    protected override HtmlTextWriterTag TagKey
    {
      get
      {
        return HtmlTextWriterTag.Div;
      }
    }
  }

  #region DefaultTemplate
  sealed class DefaultTemplate : ITemplate
  {
    void ITemplate.InstantiateIn(Control owner)
    {
      Label title = new Label();

      LiteralControl linebreak = new LiteralControl("<br/>");

      Label caption = new Label();

    }

  }
  #endregion

}
