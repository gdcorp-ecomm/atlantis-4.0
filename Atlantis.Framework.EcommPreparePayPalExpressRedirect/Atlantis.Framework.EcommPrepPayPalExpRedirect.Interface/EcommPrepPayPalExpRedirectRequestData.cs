using System;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml;
using System.IO;

namespace Atlantis.Framework.EcommPrepPayPalExpRedirect.Interface
{
  public class EcommPrepPayPalExpRedirectRequestData : RequestData
  {
    public string ReturnURL { get; set; }
    public string CancelURL { get; set; }
    public string HeaderImage { get; set; }
    public string HeaderBorderColor { get; set; }
    public string HeaderBackColor { get; set; }
    public string HeaderPayFlowColor { get; set; }
    public string CartBorderColor { get; set; }
    public string LogoImage { get; set; }
    public string LocaleCode { get; set; }
    public int ProfileID { get; set; }

    public EcommPrepPayPalExpRedirectRequestData( string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
                                                  string returnUrl, string cancelUrl, string headerImage, string headerBorderColor, string headerBackColor,
                                                  string headerPayFlowColor, string cartBorderColor, string logoImage, string localeCode)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(20);
      ReturnURL = returnUrl;
      CancelURL = cancelUrl;
      HeaderImage = headerImage;
      HeaderBorderColor = headerBorderColor;
      HeaderBackColor = headerBackColor;
      HeaderPayFlowColor = headerPayFlowColor;
      CartBorderColor = cartBorderColor;
      LogoImage = logoImage;
      LocaleCode = localeCode;
    }

    public EcommPrepPayPalExpRedirectRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
                                                  string returnUrl, string cancelUrl, string headerImage, string headerBorderColor, string headerBackColor,
                                                  string headerPayFlowColor, string cartBorderColor, string logoImage, string localeCode,int profileID)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(20);
      ReturnURL = returnUrl;
      CancelURL = cancelUrl;
      HeaderImage = headerImage;
      HeaderBorderColor = headerBorderColor;
      HeaderBackColor = headerBackColor;
      HeaderPayFlowColor = headerPayFlowColor;
      CartBorderColor = cartBorderColor;
      LogoImage = logoImage;
      LocaleCode = localeCode;
      ProfileID = profileID;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommPrepPayPalExpRedirectRequestData");
    }

    public override string ToXML()
    {
      var sbRequest = new StringBuilder();
      var xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("PayPalExpressSetup");
      xtwRequest.WriteAttributeString("ReturnURL", ReturnURL);
      xtwRequest.WriteAttributeString("CancelURL", CancelURL);

      if (ProfileID != 0)
      {
        xtwRequest.WriteAttributeString("pp_shopperProfileID", ProfileID.ToString());
      }
      if (!string.IsNullOrEmpty(HeaderImage))
        xtwRequest.WriteAttributeString("cpp-header-image", HeaderImage);

      if (!string.IsNullOrEmpty(HeaderBorderColor))
        xtwRequest.WriteAttributeString("cpp-header-border-color", HeaderBorderColor);

      if (!string.IsNullOrEmpty(HeaderBackColor))
        xtwRequest.WriteAttributeString("cpp-header-back-color", HeaderBackColor);

      if (!string.IsNullOrEmpty(HeaderPayFlowColor))
        xtwRequest.WriteAttributeString("cpp-header-payflow-color", HeaderPayFlowColor);

      if (!string.IsNullOrEmpty(CartBorderColor))
        xtwRequest.WriteAttributeString("cpp-cart-border-color", CartBorderColor);

      if (!string.IsNullOrEmpty(LogoImage))
        xtwRequest.WriteAttributeString("cpp-logo-image", LogoImage);

      if (!string.IsNullOrEmpty(LocaleCode))
        xtwRequest.WriteAttributeString("LocaleCode", LocaleCode);

      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }
  }
}
