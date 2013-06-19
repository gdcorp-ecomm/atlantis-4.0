using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Util;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Reflection;
using System.Globalization;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing.Design;

using BotDetect.Configuration;
using BotDetect.Persistence;

using BotDetect.CaptchaCode;
using BotDetect.CaptchaImage;
using BotDetect.CaptchaSound;

namespace BotDetect.Web.UI
{
  /// <summary>
  /// The ASP.NET Custom Web Control used to add Captcha protection to 
  /// ASP.NET Web Forms applications.
  /// </summary>
  [DefaultProperty("CodeLength"),
    RefreshProperties(RefreshProperties.All),
    Designer(typeof(CaptchaDesigner)),
    ToolboxData("<{0}:Captcha runat=server></{0}:Captcha>")]
  [System.Drawing.ToolboxBitmap(typeof(Captcha))]
  public class Captcha : WebControl
  {
    public Captcha()
    {
      captchaControl = new CaptchaControl("DesignerInitializedCaptcha");

      // use relative urls in Web Forms by default
      captchaControl.Urls = CaptchaUrls.Relative;
    }

    public Captcha(string captchaId) 
    {
      captchaControl = new CaptchaControl(captchaId);

      // use relative urls in Web Forms by default
      captchaControl.Urls = CaptchaUrls.Relative;
    }


    private CaptchaControl captchaControl;

    /// <summary>
    /// Underlying CaptchaControl instance used by the current Captcha instance. 
    /// The Web Forms Captcha control delegates much of it's Captcha 
    /// functionality to the abstract CaptchaControl instance
    /// </summary>
    public CaptchaControl CaptchaControl
    {
      get
      {
        return captchaControl;
      }
    }

    /// <summary>
    /// Rendering helper
    /// </summary>
    public static bool IsDesignMode = HttpContext.Current == null;




    #region CaptchaControl field delegation

    /// <summary>
    /// Unique identifier of the Captcha control within the application 
    /// (for example, if you placed one Captcha control on the Registration 
    /// page and another on the Contact Us, they would have distinct 
    /// CaptchaId values)
    /// </summary>
    public string CaptchaId
    {
      get
      {
        return captchaControl.CaptchaId;
      }
    }

    /// <summary>
    /// Locale string, affects the character set used for Captcha code 
    /// generation and the pronunciation language used for Captcha sound 
    /// generation
    /// </summary>
    [Category("BotDetect"),
    Description("Locale string, affects the character set used for Captcha code generation and the pronunciation language used for Captcha sound generation")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public string Locale
    {
      get
      {
        return captchaControl.Locale;
      }
      set
      {
        captchaControl.Locale = value;
      }
    }

    /// <summary>
    /// Length (number of characters) of the code rendered; the default value is 5.
    /// </summary>
    [Category("BotDetect"),
    Description("Length (number of characters) of the code rendered. The default value is 5.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public int CodeLength
    {
      get
      {
        return captchaControl.CodeLength;
      }
      set
      {
        captchaControl.CodeLength = value;
      }
    }

    /// <summary>
    /// Code style, i.e. the algorithm used to generate Captcha codes; 
    /// the default value is Alphanumeric
    /// </summary>
    [Category("BotDetect"),
      Description("Code style, i.e. the algorithm used to generate Captcha codes; the default value is Alphanumeric")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public CodeStyle CodeStyle
    {
      get
      {
        return captchaControl.CodeStyle;
      }
      set
      {
        captchaControl.CodeStyle = value;
      }
    }

    /// <summary>
    /// Optional name of the user-defined character set used for Captcha 
    /// code generation. A collection of custom character sets can be 
    /// defined in the <botDetect> section of the web.config file
    /// </summary>
    [Category("BotDetect"),
      Description("Optional name of the user-defined character set used for Captcha code generation. A collection of custom character sets can be defined in the <botDetect> section of the web.config file")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public string CustomCharacterSetName
    {
      get
      {
        return captchaControl.CustomCharacterSetName;
      }
      set
      {
        captchaControl.CustomCharacterSetName = value;
      }
    }

    /// <summary>
    /// Image style, i.e. the algorithm used to render Captcha codes in 
    /// images; if no ImageStyle is set, it is randomized by default
    /// </summary>
    [Category("BotDetect"),
      Description("Image style, i.e. the algorithm used to render Captcha codes in images; if no ImageStyle is set, it is randomized by default")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public ImageStyle ImageStyle
    {
      get
      {
        return captchaControl.ImageStyle;
      }
      set
      {
        captchaControl.ImageStyle = value;
      }
    }

    /// <summary>
    /// Image format in which the Captcha image will be rendered; the default format is JPEG
    /// </summary>
    [Category("BotDetect"),
      Description("Image format in which the Captcha image will be rendered; the default format is JPEG")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public BotDetect.ImageFormat ImageFormat
    {
      get
      {
        return captchaControl.ImageFormat;
      }
      set
      {
        captchaControl.ImageFormat = value;
      }
    }

    /// <summary>
    /// Size of the Captcha image rendered; the default size is (250, 50)
    /// </summary>
    [Category("BotDetect"),
   Description("Size of the Captcha image rendered; the default size is (250, 50)")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public System.Drawing.Size ImageSize
    {
      get
      {
        return captchaControl.ImageSize;
      }
      set
      {
        captchaControl.ImageSize = value;
      }
    }

    /// <summary>
    /// Optional custom light color point, modifies the color palette used for Captcha image drawing
    /// </summary>
    [Category("BotDetect"),
      Description("Optional custom light color point, modifies the color palette used for Captcha image drawing")]
    [DefaultValue(typeof(System.Drawing.Color), ""), TypeConverter(typeof(WebColorConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public System.Drawing.Color CustomLightColor
    {
      get
      {
        return captchaControl.CustomLightColor;
      }
      set
      {
        captchaControl.CustomLightColor = value;
      }
    }

    /// <summary>
    /// Optional custom dark color point, modifies the color palette used for Captcha image drawing
    /// </summary>
    [Category("BotDetect"),
      Description("Optional custom dark color point, modifies the color palette used for Captcha image drawing")]
    [DefaultValue(typeof(System.Drawing.Color), ""), TypeConverter(typeof(WebColorConverter))]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public System.Drawing.Color CustomDarkColor
    {
      get
      {
        return captchaControl.CustomDarkColor;
      }
      set
      {
        captchaControl.CustomDarkColor = value;
      }
    }


    /// <summary>
    /// Sound style, i.e. the algorithm used to pronounce Captcha codes 
    /// in sounds; if no SoundStyle is set, it is randomized by default
    /// </summary>
    [Category("BotDetect"),
      Description("Sound style, i.e. the algorithm used to pronounce Captcha codes in sounds; if no SoundStyle is set, it is randomized by default")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public SoundStyle SoundStyle
    {
      get
      {
        return captchaControl.SoundStyle;
      }
      set
      {
        captchaControl.SoundStyle = value;
      }
    }

    /// <summary>
    /// Audio format in which the Captcha sound will be generated; 
    /// the default format is WawPcm16bit8kHzMono
    /// </summary>
    [Category("BotDetect"),
   Description("	Audio format in which the Captcha sound will be generated; the default format is WawPcm16bit8kHzMono")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public SoundFormat SoundFormat
    {
      get
      {
        return captchaControl.SoundFormat;
      }
      set
      {
        captchaControl.SoundFormat = value;
      }
    }

    /// <summary>
    /// User input textbox client-side identifier (the ASP.NET ClientID 
    /// property), used for all client-side user input processing, e.g. 
    /// automatic user input uppercasing and focusing
    /// </summary>
    [Category("BotDetect"),
     Description("User input textbox client-side identifier (the ASP.NET ClientID property), used for all client-side user input processing, e.g. automatic user input uppercasing and focusing")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public string UserInputClientID
    {
      get
      {
        return captchaControl.UserInputClientID;
      }

      set
      {
        captchaControl.UserInputClientID = value;
      }
    }

    /// <summary>
    /// Override Captcha icon layout, always use small default icons
    /// </summary>
    [Category("BotDetect"),
     Description("Override Captcha icon layout, always use small default icons")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public bool UseSmallIcons
    {
      get
      {
        return captchaControl.UseSmallIcons;
      }

      set
      {
        captchaControl.UseSmallIcons = value;
      }
    }

    /// <summary>
    /// Override Captcha icon layout, always use horizontal icons layout
    /// </summary>
    [Category("BotDetect"),
     Description("Override Captcha icon layout, always use horizontal icons layout")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public bool UseHorizontalIcons
    {
      get
      {
        return captchaControl.UseHorizontalIcons;
      }

      set
      {
        captchaControl.UseHorizontalIcons = value;
      }
    }

    /// <summary>
    /// Specify custom Captcha icons div width to override the default icon layout dimensions
    /// </summary>
    [Category("BotDetect"),
     Description("Specify custom Captcha icons div width to override the default icon layout dimensions")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public int IconsDivWidth
    {
      get
      {
        return captchaControl.IconsDivWidth;
      }

      set
      {
        captchaControl.IconsDivWidth = value;
      }
    }

    /// <summary>
    /// Starting tabindex for the Captcha control Html output. There are 
    /// three keyboard-selectable Captcha markup elements: the Captcha 
    /// image help link, the Captcha sound icon and the Captcha reload
    /// icon. Depending on your settings (whether the Captcha help link
    /// is enabled, are Captcha sounds enabled, is Captcha reloading 
    /// enabled), the next available tabindex on the page can be from 0 to
    /// 3 greater than this value.
    /// </summary>
    [Category("BotDetect"),
     Description("Starting tabindex for the Captcha control Html output. There are three keyboard-selectable Captcha markup elements: the Captcha image help link, the Captcha sound icon and the Captcha reload icon. Depending on your settings (whether the Captcha help link is enabled, are Captcha sounds enabled, is Captcha reloading enabled), the next available tabindex on the page can be from 0 to 3 greater than this value.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public override short TabIndex
    {
      get
      {
        return captchaControl.TabIndex;
      }

      set
      {
        captchaControl.TabIndex = value;
      }
    }

    /// <summary>
    /// The Captcha image tooltip can be set for the entire application using web.config settings; 
    /// this instance property allows overriding those values dynamically.
    /// </summary>
    [Category("BotDetect"),
    Description("Image alt text used for Captcha images")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public string CaptchaImageTooltip
    {
      get
      {
        return captchaControl.CaptchaImageTooltip;
      }

      set
      {
        captchaControl.CaptchaImageTooltip = value;
      }
    }


    /// <summary>
    /// The Captcha reload icon tooltip can be set for the entire application using web.config settings; 
    /// this instance property allows overriding those values dynamically.
    /// </summary>
    [Category("BotDetect"),
     Description("Link title and image alt text used for the Captcha Reload icon ")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public string ReloadIconTooltip
    {
      get
      {
        return captchaControl.ReloadIconTooltip;
      }

      set
      {
        captchaControl.ReloadIconTooltip = value;
      }
    }


    /// <summary>
    /// The Captcha sound icon tooltip can be set for the entire application using web.config settings; 
    /// this instance property allows overriding those values dynamically.
    /// </summary>
    [Category("BotDetect"),
     Description("Link title and image alt text used for the Captcha Sound icon ")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PersistenceMode(PersistenceMode.Attribute)]
    public string SoundIconTooltip
    {
      get
      {
        return captchaControl.SoundIconTooltip;
      }

      set
      {
        captchaControl.SoundIconTooltip = value;
      }
    }


    /// <summary>
    /// Optional global list of sequences which will be filtered out of 
    /// the randomly generated Captcha codes, used for example for 
    /// swear-word filtering
    /// </summary>
    public static List<string> BannedSequences
    {
      get
      {
        return CaptchaControl.BannedSequences;
      }
      set
      {
        CaptchaControl.BannedSequences = value;
      }
    }

    // captcha instance events

    /// <summary>
    /// Custom event handler delegate for InitializedCaptchaControl events, 
    /// most commonly used for Captcha property randomization
    /// </summary>
    public event EventHandler<InitializedCaptchaControlEventArgs> InitializedCaptchaControl;

    /// <summary>
    /// Custom event handler delegate for GeneratingCaptchaCode events, 
    /// which occur before each Captcha code is generated
    /// </summary>
    public event EventHandler<GeneratingCaptchaCodeEventArgs> GeneratingCaptchaCode;

    /// <summary>
    /// Custom event handler delegate for GeneratedCaptchaCode events, 
    /// which occur after each Captcha code has been generated
    /// </summary>
    public event EventHandler<GeneratedCaptchaCodeEventArgs> GeneratedCaptchaCode;

    /// <summary>
    /// Custom event handler delegate for GeneratingCaptchaImage events, 
    /// which occur before each Captcha image is generated
    /// </summary>
    public event EventHandler<GeneratingCaptchaImageEventArgs> GeneratingCaptchaImage;

    /// <summary>
    /// Custom event handler delegate for GeneratedCaptchaImage events, 
    /// which occur after each Captcha image has been generated
    /// </summary>
    public event EventHandler<GeneratedCaptchaImageEventArgs> GeneratedCaptchaImage;

    /// <summary>
    /// Custom event handler delegate for GeneratingCaptchaSound events, 
    /// which occur before each Captcha sound is generated
    /// </summary>
    public event EventHandler<GeneratingCaptchaSoundEventArgs> GeneratingCaptchaSound;

    /// <summary>
    /// Custom event handler delegate for GeneratedCaptchaSound events, 
    /// which occur after each Captcha sound has been generated
    /// </summary>
    public event EventHandler<GeneratedCaptchaSoundEventArgs> GeneratedCaptchaSound;

    /// <summary>
    /// Custom event handler delegate for ValidatingUserInput events, 
    /// which occur before each user input is generated
    /// </summary>
    public event EventHandler<ValidatingUserInputEventArgs> ValidatingUserInput;

    /// <summary>
    /// Custom event handler delegate for ValidatedUserInput events, 
    /// which occur after each user input has been generated
    /// </summary>
    public event EventHandler<ValidatedUserInputEventArgs> ValidatedUserInput;

    /// <summary>
    /// All events are kept in the global CaptchaBase or CaptchaControl delegates, 
    /// but we expose Captcha control instance equivalents for easier usage, 
    /// and forward them to global ones when changed
    /// </summary>
    protected void ForwardInstanceHandlersToGlobal()
    {
      CaptchaControl.RegisterInitializedCaptchaControlHandler(captchaControl.CaptchaId, this.InitializedCaptchaControl);

      CaptchaBase.RegisterGeneratingCaptchaCodeHandler(captchaControl.CaptchaId, this.GeneratingCaptchaCode);
      CaptchaBase.RegisterGeneratedCaptchaCodeHandler(captchaControl.CaptchaId, this.GeneratedCaptchaCode);

      CaptchaBase.RegisterGeneratingCaptchaImageHandler(captchaControl.CaptchaId, this.GeneratingCaptchaImage);
      CaptchaBase.RegisterGeneratedCaptchaImageHandler(captchaControl.CaptchaId, this.GeneratedCaptchaImage);

      CaptchaBase.RegisterGeneratingCaptchaSoundHandler(captchaControl.CaptchaId, this.GeneratingCaptchaSound);
      CaptchaBase.RegisterGeneratedCaptchaSoundHandler(captchaControl.CaptchaId, this.GeneratedCaptchaSound);

      CaptchaBase.RegisterValidatingUserInputHandler(captchaControl.CaptchaId, this.ValidatingUserInput);
      CaptchaBase.RegisterValidatedUserInputHandler(captchaControl.CaptchaId, this.ValidatedUserInput);
    }

    #endregion CaptchaControl field delegation

    protected override void CreateChildControls()
    {
      base.CreateChildControls();

      InitializePageHeader();
    }

    protected void InitializePageHeader()
    {
      try
      {
        
      }
      catch (HttpException ex)
      {
        // "The Controls collection cannot be modified because the control contains code blocks (i.e. <% ... %>)."

        // We're trying to add link controls dynamically, which fails when the <head> element is constructed
        // as a pure string and doesn't initialize the Controls collection at all.

        // We fall back to adding the stylesheet in the body which works, even if it produces invalid XHTML;
        // if customers want valid XHTML, they should ensure an appropriate header is constructed.

        CaptchaLogging.Trace("Warning", "Invalid Page.Header", ex);
      }
    }

    private bool _layoutCssAdded;

    /// <summary>
    /// Add the stylesheet link in the Page header if it exists, and
    /// remember if it needs to be added later otherwise
    /// </summary>
    protected void IncludeStyleSheets()
    {
      if (null != this.Page.Header)
      {
        int includeIndex = IncludeMainStyleSheet();

        // set success flag, used when rendering the control
        _layoutCssAdded = true;
      }
    }

    protected int IncludeMainStyleSheet()
    {
      // construct link element
      HtmlLink cssLink = new HtmlLink();
      cssLink.Href = CaptchaUrls.LayoutStyleSheetUrl;
      cssLink.Attributes.Add("rel", "stylesheet");
      cssLink.Attributes.Add("type", "text/css");

      // check that the stylesheet is not already included
      bool alreadyAdded = false;
      foreach (Control c in this.Page.Header.Controls)
      {
        // check for <link> elements with the same href
        HtmlLink test = c as HtmlLink;
        if ((null != test) &&
            (0 == String.Compare(test.Href, cssLink.Href,
                StringComparison.InvariantCultureIgnoreCase)))
        {
          alreadyAdded = true;
          break;
        }
      }

      // find first <style> or <link> element position
      int firstPossibleOverrideIndex = this.Page.Header.Controls.Count;
      foreach (Control c in this.Page.Header.Controls)
      {
        // check for <style> and <link> elements 
        if (IsStyleOrLinkElement(c))
        {
          int currentIndex = this.Page.Header.Controls.IndexOf(c);
          if (currentIndex < firstPossibleOverrideIndex)
          {
            firstPossibleOverrideIndex = currentIndex;
          }
        }
      }

      // the stylesheet should be included after title & meta tags
      // etc., but before any <style> or <link> elements 
      // (so stylesheet overrides via these elements are possible)
      if (!alreadyAdded)
      {
        this.Page.Header.Controls.AddAt(firstPossibleOverrideIndex, cssLink);
        return firstPossibleOverrideIndex;
      }

      return -1;
    }

    /// <summary>
    /// Check for "link" and "style" elements
    /// </summary>
    protected static bool IsStyleOrLinkElement(Control c)
    {
      if (null == c) { return false; }

      if (c is HtmlLink)
      {
        return true;
      }

      LiteralControl literal = c as LiteralControl;
      if (null != literal &&
          StringHelper.HasValue(literal.Text) &&
          literal.Text.Contains(@"<style"))
      {
        return true;
      }

      return false;
    }


    protected override void OnInit(System.EventArgs e)
    {
      base.OnInit(e);

      if (!IsDesignMode)
      {
        // determine the Captcha identifier
        string captchaId = this.AppRelativeCaptchaId;

        // load any saved values
        CaptchaPersistence.Load(captchaControl, captchaId);

        // the control doesn't use viewstate
        this.EnableViewState = false;
      }
    }

    /// <summary>
    /// Each Captcha in the application has a unique id, depending on which 
    /// page it is on and how it is called
    /// </summary>
    protected string AppRelativeCaptchaId
    {
      get
      {
        string captchaId = "Captcha1";

        try
        {
          // use the relative page path...
          string pagePath = this.Page.AppRelativeVirtualPath.ToLowerInvariant();
          pagePath = pagePath.Replace("~/", "");
          pagePath = pagePath.Replace('/', '_');
          pagePath = pagePath.Replace(".aspx", "_");

          // ...combined with the control id...
          captchaId = "c_" + pagePath + this.ClientID.ToLowerInvariant();

          // ...but avoid using non-alphanumeric characters (except underscore) and very long strings...
          captchaId = CaptchaDefaults.DisallowedCharacters.Replace(captchaId, "");

          if (255 < captchaId.Length)
          {
            captchaId = captchaId.Substring(captchaId.Length - 255);
          }

          // ...and keep it all Url-safe and Session key -safe
          captchaId = HttpUtility.UrlEncode(captchaId);
        }
        catch (Exception ex)
        {
          // ignore unusable values 
          Debug.Assert(false, ex.Message);
        }

        return captchaId;
      }
    }

    protected override void OnLoad(System.EventArgs e)
    {
      base.OnLoad(e);
    }

    protected override void OnPreRender(System.EventArgs e)
    {
      base.OnPreRender(e);

      if (!IsDesignMode)
      {
        // save all customized control values
        CaptchaPersistence.Save(captchaControl);

        // event handler propagation
        ForwardInstanceHandlersToGlobal();

        // we move control initialization to after state saving,
        // so custom CaptchaControlInitialized handlers registered in
        // the Page_Init properly affect control state and their
        // effects are not persisted in Session state
        captchaControl.Initialize();
      }
    }

    /// <summary> 
    /// Render this control to the output parameter specified.
    /// </summary>
    /// <param name="output"> The HTML writer to write out to </param>
    protected override void Render(HtmlTextWriter writer)
    {
      if (!IsDesignMode)
      {
        RenderXhtml11Strict(writer);
      }
    }

    protected short activeTabIndex;

    public void RenderXhtml11Strict(HtmlTextWriter writer)
    {
      writer.WriteLine();

      RenderWarnings(writer);

      writer.WriteLine("  <div class=\"LBD_CaptchaDiv {4}\" id=\"{0}_CaptchaDiv\" style=\"width: {1}px; height: {2}px; {3}\">",
          captchaControl.CaptchaId, captchaControl.TotalWidth, captchaControl.TotalHeight, this.Style.Value, this.CssClass);

      RenderCaptchaImageMarkup(writer);
      RenderCaptchaIcons(writer);

      RenderHiddenFields(writer);

      writer.WriteLine("  </div>");
      writer.WriteLine();
    }

    protected void RenderWarnings(HtmlTextWriter writer)
    {
      RenderTestModeWarning(writer);
      RenderHttpHandlerWarning(writer);
      RenderSessionWarnings(writer);
    }

    /// <summary>
    /// We want to prevent accidental TestMode Captcha deployment to production websites
    /// </summary>
    protected void RenderTestModeWarning(HtmlTextWriter writer)
    {
      if (CaptchaConfiguration.CaptchaCodes.TestMode.Enabled)
      {
        writer.WriteLine("<p class=\"LBD_Warning\">Test Mode Enabled. ALWAYS disable this setting in production environments.</p>");
      }
    }

    /// <summary>
    /// Report if the HttpHandler is not properly registered
    /// </summary>
    protected void RenderHttpHandlerWarning(HtmlTextWriter writer)
    {
      writer.WriteLine(CaptchaHandler.HtmlReport());
    }

    /// <summary>
    /// Report Page-context Session state errors
    /// </summary>
    protected void RenderSessionWarnings(HtmlTextWriter writer)
    {
      writer.WriteLine(SessionTroubleshooting.HtmlReport(captchaControl));
    }

    /// <summary>
    /// Captcha image + help link
    /// </summary>
    protected void RenderCaptchaImageMarkup(HtmlTextWriter writer)
    {
      writer.WriteLine(" <div class=\"LBD_CaptchaImageDiv\" id=\"{0}_CaptchaImageDiv\" style=\"width: {1}px; height: {2}px;\">",
          captchaControl.CaptchaId, captchaControl.ImageSize.Width, captchaControl.ImageSize.Height);


      if (!captchaControl.HelpLinkEnabled)
      {
        RenderPlainImage(writer);
      }
      else
      {
        switch (captchaControl.HelpLinkMode)
        {
          case HelpLinkMode.Image:
            RenderLinkedImage(writer);
            break;

          case HelpLinkMode.Text:
            RenderPlainImageWithTextLink(writer);
            break;
        }
      }

	  writer.WriteLine(" </div>");      
    }



    protected void RenderPlainImage(HtmlTextWriter writer)
    {
      // plain image
      writer.WriteLine("   <img class=\"LBD_CaptchaImage\" id=\"{0}\" src=\"{1}\" alt=\"{2}\" />",
          captchaControl.ImageClientId, captchaControl.CaptchaImageUrl, captchaControl.CaptchaImageTooltip);
    }


    protected void RenderLinkedImage(HtmlTextWriter writer)
    {
      //image link to configured help page
      if (captchaControl.IsTabIndexSet)
      {
        writer.WriteLine("   <a target=\"_blank\" href=\"{3}\" title=\"{6}\" tabindex=\"{5}\" onclick=\"{4}.OnHelpLinkClick(); return {4}.FollowHelpLink;\"><img class=\"LBD_CaptchaImage\" id=\"{0}\" src=\"{1}\" alt=\"{2}\" /></a>",
            captchaControl.ImageClientId, captchaControl.CaptchaImageUrl, captchaControl.CaptchaImageTooltip, captchaControl.HelpPage, captchaControl.CaptchaId, captchaControl.TabIndex, captchaControl.HelpLinkText);

        captchaControl.TabIndex++;
      }
      else
      {
        writer.WriteLine("   <a target=\"_blank\" href=\"{3}\" title=\"{6}\" onclick=\"{4}.OnHelpLinkClick(); return {4}.FollowHelpLink;\"><img class=\"LBD_CaptchaImage\" id=\"{0}\" src=\"{1}\" alt=\"{2}\" /></a>",
            captchaControl.ImageClientId, captchaControl.CaptchaImageUrl, captchaControl.CaptchaImageTooltip, captchaControl.HelpPage, captchaControl.CaptchaId, captchaControl.TabIndex, captchaControl.HelpLinkText);
      }
    }

    protected void RenderPlainImageWithTextLink(HtmlTextWriter writer)
    {
      // image wrapped in an extra div 
      writer.WriteLine("   <div class=\"LBD_CaptchaImageDiv\" style=\"width:{3}px; height:{4}px;\"><img class=\"LBD_CaptchaImage\" id=\"{0}\" src=\"{1}\" alt=\"{2}\" /></div>",
          captchaControl.ImageClientId, captchaControl.CaptchaImageUrl, captchaControl.CaptchaImageTooltip, captchaControl.ImageSize.Width, captchaControl.AdjustedHeight, captchaControl.CaptchaId);

      // + help link
      if (captchaControl.IsTabIndexSet)
      {
        writer.WriteLine("   <a href=\"{0}\" target=\"_blank\" title=\"{1}\" tabindex=\"{4}\" style=\"display: block !important; height: {2}px !important; margin: 0 !important; padding: 0 !important; font-size: {3}px !important; line-height: {2}px !important; visibility: visible !important; font-family: Verdana, DejaVu Sans, Bitstream Vera Sans, Verdana Ref, sans-serif !important; vertical-align: middle !important; text-align: center !important; text-decoration: none !important; background-color: #f8f8f8 !important; color: #606060 !important;\">{1}</a>",
            captchaControl.HelpPage, captchaControl.HelpLinkText, captchaControl.HelpLinkHeight, captchaControl.HelpLinkFontSize, captchaControl.TabIndex);

        captchaControl.TabIndex++;
      }
      else
      {
        writer.WriteLine("   <a href=\"{0}\" target=\"_blank\" title=\"{1}\" style=\"display: block !important; height: {2}px !important; margin: 0 !important; padding: 0 !important; font-size: {3}px !important; line-height: {2}px !important; visibility: visible !important; font-family: Verdana, DejaVu Sans, Bitstream Vera Sans, Verdana Ref, sans-serif !important; vertical-align: middle !important; text-align: center !important; text-decoration: none !important; background-color: #f8f8f8 !important; color: #606060 !important;\">{1}</a>",
            captchaControl.HelpPage, captchaControl.HelpLinkText, captchaControl.HelpLinkHeight, captchaControl.HelpLinkFontSize);
      }
    }


    /// <summary>
    /// Sound and Reload icons
    /// </summary>
    protected void RenderCaptchaIcons(HtmlTextWriter writer)
    {
      if (captchaControl.RenderIcons)
      {

        // reload icon
        if (captchaControl.ReloadIconEnabled)
        {
          RenderHiddenElements(writer, "_ReloadLink", captchaControl.ReloadIconUrl);        
        }

        // sound icon
        if (captchaControl.CaptchaSoundEnabled)
        {
          if (captchaControl.CaptchaSoundAvailable)
          {
            RenderHiddenElements(writer, "_SoundLink", captchaControl.CaptchaSoundUrl);            
          }
        }

        // invisible sound placeholder element
        if (captchaControl.CaptchaSoundEnabled)
        {
          writer.WriteLine("   <div style=\"display:none\" class=\"LBD_Placeholder\" id=\"{0}\">&nbsp;</div>", captchaControl.AudioPlaceholderClientId);
        }
      }
    }

    private void RenderHiddenElements(HtmlTextWriter writer, string controlSuffix, string href) //GODADDY OVERRIDE
    {
      string controlToWrite = string.Format("<a style=\"display:none\" id=\"{0}{1}\" href=\"{2}\"></a>", captchaControl.CaptchaId, controlSuffix, href);
      writer.WriteLine(controlToWrite);
    }
    protected void RenderScriptIncludes(HtmlTextWriter writer)
    {
      // we need the script include for sound & reload icons, and any 
      // client-side user input processing
      if (!(captchaControl.ReloadIconEnabled ||
          captchaControl.CaptchaSoundEnabled ||
          (null != captchaControl.UserInputClientID &&
              (captchaControl.AutoFocusInput ||
               captchaControl.AutoClearInput ||
               captchaControl.AutoUppercaseInput)
         ))
      )
      {
        return;
      }

      // client-side BotDetect library include and initialization
      writer.WriteLine(captchaControl.ClientScriptIncludeFragment);
      writer.WriteLine(captchaControl.ClientScriptInitializationFragment);
    }

    /// <summary>
    /// In case the <head> element is not runat="server", we can't add the Css there
    /// so we have to output it in the body, even if it breaks Xhtml validity
    /// </summary>
    protected void RenderCssIncludes(HtmlTextWriter writer)
    {
      if (!_layoutCssAdded)
      {
        writer.WriteLine(HtmlHelper.StylesheetInclude(CaptchaUrls.LayoutStyleSheetUrl));
      }
    }

    /// <summary>
    /// Hidden fields must be included reliably, so we skip Page.ClientScript.RegisterClientScriptInclude etc.
    /// since those methods behave differently when the control is used in a plain WebForms page, 
    /// ASP.NET Ajax form, ASP.NET MVC application...
    /// </summary>
    protected void RenderHiddenFields(HtmlTextWriter writer)
    {
      writer.WriteLine(HtmlHelper.HiddenField(captchaControl.ValidatingInstanceKey, captchaControl.CurrentInstanceId));
    }

    /// <summary>
    /// Validates the Captcha, comparing the user input to the Captcha code 
    /// stored on the server. Each randomly generated Captcha code can only 
    /// be validated once, regardless of the validation result
    /// </summary>
    /// <param name="userInput">Captcha code submitted by the user</param>
    /// <returns>does the submitted code match the server-side stored one</returns>
    public bool Validate(string userInput)
    {
      return captchaControl.Validate(userInput);
    }

    /// <summary>
    /// BotDetect control and .NET runtime versions debug string
    /// </summary>
    public static string ControlInfo
    {
      get
      {
        string controlInfo = "";

        try
        {
          Assembly assembly = Assembly.GetExecutingAssembly();

          string title = "";
          object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
          if (null != attributes && 1 == attributes.Length)
          {
            title = ((AssemblyTitleAttribute)attributes[0]).Title;
          }

          string version = assembly.GetName().Version.ToString();

          controlInfo = String.Format(CultureInfo.InvariantCulture, "Assembly \"{0}\" version {1} loaded by .NET runtime version {2}.",
              title, version, Environment.Version.ToString(4));
        }
        catch (Exception e)
        {
          CaptchaLogging.Trace("Warning", "Unable to access control info", e);
        }

        return controlInfo;
      }
    }
  }
}
