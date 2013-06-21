﻿using System;
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
    private HtmlGenericControl _loadingPlaceHolder = new HtmlGenericControl();

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

      _loadingPlaceHolder.TagName = "div";
      _loadingPlaceHolder.Attributes["style"] = "display:none;";

      _audioContainer.TagName = "div";
      _audioContainer.ID = CaptchaID + "_AudioPlaceholder";
    }

    [Description("image placeholder in the template"), Category("ChildControls")] 
    public HtmlGenericControl LoadingImagePlaceHolder
    {
      get
      {
        return _loadingPlaceHolder;
      }
    }

    [Description("reload image"), Category("ChildControls")] 
    public HtmlImage ReloadImage
    {
      get
      {
        return _reloadImage;
      }
    }
    [Description("play sound image"), Category("ChildControls")] 
    public HtmlImage PlaySoundImage
    {
      get
      {
        return _playSound;
      }
    }

    [Description("stash content"), Category("ChildControls")] 
    public StashContent StashContentControl
    {
      get
      {
        return _stashLocation;
      }
    }

    [Description("is sound enabled"), Category("Behavior")] 
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
    [Description("unique captcha instance ID"), Category("Behavior")]
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
    [Description("jquery delimiter"), Category("Behavior")]
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
    [Description("captcha instance ID - used for validation"), Category("Behavior")]
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
    [Description("BotDetect Captcha Control"), Category("ChildControls")]
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
    [Description("captcha image placeholder id"), Category("TemplatePlaceholderID")]
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
    [Description("captcha value id"), Category("Behavior")]
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
    [Description("auto focus on input"), Category("Behavior")]
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
    [Description("auto clear input"), Category("Behavior")]
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
    [Description("ajax authentication enable"), Category("AjaxBehavior")]
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
    [Description("jscript callback function on ajax authentication"), Category("AjaxBehavior")]
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
    [Description("jquery selector for ajax validators"), Category("AjaxBehavior")]
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
    [Description("placeholder in template used for reload captcha"), Category("TemplatePlaceholderID")]
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
    [Description("placeholder in template used for sound play"), Category("TemplatePlaceholderID")]
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

    private string _loadingText = string.Empty;
    [Description("text shown whil image renders"), Category("Behavior")]
    public string LoadingText
    {
      get
      {
        return _loadingText;
      }
      set
      {
        _loadingText = value;
      }
    }

    private string _loadingImagePlaceHolderID;
    [Description("placeholder of UI to be shown while image renders"), Category("TemplatePlaceholderID")]
    public string LoadingImagePlaceHolderID
    {
      get
      {
        return _loadingImagePlaceHolderID;
      }
      set
      {
        _loadingImagePlaceHolderID = value;
      }
    }

    private string _loadingImageContainerPlaceHolderID;
    [Description("loading image container placeholder id"), Category("TemplatePlaceholderID")]
    public string LoadingImageContainerPlaceHolderID
    {
      get
      {
        return _loadingImageContainerPlaceHolderID;
      }
      set
      {
        _loadingImageContainerPlaceHolderID = value;
      }
    }

    private string _stashRenderLocation;
    [Description("stash render location"), Category("Behavior")]
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

    private int _autoReloadMiliSeconds = 10000;
    [Description("auto reload time in miliseconds"), Category("Behavior")]
    public int AutoReloadMiliSeconds
    {
      get
      {
        return _autoReloadMiliSeconds;
      }
      set
      {
        if (_autoReloadMiliSeconds >= 10000)
        {
          _autoReloadMiliSeconds = value;
        }
      }
    }

    private bool _autoReloadImage = false;
    [Description("auto reload enable"), Category("Behavior")]
    public bool AutoReloadImage
    {
      get
      {
        return _autoReloadImage;
      }
      set
      {
        _autoReloadImage = value;
      }
    }

    private bool _autobindEvents = true;
    [Description("auto bind events"), Category("Behavior")]
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
    [Description("reload id"), Category("Behavior")]
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
    [Description("play sound id"), Category("Behavior")]
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
          else if (currentcontrol.ID.Equals(LoadingImageContainerPlaceHolderID, StringComparison.OrdinalIgnoreCase))
          {
            currentcontrol.Controls.Add(_loadingPlaceHolder);
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
   [Description("use session during custom validation"), Category("Behavior")]
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
      scriptBuilder.Append("',");
      if (AjaxAuthentication)
      {
        scriptBuilder.Append(AjaxAuthenticationCallback);
      }
      else
      {
        scriptBuilder.Append("''");
      }
      scriptBuilder.Append(",'");
      scriptBuilder.Append(_loadingImagePlaceHolderID);
      scriptBuilder.Append("'");
      scriptBuilder.Append(",'");
      scriptBuilder.Append(_loadingText);
      scriptBuilder.Append("'");
      scriptBuilder.Append(",'");
      scriptBuilder.Append(_autoReloadImage);
      scriptBuilder.Append("'");
      scriptBuilder.Append(",'");
      scriptBuilder.Append(_autoReloadMiliSeconds);
      scriptBuilder.Append("'");
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
