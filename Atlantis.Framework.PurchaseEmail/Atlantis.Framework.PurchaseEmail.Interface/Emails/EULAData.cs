using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class EULAData
  {
    DepartmentIds _departmentIds;
    OrderData _orderData;
    ILinkProvider _links;

    public EULAData(OrderData orderData, DepartmentIds departmentIds, ILinkProvider links)
    {
      _departmentIds = departmentIds;
      _orderData = orderData;
      _links = links;
    } 

    public EULAItem GetEULAData(EULARuleType EULARule, string isc)
    {
      const string TOPIC_RELATIVE_PATH = "topic/";
      const string ARTICLE_RELATIVE_PATH = "article/";
      const string LEGAL_RELATIVE_PATH = "agreements/ShowDoc.aspx";
      const string HELP_ARTICLE_RELATIVE_PATH = "help/article/";

      string[] queryStringArgs = { "isc", isc, "prog_id", _orderData.ProgId};
      string productName = null, productInfoURL = null, legalAgreementURL = null;
      EULAType agreementType = EULAType.Legal;
      switch (EULARule)
      {
        case EULARuleType.XXX:
          productName = ".XXX Domain Registration";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "7333", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.BusinessAccel:
          productName = "Business Accelerator";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "5864", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Business_Accelerator", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Reg:
          productName = "Domain Registration";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "158", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "REG_SA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Transfer:
          productName = "Domain Transfer";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "160", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "TRANSFER_SA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Dbp:
          productName = "Private Registration";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "248", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "DOMAIN_NAMEPROXY", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Hosting:
          productName = "Hosting";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "4", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "HOSTING_SA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.QSC:
          productName = ProductNames.ShoppingCart;
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "267", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "QSC_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Csite:
          productName = "Online Copyright Registration";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "182", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "CSITE_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.EmailCounts:
          productName = "Express Email Marketing";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "185", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "QS_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.StealthRay: break;
        case EULARuleType.TB:
          productName = "Search Engine Visibility";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "736", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "TB_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.WSC:
          break;
        case EULARuleType.WST:
          productName = "WebSite Tonight&#174;";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "178", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "WST_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DedHostingIP:
          productName = "Dedicated Hosting IP";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "1057", QueryParamMode.CommonParameters, false, queryStringArgs);

          break;
        case EULARuleType.FaxThruEmail:
          productName = "Fax Thru Email";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "175", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "FAXEMAIL_SA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.SMTPRelay:
          productName = "SMTP Relay";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "345", QueryParamMode.CommonParameters, false, queryStringArgs);

          break;
        case EULARuleType.Starter:
          productName = "Starter Web Page or For Sale Page";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "825", QueryParamMode.CommonParameters, false, queryStringArgs);

          break;
        case EULARuleType.FreeHosting:
          productName = "Free Hosting w/Web Site Builder";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "243", QueryParamMode.CommonParameters, false, queryStringArgs);

          break;
        case EULARuleType.TrafficFacts:
          productName = "Site Analytics";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "233", QueryParamMode.CommonParameters, false, queryStringArgs);

          break;
        case EULARuleType.SSLCerts:
          productName = "SSL Certificates";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "186", QueryParamMode.CommonParameters, false, queryStringArgs);

          break;
        case EULARuleType.Merchant:
          productName = "Merchant Account";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "237", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.DedVirtHosting:
          productName = "Virtual Dedicated Server";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "1122", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "hosting_sa", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DedHosting:
          productName = "Dedicated Server";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "1122", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Dedicated Hosting SA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.FreeWebmail:
          productName = "Free Personal Email";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "154", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "WEBMAIL_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Spam: break;
        case EULARuleType.VSDB:
          productName = "Online File Folder";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "261", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "VSDB_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DA:
          productName = "DomainAlert&#174;";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "247", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "DOMAIN_BACK", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.WebMail:
          productName = "Webmail";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "154", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "WEBMAIL_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.InstantPage:
          productName = "InstantPage&#174;";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "category/instantpage/866", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "InstantPage_TOS", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.QB:
          productName = ProductNames.QuickBlogcast;
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "477", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "QUICKBLOG_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Whois:
          productName = "Business Registration";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "255", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Deluxe_WhoIs_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DOP:
          productName = "Domain Ownership Protection";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "614", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Domain_Protect_SA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.CashPark:
          productName = "CashParking";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "285", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Cash_Park_SA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.CashParkHdr:
          productName = "CashParking Custom Header";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "659", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Custom_Header_SA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DotCert:
          productName = "Certified Domain";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "297", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.GiftCard:
          productName = "Gift Card";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "302", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "gift_card", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Broker:
          productName = "Domain Buy Service";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "304", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Domain_Brokerage_SA", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.FTP:
          productName = "FTP Backup";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "1686", QueryParamMode.CommonParameters, false, queryStringArgs);

          break;
        case EULARuleType.EZPrint:
          productName = "Photo Store";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "305", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.Photo:
          productName = (_orderData.IsGodaddy) ? "Go Daddy Photo Album" : "Photo Album";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "340", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Photo_Prod_SA", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Club:
          productName = "Discount Domain Club";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "2398", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "discountclub", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.AssistedServices:
          productName = "Assisted Service Plan";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "2466", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.Logo:
          productName = "Logo Design";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "475", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "logo_design_sa", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Banner:
          productName = "Web Banner Design";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "475", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "web_banner_design_sa", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.CustomWST:
          productName = "Dream Website Design";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "449", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "custom_website", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.WST_WithMaint:
          productName = "Website Tonight w/Maintenance";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "449", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "WST_maintenance_eula", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.WST_Update:
          productName = "Website Update";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "449", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "website_update_sa", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Training:
          productName = "GoDaddy University terms of service:";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "training", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Super:
          productName = "Super Reseller";
          if (_orderData.ContextId == ContextIds.WildWestDomains)
          {
            productInfoURL = _links.GetUrl(LinkTypes.Starfield, "guides/getting_started_super_reseller.pdf");
          }
          else
          {
            productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "3340", QueryParamMode.CommonParameters, false, queryStringArgs);
          }
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "superreseller_eula", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Reseller:
          productName = "Reseller Plans";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "220", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "reseller_sa", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.premListing:
          productName = "Premium Domain Name";
          productInfoURL = _links.GetUrl(LinkTypes.Help, ARTICLE_RELATIVE_PATH + "3497", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Premium_Domain", "isc", isc, "prog_id", _orderData.ProgId);
          agreementType = EULAType.Membership;
          break;
        case EULARuleType.loadedDomain:
          productName = "SmartSpace&#174;";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "686", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "LOADED_DOMAIN", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.dad:
          productName = "Power Content Plans";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "680", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Domain_Development", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.crm:
          productName = (_orderData.IsGodaddy) ? "Go Daddy Contact Manager" : "Contact Manager";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "717", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "Customer_Manager_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.survey_EULA:
          productName = (_orderData.IsGodaddy) ? "Go Daddy Site Surveys" : "Site Surveys";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "725", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "site_survey_EULA", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.OutlookMail:
          productName = "Hosted Exchange Email";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "663", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "hosted_exchange", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.WebStore:
          productName = "Web Store Design";
          productInfoURL = _links.GetUrl(LinkTypes.Help, TOPIC_RELATIVE_PATH + "735", QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "webstore_design_svc_and_mtce_agmt", "isc", isc, "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.AdSpace:
          productName = "Ad Space";
          productInfoURL = _links.GetUrl(LinkTypes.Community, HELP_ARTICLE_RELATIVE_PATH + "6161",
                                         QueryParamMode.CommonParameters, false, queryStringArgs);
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", "AdSpace_TOS", "isc", isc, "prog_id", _orderData.ProgId);
          break;

      }

      EULAItem eulaItem = new EULAItem(productName, productInfoURL, legalAgreementURL);
      eulaItem.AgreementType = agreementType;
      return eulaItem;
    }
    
  }
}
