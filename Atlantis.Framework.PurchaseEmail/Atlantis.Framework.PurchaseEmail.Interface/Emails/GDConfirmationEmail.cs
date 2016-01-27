using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Atlantis.Framework.GetPaymentProfileAlternate.Interface;
using Atlantis.Framework.EcommPaymentProfile.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MessagingProcess.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;
using Atlantis.Framework.PaymentProfileClass.Interface;
using System.Xml;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class GDConfirmationEmail : PurchaseConfirmationEmailBase
  {
    private bool _isAZHumane;
    private bool _isDevServer;
    private RequestData _requestData;

    private const string NATIONALBREASTCANCER_TYPE = "PurchasedNCBWebsite";
    private const string NATIONALBREASTCANCER_NAMESPACE = "FOS";
    private const string NATIONALBREASTCANCER_ITC_CODE = "dpp_carepkg";

    private const string AZ_HUMANE_PACKAGE_TYPE = "AZHumaneSocietyWSB";
    private const string AZ_HUMANE_PACKAGE_NAMESPACE = "FOS";
    private const string AZ_HUMANE_PACKAGE_ITC_CODE = "slp_az_humane";
    private const string AZ_HUMANE_PACKAGE_MANAGER_ITC_CODE = "mgr_slp_az_humane";

    public GDConfirmationEmail(OrderData orderData, EmailRequired emailRequired, bool isAZHumane, bool isDevServer, RequestData rd, ObjectProviderContainer objectContainer)
      : base(orderData, emailRequired, objectContainer)
    {
      _isAZHumane = isAZHumane;
      _isDevServer = isDevServer;
      _requestData = rd;
    }

    protected override void SetParams()
    {
      SetParam(EmailTokenNames.OrderId, Order.OrderId);

      if (EmailTemplate.Id == EmailTemplateType.OrderConfirmation || EmailTemplate.Id == EmailTemplateType.GDWelcome)
      {
        string xmlParam;
        using (var ms = new MemoryStream())
        {
          Order.OrderXmlDoc.Save(ms);
          xmlParam = String.Concat("<?xml version=\"1.0\" encoding=\"utf-8\"?>", Encoding.UTF8.GetString(ms.ToArray()));
          ms.Close();
        }

        SetParam(EmailTokenNames.CartXml, xmlParam, AttributeValueWriteMethod.CDataBlock);

        SetParam(EmailTokenNames.ItemsText, GetEulaBlock());
      }
      else
      {
        SetParam(EmailTokenNames.ItemsText, GetItemsText());
      }
      SetParam(EmailTokenNames.ShopperId, ShopperContext.ShopperId);
      SetParam(EmailTokenNames.OrderTotal, TotalPrice);
      SetParam(EmailTokenNames.ServerName, _isDevServer ? "www.dev.godaddy.com" : "www.godaddy.com");
      SetParam(EmailTokenNames.LoginName, LoginName);
      SetParam(EmailTokenNames.CC_Address, string.Empty);
      SetParam(EmailTokenNames.OrderTime, OrderTime);
      SetParam(EmailTokenNames.CrossSellItems, GetCrossSellItems());
      SetParam(EmailTokenNames.VATId, VATId);
      
      // TODO: this product does not exist anymore. Confirm that our email templates don't have these two tokens and remove these 2 lines.
      SetParam(EmailTokenNames.VideoMeHTMLLink, string.Empty);
      SetParam(EmailTokenNames.VideoMeTextLink, string.Empty);

      SetParam(EmailTokenNames.HostingConciergeBanner, HostingConciergeBannerHtmlGet());
      SetParam(EmailTokenNames.HostingConciergeText, HostingConciergeTextGet((IsHTMLEmail ? true : false), true));
      SetParam(EmailTokenNames.AlipayRenewalText, AliPayRenewalNotice());
    }

    private const string aliPayRenewalMessageOnError = aliPayRenewalMessage;
    private const string aliPayRenewalMessage = "<span style='color:red; font-weight:bold'>Payment Alert</span>: Alipay does not support automatic renewal of GoDaddy products. You will receive an email notification with manual renewal instructions in advance of the expiration date of any of your products.";

    private string AliPayRenewalNotice()
    {
      string notice = String.Empty;
      switch (EmailTemplate.Id)
      {
        case EmailTemplateType.GDWelcome:
        case EmailTemplateType.OrderConfirmation:
          var xmlPaymentMethod = Order.OrderXmlDoc.SelectSingleNode("/ORDER/ORDERDETAIL[@cc_type='AliPay']");
          if (xmlPaymentMethod == null)
          {
            break;
          }

          // See if there are any domains purchased w/autorenew enabled
          int itemsWithAutoRenewal = Order.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM/CUSTOMXML/domainBulkRegistration/domain[@autorenewflag=1]").Count;
          if (itemsWithAutoRenewal == 0)
          {
            break;
          }

          try
          {
            // Customer has no non-Alipay backup payment method available
            var reqAltPaymentProfile = new GetPaymentProfileAlternateRequestData(ShopperContext.ShopperId, _requestData.SourceURL, Order.OrderId, Order.Pathway, Order.PageCount);
            var rspAltPaymentProfile = (GetPaymentProfileAlternateResponseData)Engine.Engine.ProcessRequest(reqAltPaymentProfile, PurchaseEmailEngineRequests.PaymentProfileAlternateId);
            if (!rspAltPaymentProfile.IsSuccess)
            {
              AtlantisException aex = new AtlantisException(_requestData, "GDConfirmationEmail.AliPayRenewalNotice", "GetPaymentProfileAlternateResponseData.IsSuccess<>true", String.Empty);
              Engine.Engine.LogAtlantisException(aex);
              notice = aliPayRenewalMessageOnError;
              break;
            }

            if (rspAltPaymentProfile.PaymentProfileId <= 0)
            {
              // no backup payment method... so the renewal will fail
              notice = aliPayRenewalMessage;
              break;
            }

            // get all of our payment profiles
            EcommPaymentProfileRequestData reqPaymentProfiles = new EcommPaymentProfileRequestData(ShopperContext.ShopperId, _requestData.SourceURL, Order.OrderId, Order.Pathway, Order.PageCount, rspAltPaymentProfile.PaymentProfileId);
            EcommPaymentProfileResponseData rspPaymentProfile = (EcommPaymentProfileResponseData)Engine.Engine.ProcessRequest(reqPaymentProfiles, PurchaseEmailEngineRequests.PaymentProfiles);
            if (!rspPaymentProfile.IsSuccess)
            {
              AtlantisException aex = new AtlantisException(_requestData, "GDConfirmationEmail.AliPayRenewalNotice", "GetPaymentProfilesResponseData.IsSuccess<>true", String.Empty);
              Engine.Engine.LogAtlantisException(aex);
              notice = aliPayRenewalMessageOnError;
              break;
            }

            //PaymentProfile altPaymentProfile rspPaymentProfile.AccessProfile(ShopperContext.ShopperId, Order.ManagerUserId, Order.ManagerUserName, "AlipayRenewalNotice");
            // See if the Alternate Payment Profile is not alipay... otherwise send the message

            PaymentProfile altPaymentProfile = rspPaymentProfile.AccessProfile(ShopperContext.ShopperId, Order.ManagerUserId, Order.ManagerUserName, "AlipayRenewalNotice");
            if (altPaymentProfile.ProfileType.Equals("alipay", StringComparison.OrdinalIgnoreCase))
            {
              // backup payment method is also alipay... so renewal will fail
              notice = aliPayRenewalMessage;
              break;
            }
          }
          catch (Exception ex)
          {
            AtlantisException aex = new AtlantisException(_requestData, "GDConfirmationEmail.AliPayRenewalNotice", "Exception caught. ", String.Empty, ex);
            Engine.Engine.LogAtlantisException(aex);
            notice = aliPayRenewalMessageOnError;
          }
          break;

        default:
          break;
      }
      return notice;
    }

    private string HostingConciergeBannerHtmlGet()
    {
      string hostPhoneUsa;
      string hostPhoneIntl;
      HostPhoneUsaIntlGet(out hostPhoneUsa, out hostPhoneIntl);
      StringBuilder banner = new StringBuilder();

      if (HostingConciergeEnabled && !String.IsNullOrEmpty(hostPhoneIntl) && !String.IsNullOrEmpty(hostPhoneUsa))
      {

        int hostingHeight = 104;
        int hostingWidth = 196;
        string hostingImage;
        switch (EmailTemplate.Id)
        {
          case EmailTemplateType.GDWelcome:
            hostingImage = Links.GetUrl(LinkTypes.Image, "promos/htmlemails/bbtemplate/52345_hosting_01c.jpg",
                                       QueryParamMode.ExplicitParameters, false);
            hostingHeight = 106;
            hostingWidth = 182;
            break;
          default:
            hostingImage = Links.GetUrl(LinkTypes.Image, "promos/htmlemails/bbtemplate/52345_hosting_01b.jpg",
                                       QueryParamMode.ExplicitParameters, false);
            break;
        }
        banner.Append(
            "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" bgcolor=\"white\" width=\"" + hostingWidth + "\" style=\"border: 1px solid black;\">");
        banner.Append(
            "<tbody><tr> <td><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"> <tbody><tr> ");
        banner.Append("<td align=\"center\"><img height=\"" + hostingHeight);
        banner.Append("\" border=\"0\" width=\"" + hostingWidth + "\" src=\"");
        banner.Append(hostingImage + "\" alt=\"[%%LCST.REQ.UTOS_CONCIERGE%%]\"></a>");
        banner.Append("</td> </tr> <tr> <td align=\"left\" style=\"padding:3px 5px 5px 5px;\">");
        banner.Append(
            "<font style=\"font-size: 11px; color: black; font-family: arial,sans serif;\">");
        banner.Append(
            "Call <span style='font-size: 12px; font-weight:bold'>" + hostPhoneUsa + "</span> (US only) or <span style='font-size: 12px; font-weight:bold'>" + hostPhoneIntl + "</span> for the next 30 days.</font></td> </tr> </tbody></table>");
        string transparentImage = Links.GetUrl(LinkTypes.Image, "promos/std/spc_trans.gif", QueryParamMode.CommonParameters, false);
        banner.Append("</td> </tr> </tbody></table><img height=\"12\" border=\"0\" src=\"" + transparentImage +
                      "\"><br>");
      }
      return banner.ToString();
    }


    private EmailTemplate _emailTemplate;
    protected override EmailTemplate EmailTemplate
    {
      get
      {
        return _emailTemplate;
      }
    }

    private EmailTemplate DetermineFirstEmailTemplate()
    {
      EmailTemplate temp;

      if (Order.IsRefund)
      {
        if (IsFraudRefund())
        {
          temp = EmailTemplates[EmailTemplateType.CustomerAccountLocked];
        }
        else
        {
          temp = EmailTemplates[EmailTemplateType.RefundConfirmation];
        }
      }
      else
      {
        if (DoRecurringHostingExists())
        {
          if (IsNewShopper)
          {
            temp = EmailTemplates[EmailTemplateType.GDWelcome];
          }
          else
          {
            temp = EmailTemplates[EmailTemplateType.OrderConfirmation];
          }

        }
        else
        {
          if (EmailRequired.ProcessFee && EmailRequired.OtherProductIdsExist)
          {
            if (IsShopperFirstOrder)
            {
              temp = EmailTemplates[EmailTemplateType.GDWelcome];
            }
            else
            {
              temp = EmailTemplates[EmailTemplateType.OrderConfirmation];
            }
          }
          else
          {
            if (EmailRequired.ProcessFee && !EmailRequired.OtherProductIdsExist)
            {
              temp = EmailTemplates[EmailTemplateType.MiscFeesConfirmation];
            }
            else
            {
              if (IsShopperFirstOrder)
              {
                temp = EmailTemplates[EmailTemplateType.GDWelcome];
              }
              else
              {
                temp = EmailTemplates[EmailTemplateType.OrderConfirmation];
              }
            }
          }
        }

      }

      return temp;
    }

    public override List<MessagingProcessRequestData> GetMessageRequests()
    {
      List<MessagingProcessRequestData> result = new List<MessagingProcessRequestData>(2);
      if (Order.PrivateLabelId == 1)
      {
        _emailTemplate = DetermineFirstEmailTemplate();
        result.AddRange(base.GetMessageRequests());
        if (IsITCInOrder(NATIONALBREASTCANCER_ITC_CODE))
        {
          result.Add(CreateNationalBreastCancerFoundationRequest());
        }
        if (IsITCInOrder(AZ_HUMANE_PACKAGE_ITC_CODE) || IsITCInOrder(AZ_HUMANE_PACKAGE_MANAGER_ITC_CODE))
        {
          result.Add(CreateAZHumanePackageRequest());
        }
        if (_isAZHumane)
        {
          _emailTemplate = EmailTemplates[EmailTemplateType.ArizonaHumaneSociety];
          result.AddRange(base.GetMessageRequests());
        }
      }

      return result;
    }

    private bool IsITCInOrder(string itcCode)
    {
      bool result = false;
      try
      {
        string xpath = string.Concat("/ORDER/ITEMS/ITEM[@itemtrackingcode='", itcCode, "']");
        if (Order.OrderXmlDoc.SelectNodes(xpath).Count > 0)
        {
          result = true;
        }
      }
      catch (System.Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
      return result;
    }

    private MessagingProcessRequestData CreateNationalBreastCancerFoundationRequest()
    {

      MessagingProcessRequestData request = new MessagingProcessRequestData(ShopperContext.ShopperId, string.Empty, Order.OrderId,
         SiteContext.Pathway, SiteContext.PageCount, SiteContext.PrivateLabelId, NATIONALBREASTCANCER_TYPE, NATIONALBREASTCANCER_NAMESPACE);

      ResourceItem emailParms = new ResourceItem("CartOrder", Order.OrderId);

      ContactPointItem emailContact = new ContactPointItem("ShopperContact", ContactPointTypes.Shopper);
      emailContact["MarketID"] = Order.MarketID;
      emailContact["id"] = ShopperContext.ShopperId;
      emailContact["EmailType"] = IsHTMLEmail ? "html" : "plaintext";
      emailParms.ContactPoints.Add(emailContact);
      emailParms["DOMAINNAME"] = new AttributeValue(FirstDomainInOrder()); 
      request.AddResource(emailParms);

      return request;
    }

    private MessagingProcessRequestData CreateAZHumanePackageRequest()
    {

      MessagingProcessRequestData request = new MessagingProcessRequestData(ShopperContext.ShopperId, string.Empty, Order.OrderId,
         SiteContext.Pathway, SiteContext.PageCount, SiteContext.PrivateLabelId, AZ_HUMANE_PACKAGE_TYPE, AZ_HUMANE_PACKAGE_NAMESPACE);

      ResourceItem emailParms = new ResourceItem("CartOrder", Order.OrderId);

      ContactPointItem emailContact = new ContactPointItem("ShopperContact", ContactPointTypes.Shopper);
      emailContact["MarketID"] = Order.MarketID;
      emailContact["id"] = ShopperContext.ShopperId;
      emailContact["EmailType"] = IsHTMLEmail ? "html" : "plaintext";
      emailParms.ContactPoints.Add(emailContact);
      emailParms["DOMAINNAME"] = new AttributeValue(FirstDomainInOrder());
      request.AddResource(emailParms);

      return request;
    }

    private string FirstDomainInOrder()
    {
      string domainName = string.Empty;
      try
      {
        string xpath = string.Concat("/ORDER/ITEMS/ITEM/CUSTOMXML/*/domain");
        XmlNodeList  domainList=Order.OrderXmlDoc.SelectNodes(xpath);
        if (domainList!=null && domainList.Count>0)
        {
          XmlNode firstDomain = domainList[0];
          string domainSLD = string.Empty;
          string domainTLD = string.Empty;
          XmlAttribute domainsld = firstDomain.Attributes["sld"];
          if (domainsld != null)
          {
            domainSLD = domainsld.Value;
          }
          XmlAttribute domaintld = firstDomain.Attributes["tld"];
          if (domaintld != null)
          {
            domainTLD = domaintld.Value;
          }
          domainName = string.Concat(domainSLD, ".", domainTLD);
        }        
      }
      catch (System.Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
      return domainName;
    }
  }

  internal class HostingGodaddy
  {
  }
}
