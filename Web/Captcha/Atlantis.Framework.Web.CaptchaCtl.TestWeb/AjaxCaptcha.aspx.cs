using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AjaxCaptcha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected string ValidateCaptchaURL
    {
      get
      {
        string refURL= Request.Url.Scheme + "://" + Request.Url.Authority  + VirtualPathUtility.ToAbsolute("~/ValidateCaptchaAction.aspx");
        System.Diagnostics.Debug.WriteLine(refURL);
        return refURL;
      }
    }

}