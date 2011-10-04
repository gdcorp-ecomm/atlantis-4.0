using System.Collections.Generic;

using Atlantis.Framework.DocumentAttributes.Interface;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DocumentAttributes.Tests
{
  [TestClass]
  public class DocumentAttributesTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetDocumentAttributes()
    {
      DocumentAttributesRequestData request = new DocumentAttributesRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      request.RequestTimeout = new System.TimeSpan(0, 0, 60);
      request.PrivateLabelId = 1;
      request.Title = true;
      request.LastModified = false;
      request.DocumentId = false;
      request.DocumentName = true;
      List<string> documentNames = new List<string>()
        {
          "Cash_Park_SA", "certified_domains_sa", "CONSOLIDATE", "discountclub", "Domain_Brokerage_SA", "DOMAIN_MON_EULA", "APPRAISAL_SA", "DOMAIN_BACK", "DOMAIN_NC", "DOMAIN_NAMEPROXY", "REG_SA", "TRANSFER_SA", "Domain_Protect_SA", "Domain_Transfer", "instant_mobilizer_EULA", "PARK_SA", "Premium_Domain", "quick_content_sa", "Total_DNS_TOS",
          "API_SA", "gd_affiliate_prog_sa", "RESELLER_SA", "SUPERRESELLER_EULA",
          "GDC_TOS_Customer", "social_visibility_TOS", "Videome_TOS",
          "hosting_PPC_sa", "Cloud_Hosting", "6689", "Dedicated Hosting SA", "HOSTING_SA",
          "Custom_Header_SA", "InstantPage_TOS", "Photo_Prod_SA", "Domain_Development", "QUICKBLOG_EULA", "TB_EULA", "Loaded_Domain", "traffic_sa", "Website_Update_sa", "WST_Maintenance_EULA", "website_header_design_sa", "WST_EULA",
          "Website_Protection_TOS",
          "Auctions_Bid_Offer_Buy", "dna_member",
          "gift_card", "GAG_SA",
          "GROUP_CALENDAR_EULA", "DB_TOS", "FAXEMAIL_SA", "hosted_exchange", "VSDB_EULA", "WEBMAIL_EULA", "WEBMAIL_SA",
          "AdSpace_TOS", "bizspark_TOS", "Logo_Design_sa", "Business_Accelerator", "Customer_Manager_EULA", "DELUXE_WHOIS_EULA", "QS_EULA", "inc_svcs_EULA", "QSC_EULA", "STATS_EULA", "site_survey_EULA", "web_banner_design_sa", "webstore_design_svc_and_mtce_agmt", "webstore_design_service_eula", "webstore_prohibited_items_policy",
          "PRIVACY", "SPAM_EULA", "CIVIL_SUBPOENA", "CRIM_SUBPOENA", "Dispute_On_Transfer_Away_Form", "UNIFORM_DOMAIN", "TRADMARK_COPY", "BRAND GUIDELINES", "PERMISSIONS"
        };
      request.AddDocumentNames(documentNames);

      DocumentAttributesResponseData response = (DocumentAttributesResponseData)Engine.Engine.ProcessRequest(request, 393);
      Assert.IsTrue(response.GetDocumentAttributes.Count == documentNames.Count);
    }
  }
}
