
using System.Collections.Generic;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal static class ProductIds
  {
    public const int LogoDesign = 652;
    public const int DeluxeLogoDesign = 925;
    public const int BusinessCardDesign = 927;
    public const int LetterHeadDesign = 928;
    public const int CustomDesignBannerStatic = 930;
    public const int CustomDesignBannerAnimated = 931;
    public const int CustomDesignFavicon = 933;

    public const int website01pg = 12301; //WST_ 1 page site - not sold renewal
    public const int website05pg = 12302; //WST_ 5 page site
    public const int website10pg = 12303; //WST_ 10 page site
    public const int website20pg = 12304; //WST_ 20 page site

    public const int WST_E_1year = 6760;		//WebSite Tonight Economy - 5 Page Web Site - 1 year
    public const int WST_E_2year = 6761;
    public const int WST_E_3year = 6762;
    public const int WST_E_4year = 6763;
    public const int WST_E_5year = 6764;

    public const int WST_D_1year = 6770;		//WebSite Tonight Deluxe - 10 Page Web Site - 1 year
    public const int WST_D_2year = 6771;
    public const int WST_D_3year = 6772;
    public const int WST_D_4year = 6773;
    public const int WST_D_5year = 6774;

    public const int WST_P_1year = 6780;		//WebSite Tonight Premium - Unlimited Pages Web Site - 1 year
    public const int WST_P_2year = 6781;
    public const int WST_P_3year = 6782;
    public const int WST_P_4year = 6783;
    public const int WST_P_5year = 6784;

    public const int OGC_E = 805; //Online Group Calendar: Economy (1-3)	1 year
    public const int OGC_D = 806; //Online Group Calendar: Deluxe (4-10)	1 year
    public const int OGC_P = 807; //Online Group Calendar: Premium (11-100)	1 year

    public const int FTE_Local_E = 1100; //fax thru email - local economy plan (100 minutes)
    public const int FTE_Local_D = 1134; //fax thru email - local deluxe plan (200 minutes)
    public const int FTE_Local_P = 1135; //fax thru email - local premium plan (400 minutes)
    public const int FTE_Toll_E = 1136; //fax thru email - toll free economy plan (100 minutes)
    public const int FTE_Toll_D = 1137; //fax thru email - toll free deluxe plan (200 minutes)
    public const int FTE_Toll_P = 1138;  //fax thru email - toll free premium plan (400 minutes)

    public const int Calendar_E = 800; //Online Group Calendar: Economy (1-5) - no longer offered
    public const int Calendar_D = 801; //Online Group Calendar: Deluxe (6-25) - no longer offered
    public const int Calendar_P = 802; //Online Group Calendar: Premium (26-100) - no longer offered

    public const int Cart_E_Monthly = 77; //economy quick shopping cart id
    public const int Cart_D_Monthly = 78; //deluxe quick shopping cart id
    public const int Cart_P_Monthly = 79; //premium quick shopping cart id

    public const int Cart_E_1year = 6790; //Quick Shopping Cart - Economy Edition - 1 year
    public const int Cart_E_2year = 6791;
    public const int Cart_E_3year = 6792;
    public const int Cart_E_4year = 6793;
    public const int Cart_E_5year = 6794;

    public const int Cart_D_1year = 6800; //Quick Shopping Cart - Deluxe Edition - 1 year
    public const int Cart_D_2year = 6801;
    public const int Cart_D_3year = 6802;
    public const int Cart_D_4year = 6803;
    public const int Cart_D_5year = 6804;

    public const int Cart_P_1year = 6810;	//Quick Shopping Cart - Premium Edition - 1 year
    public const int Cart_P_2year = 6811;
    public const int Cart_P_3year = 6812;
    public const int Cart_P_4year = 6813;
    public const int Cart_P_5year = 6814;

    public const int DNA_DomainPurchase = 736;
    public const int DNA_ManagedOffer = 737;
    public const int DNA_ManagedAuction = 738;
    public const int DNA_TransAssuredOffer = 739;
    public const int DNA_PrivateOffer = 740;
    public const int DNA_PrivateAuction = 741;
    public const int DNA_TransAssuredAuction = 742;
    public const int DNA_Subscription = 761009;
    public const int DNA_SubscriptionRenewal = 10743;
    public const int DNA_SubscriptionMonitorBundle = 761009;

    public const int Reseller = 765002; //basic reseller turnkey
    public const int Reseller_Renewal = 775002; //basic reseller turnkey renewal
    public const int ResellerPro = 765001; //reseller turnkey
    public const int ResellerPro_Renewal = 775001; //reseller turnkey renewal

    public const int Hosting_Asp_D_Monthly = 154;
    public const int Hosting_Asp_D_1year = 6720;		//Deluxe Hosting with ASP - 1 year
    public const int Hosting_Asp_D_2year = 6721;
    public const int Hosting_Asp_D_3year = 6722;
    public const int Hosting_Asp_D_4year = 6723;
    public const int Hosting_Asp_D_5year = 6724;

    public const int Hosting_Cgi_D_1year = 6730;		//Premium Hosting w/ PHP / PERL - 1 year
    public const int Hosting_Cgi_D_2year = 6731;
    public const int Hosting_Cgi_D_3year = 6732;
    public const int Hosting_Cgi_D_4year = 6733;
    public const int Hosting_Cgi_D_5year = 6734;

    public const int ColdFusion = 4605; //ColdFusion hosting addon
    public const int TrafficStats_D = 3701; //traffic facts
    public const int TrafficBlazor_D = 763001; //traffic blazer 1yr w/ email counts
    public const int TrafficBlazor_1year = 1401; //traffic blazer 1yr
    public const int CampaignBlazer_Tier1 = 4701; //emailcounts 5,000

    public const int Email_D = 1866; //deluxe email
    public const int OFF_D = 3800;
    public const int EEM_Tier1 = 4711; //emailcounts 500 month
    public const int SSL_1Year_Turbo = 3604; //turbo ssl (1 Year)
    public const int SSL_1Year_Cert = 3601; //high-assurance ssl 1 yr
    public const int Copyright = 2202; //csite
    public const int ResellerSuper = 18893; //reseller Super
    public const int MerchAcctStd = 3010; //Basic (Chase Paymentech) - GD
    public const int MerchPQ = 3002; //Basic (PayQuake) - Resellers
    public const int DBP = 7001; //dbp
    public const int WhoIs = 84; //Deluxe Business Registration - Proxima

    public const int domainAlertDept = 130;
    public const int domainAlert01Pk = 9002;	//monitoring single - no longer offered
    public const int domainAlert10Pk = 9001;	//monitoring 10pk - no longer offered (use 100pk)
    public const int domainAlert100Pk = 9001; //monitoring 100pk - replacing 10pk, same pfids
    public const int domainAlertPLSub = 9004; //power list
    public const int domainAlertBOrder = 9003; //backorder
    public const int domainAlertPrvBOrder = 9005; //dbp for backorders
    public const int domainAlert01PkRenewal = 19002; //monitoring single renewal
    public const int domainAlert10PkRenewal = 19001; //monitoring 10pk renewal - no longer offered (use 100pk)
    public const int domainAlert100PkRenewal = 19001; //monitoring 100pk renewal - replacing 10pk renewal, same pfids
    public const int domainAlertPLSubRenewal = 19004; //power list renewal
    public const int domainAlertBOrderRenewal = 19003; //backorder renewal
    public const int domainAlertPrvBOrderRenewal = 19005; //dbp for backorders renewal
    public const int powerGrabPrice = 101; //powergrab rates for expiring names list
    public const int domainAlertSTBOrder = 9006;		//standard and tactics backorder
    public const int domainAlertMon = 761007; //domain alert w/monitoring bundle
    public const int domainAlertBOrderMon = 761008; //domain alert backorder w/monitoring bundle
    public const int domainAlertMonRenewal = 771007; //domain alert w/monitoring bundle	renewal
    public const int domainAlertBOrderMonRenewal = 771008; //domain alert backorder w/monitoring bundle renewal

    public const int DBPAdminFees_10 = 8004;
    public const int DBPAdminFees_20 = 8005;
    public const int DBPAdminFees_30 = 8006;

    public const int DomainsByProxy = 7001;

    public const int WebSiteDesignRestartFee = 653;
    public const int TemplateDesignKillFee = 2326;
    public const int BrandIdentityDesignCancellationFee = 2327;
    public const int WebSiteDesignServiceAdditionalProcessing = 2328;
    public const int CustomWebServiceMiscellaneousCharge = 2329;
    public const int WebDesignCancellation_E = 2334;
    public const int WebDesignCancellation_D = 2335;
    public const int WebDesignCancellation_P = 2336;
    public const int AdministrativeFeesInvalidWhoIs_25 = 8014;

    public const int SEVBusinessAccel = 764000;

    public const int VideoMeEcon1mo = 56909;
    public const int VideoMeEcon1yr = 56911;
    public const int VideoMeEcon2yr = 56912;
    public const int VideoMeEcon3yr = 56913;

    public const int AdSpaceEconomyMonth = 1025;
    public const int AdSpaceEconomyQuarterly = 1026;
    public const int AdSpaceEconomyYear = 1027;
    public const int AdSpaceEconomyTwoYears = 1028;
    public const int AdSpaceEconomyThreeYears = 1029;
    public const int AdSpaceDeluxeMonth = 1035;
    public const int AdSpaceDeluxeQuarterly = 1036;
    public const int AdSpaceDeluxeYear = 1037;
    public const int AdSpaceDeluxeTwoYears = 1038;
    public const int AdSpaceDeluxeThreeYears = 1039;
    public const int AdSpacePremiumMonth = 1045;
    public const int AdSpacePremiumQuarterly = 1046;
    public const int AdSpacePremiumYear = 1047;
    public const int AdSpacePremiumTwoYears = 1048;
    public const int AdSpacePremiumThreeYears = 1049;

    public const int HostingSharedEconomyLinuxMonthly = 58;
    public const int HostingSharedEconomyWindowsMonthly = 64;
    public const int HostingSharedDeluxeWindowsMonthly = 154;
    public const int HostingSharedDeluxeLinuxMonthly = 156;
    public const int HostingGridLinuxMonthly = 161;
    public const int HostingGridWindowsMonthly = 163;
    public const int HostingSharedPremiumLinuxMonthly = 180;
    public const int HostingSharedPremiumWindowsMonthly = 181;
    public const int HostingWordPressEconomyMonthly = 6660;
    public const int HostingWordPressEconomy1Year = 6661;
    public const int HostingWordPressEconomy2Years = 6662;
    public const int HostingWordPressEconomy3Years = 6663;
    public const int HostingWordPressEconomy4Years = 6664;
    public const int HostingWordPressEconomy5Years = 6665;
    public const int HostingWordPressDeluxeMonthly = 6670;
    public const int HostingWordPressDeluxe1year = 6671;
    public const int HostingWordPressDeluxe2Years = 6672;
    public const int HostingWordPressDeluxe3Years = 6673;
    public const int HostingWordPressDeluxe4Years = 6674;
    public const int HostingWordPressDeluxe5Years = 6675;
    public const int HostingWordPressUltimateMonthly = 6680;
    public const int HostingWordPressUltimate1year = 6681;
    public const int HostingWordPressUltimate2Years = 6682;
    public const int HostingWordPressUltimate3Years = 6683;
    public const int HostingWordPressUltimate4Years = 6684;
    public const int HostingWordPressUltimate5Years = 6685;
    public const int HostingSharedEconomyLinux1year = 6700;
    public const int HostingSharedEconomyLinux2Years = 6701;
    public const int HostingSharedEconomyLinux3Years = 6702;
    public const int HostingSharedEconomyLinux4Years = 6703;
    public const int HostingSharedEconomyLinux5Years = 6704;
    public const int HostingSharedEconomyWindows1year = 6710;
    public const int HostingSharedEconomyWindows2Years = 6711;
    public const int HostingSharedEconomyWindows3Years = 6712;
    public const int HostingSharedEconomyWindows4Years = 6713;
    public const int HostingSharedEconomyWindows5Years = 6714;
    public const int HostingSharedDeluxeWindows1year = 6720;
    public const int HostingSharedDeluxeWindows2Years = 6721;
    public const int HostingSharedDeluxeWindows3Years = 6722;
    public const int HostingSharedDeluxeWindows4Years = 6723;
    public const int HostingSharedDeluxeWindows5Years = 6724;
    public const int HostingSharedDeluxeLinux1year = 6730;
    public const int HostingSharedDeluxeLinux2Years = 6731;
    public const int HostingSharedDeluxeLinux3Years = 6732;
    public const int HostingSharedDeluxeLinux4Years = 6733;
    public const int HostingSharedDeluxeLinux5Years = 6734;
    public const int HostingSharedPremiumLinux1year = 6740;
    public const int HostingSharedPremiumLinux2Years = 6741;
    public const int HostingSharedPremiumLinux3Years = 6742;
    public const int HostingSharedPremiumLinux4Years = 6743;
    public const int HostingSharedPremiumLinux5Years = 6744;
    public const int HostingSharedPremiumWindows1year = 6750;
    public const int HostingSharedPremiumWindows2Years = 6751;
    public const int HostingSharedPremiumWindows3Years = 6752;
    public const int HostingSharedPremiumWindows4Years = 6753;
    public const int HostingSharedPremiumWindows5Years = 6754;
    public const int PremiumPlusLinuxMonthly = 7210;
    public const int PremiumPlusWindowsMonthly = 7211;
    public const int PremiumPlusLinux1year = 7212;
    public const int PremiumPlusLinux2Years = 7213;
    public const int PremiumPlusLinux3Years = 7214;
    public const int PremiumPlusLinux4Years = 7215;
    public const int PremiumPlusLinux5Years = 7216;
    public const int PremiumPlusWindows1year = 7217;
    public const int PremiumPlusWindows2Years = 7218;
    public const int PremiumPlusWindows3Years = 7219;
    public const int PremiumPlusWindows4Years = 7220;
    public const int PremiumPlusWindows5Years = 7221;
    public const int HostingSharedUltimateLinuxMonthly = 7225;
    public const int HostingSharedUltimateWindowsMonthly = 7226;
    public const int HostingSharedUltimateLinux1year = 7227;
    public const int HostingSharedUltimateLinux2Years = 7228;
    public const int HostingSharedUltimateLinux3Years = 7229;
    public const int HostingSharedUltimateLinux4Years = 7230;
    public const int HostingSharedUltimateLinux5Years = 7231;
    public const int HostingSharedUltimateWindows1year = 7232;
    public const int HostingSharedUltimateWindows2Years = 7233;
    public const int HostingSharedUltimateWindows3Years = 7234;
    public const int HostingSharedUltimateWindows4Years = 7235;
    public const int HostingSharedUltimateWindows5Years = 7236;
    public const int HostingGridEconomyLinuxMonthly = 42001;
    public const int HostingGridEconomyLinux1year = 42002;
    public const int HostingGridEconomyLinux2Years = 42003;
    public const int HostingGridEconomyLinux3Years = 42004;
    public const int HostingGridEconomyLinux4Years = 42005;
    public const int HostingGridEconomyLinux5Years = 42006;
    public const int HostingGridDeluxeLinuxMonthly = 42011;
    public const int HostingGridDeluxeLinux1year = 42012;
    public const int HostingGridDeluxeLinux2Years = 42013;
    public const int HostingGridDeluxeLinux3Years = 42014;
    public const int HostingGridDeluxeLinux4Years = 42015;
    public const int HostingGridDeluxeLinux5Years = 42016;
    public const int HostingGridUltimateLinuxMonthly = 42021;
    public const int HostingGridUltimateLinux1year = 42022;
    public const int HostingGridUltimateLinux2Years = 42023;
    public const int HostingGridUltimateLinux3Years = 42024;
    public const int HostingGridUltimateLinux4Years = 42025;
    public const int HostingGridUltimateLinux5Years = 42026;
    public const int HostingGridEconomyLinuxEURegionMonthly = 42051;
    public const int HostingGridEconomyLinuxEURegion1year = 42052;
    public const int HostingGridEconomyLinuxEURegion2Years = 42053;
    public const int HostingGridEconomyLinuxEURegion3Years = 42054;
    public const int HostingGridEconomyLinuxEURegion4Years = 42055;
    public const int HostingGridEconomyLinuxEURegion5Years = 42056;
    public const int HostingGridDeluxeLinuxEURegionMonthly = 42061;
    public const int HostingGridDeluxeLinuxEURegion1year = 42062;
    public const int HostingGridDeluxeLinuxEURegion2Years = 42063;
    public const int HostingGridDeluxeLinuxEURegion3Years = 42064;
    public const int HostingGridDeluxeLinuxEURegion4Years = 42065;
    public const int HostingGridDeluxeLinuxEURegion5Years = 42066;
    public const int HostingGridUltimateLinuxEURegionMonthly = 42071;
    public const int HostingGridUltimateLinuxEURegion1year = 42072;
    public const int HostingGridUltimateLinuxEURegion2Years = 42073;
    public const int HostingGridUltimateLinuxEURegion3Years = 42074;
    public const int HostingGridUltimateLinuxEURegion4Years = 42075;
    public const int HostingGridUltimateLinuxEURegion5Years = 42076;
    public const int HostingGridEconomyWindowsMonthly = 42101;
    public const int HostingGridEconomyWindows1year = 42102;
    public const int HostingGridEconomyWindows2Years = 42103;
    public const int HostingGridEconomyWindows3Years = 42104;
    public const int HostingGridEconomyWindows4Years = 42105;
    public const int HostingGridEconomyWindows5Years = 42106;
    public const int HostingGridDeluxeWindowsMonthly = 42111;
    public const int HostingGridDeluxeWindows1year = 42112;
    public const int HostingGridDeluxeWindows2Years = 42113;
    public const int HostingGridDeluxeWindows3Years = 42114;
    public const int HostingGridDeluxeWindows4Years = 42115;
    public const int HostingGridDeluxeWindows5Years = 42116;
    public const int HostingGridUltimateWindowsMonthly = 42121;
    public const int HostingGridUltimateWindows1year = 42122;
    public const int HostingGridUltimateWindows2Years = 42123;
    public const int HostingGridUltimateWindows3Years = 42124;
    public const int HostingGridUltimateWindows4Years = 42125;
    public const int HostingGridUltimateWindows5Years = 42126;
    public const int HostingGridEconomyWindowsEURegionMonthly = 42151;
    public const int HostingGridEconomyWindowsEURegion1year = 42152;
    public const int HostingGridEconomyWindowsEURegion2Years = 42153;
    public const int HostingGridEconomyWindowsEURegion3Years = 42154;
    public const int HostingGridEconomyWindowsEURegion4Years = 42155;
    public const int HostingGridEconomyWindowsEURegion5Years = 42156;
    public const int HostingGridDeluxeWindowsEURegionMonthly = 42161;
    public const int HostingGridDeluxeWindowsEURegion1year = 42162;
    public const int HostingGridDeluxeWindowsEURegion2Years = 42163;
    public const int HostingGridDeluxeWindowsEURegion3Years = 42164;
    public const int HostingGridDeluxeWindowsEURegion4Years = 42165;
    public const int HostingGridDeluxeWindowsEURegion5Years = 42166;
    public const int HostingGridUltimateWindowsEURegionMonthly = 42171;
    public const int HostingGridUltimateWindowsEURegion1year = 42172;
    public const int HostingGridUltimateWindowsEURegion2Years = 42173;
    public const int HostingGridUltimateWindowsEURegion3Years = 42174;
    public const int HostingGridUltimateWindowsEURegion4Years = 42175;
    public const int HostingGridUltimateWindowsEURegion5Years = 42176;
    public const int HostingGridEconomyLinuxAPRegionMonthly = 42177;
    public const int HostingGridEconomyLinuxAPRegion1year = 42178;
    public const int HostingGridEconomyLinuxAPRegion2Years = 42179;
    public const int HostingGridEconomyLinuxAPRegion3Years = 42180;
    public const int HostingGridEconomyLinuxAPRegion4Years = 42181;
    public const int HostingGridEconomyLinuxAPRegion5Years = 42182;
    public const int HostingGridDeluxeLinuxAPRegionMonthly = 42183;
    public const int HostingGridDeluxeLinuxAPRegion1year = 42184;
    public const int HostingGridDeluxeLinuxAPRegion2Years = 42185;
    public const int HostingGridDeluxeLinuxAPRegion3Years = 42186;
    public const int HostingGridDeluxeLinuxAPRegion4Years = 42187;
    public const int HostingGridDeluxeLinuxAPRegion5Years = 42188;
    public const int HostingGridUltimateLinuxAPRegionMonthly = 42189;
    public const int HostingGridUltimateLinuxAPRegion1year = 42190;
    public const int HostingGridUltimateLinuxAPRegion2Years = 42191;
    public const int HostingGridUltimateLinuxAPRegion3Years = 42192;
    public const int HostingGridUltimateLinuxAPRegion4Years = 42193;
    public const int HostingGridUltimateLinuxAPRegion5Years = 42194;
    public const int HostingGridEconomyWindowsAPRegionMonthly = 42195;
    public const int HostingGridEconomyWindowsAPRegion1year = 42196;
    public const int HostingGridEconomyWindowsAPRegion2Years = 42197;
    public const int HostingGridEconomyWindowsAPRegion3Years = 42198;
    public const int HostingGridEconomyWindowsAPRegion4Years = 42199;
    public const int HostingGridEconomyWindowsAPRegion5Years = 42200;
    public const int HostingGridDeluxeWindowsAPRegionMonthly = 42201;
    public const int HostingGridDeluxeWindowsAPRegion1year = 42202;
    public const int HostingGridDeluxeWindowsAPRegion2Years = 42203;
    public const int HostingGridDeluxeWindowsAPRegion3Years = 42204;
    public const int HostingGridDeluxeWindowsAPRegion4Years = 42205;
    public const int HostingGridDeluxeWindowsAPRegion5Years = 42206;
    public const int HostingGridUltimateWindowsAPRegionMonthly = 42207;
    public const int HostingGridUltimateWindowsAPRegion1year = 42208;
    public const int HostingGridUltimateWindowsAPRegion2Years = 42209;
    public const int HostingGridUltimateWindowsAPRegion3Years = 42210;
    public const int HostingGridUltimateWindowsAPRegion4Years = 42211;
    public const int HostingGridUltimateWindowsAPRegion5Years = 42212;
    public const int AdSpaceEconomyRecurringMonth = 101025;
    public const int AdSpaceEconomyRecurringQuarterly = 101026;
    public const int AdSpaceEconomyRecurringYear = 101027;
    public const int AdSpaceEconomyRecurringTwoYears = 101028;
    public const int AdSpaceEconomyRecurringThreeYears = 101029;
    public const int AdSpaceDeluxeRecurringMonth = 101035;
    public const int AdSpaceDeluxeRecurringQuarterly = 101036;
    public const int AdSpaceDeluxeRecurringYear = 101037;
    public const int AdSpaceDeluxeRecurringTwoYears = 101038;
    public const int AdSpaceDeluxeRecurringThreeYears = 101039;
    public const int AdSpacePremiumRecurringMonth = 101045;
    public const int AdSpacePremiumRecurringQuarterly = 101046;
    public const int AdSpacePremiumRecurringYear = 101047;
    public const int AdSpacePremiumRecurringTwoYears = 101048;
    public const int AdSpacePremiumRecurringThreeYears = 101049;
      public static List<int> productIds = null;

      public static bool isDomainsByProxy(int productId)
      {
          return productId == DomainsByProxy;
      }

      public static bool isHostingProduct(int productId)
      {
          if (productIds == null)
          {
            productIds = new List<int>(new[]
                                                         {   
                                                             ProductIds.HostingSharedEconomyLinuxMonthly,
                                                             ProductIds.HostingSharedEconomyWindowsMonthly,
                                                             ProductIds.HostingSharedDeluxeWindowsMonthly,
                                                             ProductIds.HostingSharedDeluxeLinuxMonthly,
                                                             ProductIds.HostingGridLinuxMonthly,
                                                             ProductIds.HostingGridWindowsMonthly,
                                                             ProductIds.HostingSharedPremiumLinuxMonthly,
                                                             ProductIds.HostingSharedPremiumWindowsMonthly,
                                                             ProductIds.HostingWordPressEconomyMonthly,
                                                             ProductIds.HostingWordPressEconomy1Year,
                                                             ProductIds.HostingWordPressEconomy2Years,
                                                             ProductIds.HostingWordPressEconomy3Years,
                                                             ProductIds.HostingWordPressEconomy4Years,
                                                             ProductIds.HostingWordPressEconomy5Years,
                                                             ProductIds.HostingWordPressDeluxeMonthly,
                                                             ProductIds.HostingWordPressDeluxe1year,
                                                             ProductIds.HostingWordPressDeluxe2Years,
                                                             ProductIds.HostingWordPressDeluxe3Years,
                                                             ProductIds.HostingWordPressDeluxe4Years,
                                                             ProductIds.HostingWordPressDeluxe5Years,
                                                             ProductIds.HostingWordPressUltimateMonthly,
                                                             ProductIds.HostingWordPressUltimate1year,
                                                             ProductIds.HostingWordPressUltimate2Years,
                                                             ProductIds.HostingWordPressUltimate3Years,
                                                             ProductIds.HostingWordPressUltimate4Years,
                                                             ProductIds.HostingWordPressUltimate5Years,
                                                             ProductIds.HostingSharedEconomyLinux1year,
                                                             ProductIds.HostingSharedEconomyLinux2Years,
                                                             ProductIds.HostingSharedEconomyLinux3Years,
                                                             ProductIds.HostingSharedEconomyLinux4Years,
                                                             ProductIds.HostingSharedEconomyLinux5Years,
                                                             ProductIds.HostingSharedEconomyWindows1year,
                                                             ProductIds.HostingSharedEconomyWindows2Years,
                                                             ProductIds.HostingSharedEconomyWindows3Years,
                                                             ProductIds.HostingSharedEconomyWindows4Years,
                                                             ProductIds.HostingSharedEconomyWindows5Years,
                                                             ProductIds.HostingSharedDeluxeWindows1year,
                                                             ProductIds.HostingSharedDeluxeWindows2Years,
                                                             ProductIds.HostingSharedDeluxeWindows3Years,
                                                             ProductIds.HostingSharedDeluxeWindows4Years,
                                                             ProductIds.HostingSharedDeluxeWindows5Years,
                                                             ProductIds.HostingSharedDeluxeLinux1year,
                                                             ProductIds.HostingSharedDeluxeLinux2Years,
                                                             ProductIds.HostingSharedDeluxeLinux3Years,
                                                             ProductIds.HostingSharedDeluxeLinux4Years,
                                                             ProductIds.HostingSharedDeluxeLinux5Years,
                                                             ProductIds.HostingSharedPremiumLinux1year,
                                                             ProductIds.HostingSharedPremiumLinux2Years,
                                                             ProductIds.HostingSharedPremiumLinux3Years,
                                                             ProductIds.HostingSharedPremiumLinux4Years,
                                                             ProductIds.HostingSharedPremiumLinux5Years,
                                                             ProductIds.HostingSharedPremiumWindows1year,
                                                             ProductIds.HostingSharedPremiumWindows2Years,
                                                             ProductIds.HostingSharedPremiumWindows3Years,
                                                             ProductIds.HostingSharedPremiumWindows4Years,
                                                             ProductIds.HostingSharedPremiumWindows5Years,
                                                             ProductIds.PremiumPlusLinuxMonthly,
                                                             ProductIds.PremiumPlusWindowsMonthly,
                                                             ProductIds.PremiumPlusLinux1year,
                                                             ProductIds.PremiumPlusLinux2Years,
                                                             ProductIds.PremiumPlusLinux3Years,
                                                             ProductIds.PremiumPlusLinux4Years,
                                                             ProductIds.PremiumPlusLinux5Years,
                                                             ProductIds.PremiumPlusWindows1year,
                                                             ProductIds.PremiumPlusWindows2Years,
                                                             ProductIds.PremiumPlusWindows3Years,
                                                             ProductIds.PremiumPlusWindows4Years,
                                                             ProductIds.PremiumPlusWindows5Years,
                                                             ProductIds.HostingSharedUltimateLinuxMonthly,
                                                             ProductIds.HostingSharedUltimateWindowsMonthly,
                                                             ProductIds.HostingSharedUltimateLinux1year,
                                                             ProductIds.HostingSharedUltimateLinux2Years,
                                                             ProductIds.HostingSharedUltimateLinux3Years,
                                                             ProductIds.HostingSharedUltimateLinux4Years,
                                                             ProductIds.HostingSharedUltimateLinux5Years,
                                                             ProductIds.HostingSharedUltimateWindows1year,
                                                             ProductIds.HostingSharedUltimateWindows2Years,
                                                             ProductIds.HostingSharedUltimateWindows3Years,
                                                             ProductIds.HostingSharedUltimateWindows4Years,
                                                             ProductIds.HostingSharedUltimateWindows5Years,
                                                             ProductIds.HostingGridEconomyLinuxMonthly,
                                                             ProductIds.HostingGridEconomyLinux1year,
                                                             ProductIds.HostingGridEconomyLinux2Years,
                                                             ProductIds.HostingGridEconomyLinux3Years,
                                                             ProductIds.HostingGridEconomyLinux4Years,
                                                             ProductIds.HostingGridEconomyLinux5Years,
                                                             ProductIds.HostingGridDeluxeLinuxMonthly,
                                                             ProductIds.HostingGridDeluxeLinux1year,
                                                             ProductIds.HostingGridDeluxeLinux2Years,
                                                             ProductIds.HostingGridDeluxeLinux3Years,
                                                             ProductIds.HostingGridDeluxeLinux4Years,
                                                             ProductIds.HostingGridDeluxeLinux5Years,
                                                             ProductIds.HostingGridUltimateLinuxMonthly,
                                                             ProductIds.HostingGridUltimateLinux1year,
                                                             ProductIds.HostingGridUltimateLinux2Years,
                                                             ProductIds.HostingGridUltimateLinux3Years,
                                                             ProductIds.HostingGridUltimateLinux4Years,
                                                             ProductIds.HostingGridUltimateLinux5Years,
                                                             ProductIds.HostingGridEconomyLinuxEURegionMonthly,
                                                             ProductIds.HostingGridEconomyLinuxEURegion1year,
                                                             ProductIds.HostingGridEconomyLinuxEURegion2Years,
                                                             ProductIds.HostingGridEconomyLinuxEURegion3Years,
                                                             ProductIds.HostingGridEconomyLinuxEURegion4Years,
                                                             ProductIds.HostingGridEconomyLinuxEURegion5Years,
                                                             ProductIds.HostingGridDeluxeLinuxEURegionMonthly,
                                                             ProductIds.HostingGridDeluxeLinuxEURegion1year,
                                                             ProductIds.HostingGridDeluxeLinuxEURegion2Years,
                                                             ProductIds.HostingGridDeluxeLinuxEURegion3Years,
                                                             ProductIds.HostingGridDeluxeLinuxEURegion4Years,
                                                             ProductIds.HostingGridDeluxeLinuxEURegion5Years,
                                                             ProductIds.HostingGridUltimateLinuxEURegionMonthly,
                                                             ProductIds.HostingGridUltimateLinuxEURegion1year,
                                                             ProductIds.HostingGridUltimateLinuxEURegion2Years,
                                                             ProductIds.HostingGridUltimateLinuxEURegion3Years,
                                                             ProductIds.HostingGridUltimateLinuxEURegion4Years,
                                                             ProductIds.HostingGridUltimateLinuxEURegion5Years,
                                                             ProductIds.HostingGridEconomyWindowsMonthly,
                                                             ProductIds.HostingGridEconomyWindows1year,
                                                             ProductIds.HostingGridEconomyWindows2Years,
                                                             ProductIds.HostingGridEconomyWindows3Years,
                                                             ProductIds.HostingGridEconomyWindows4Years,
                                                             ProductIds.HostingGridEconomyWindows5Years,
                                                             ProductIds.HostingGridDeluxeWindowsMonthly,
                                                             ProductIds.HostingGridDeluxeWindows1year,
                                                             ProductIds.HostingGridDeluxeWindows2Years,
                                                             ProductIds.HostingGridDeluxeWindows3Years,
                                                             ProductIds.HostingGridDeluxeWindows4Years,
                                                             ProductIds.HostingGridDeluxeWindows5Years,
                                                             ProductIds.HostingGridUltimateWindowsMonthly,
                                                             ProductIds.HostingGridUltimateWindows1year,
                                                             ProductIds.HostingGridUltimateWindows2Years,
                                                             ProductIds.HostingGridUltimateWindows3Years,
                                                             ProductIds.HostingGridUltimateWindows4Years,
                                                             ProductIds.HostingGridUltimateWindows5Years,
                                                             ProductIds.HostingGridEconomyWindowsEURegionMonthly,
                                                             ProductIds.HostingGridEconomyWindowsEURegion1year,
                                                             ProductIds.HostingGridEconomyWindowsEURegion2Years,
                                                             ProductIds.HostingGridEconomyWindowsEURegion3Years,
                                                             ProductIds.HostingGridEconomyWindowsEURegion4Years,
                                                             ProductIds.HostingGridEconomyWindowsEURegion5Years,
                                                             ProductIds.HostingGridDeluxeWindowsEURegionMonthly,
                                                             ProductIds.HostingGridDeluxeWindowsEURegion1year,
                                                             ProductIds.HostingGridDeluxeWindowsEURegion2Years,
                                                             ProductIds.HostingGridDeluxeWindowsEURegion3Years,
                                                             ProductIds.HostingGridDeluxeWindowsEURegion4Years,
                                                             ProductIds.HostingGridDeluxeWindowsEURegion5Years,
                                                             ProductIds.HostingGridUltimateWindowsEURegionMonthly,
                                                             ProductIds.HostingGridUltimateWindowsEURegion1year,
                                                             ProductIds.HostingGridUltimateWindowsEURegion2Years,
                                                             ProductIds.HostingGridUltimateWindowsEURegion3Years,
                                                             ProductIds.HostingGridUltimateWindowsEURegion4Years,
                                                             ProductIds.HostingGridUltimateWindowsEURegion5Years,
                                                             ProductIds.HostingGridEconomyLinuxAPRegionMonthly,
                                                             ProductIds.HostingGridEconomyLinuxAPRegion1year,
                                                             ProductIds.HostingGridEconomyLinuxAPRegion2Years,
                                                             ProductIds.HostingGridEconomyLinuxAPRegion3Years,
                                                             ProductIds.HostingGridEconomyLinuxAPRegion4Years,
                                                             ProductIds.HostingGridEconomyLinuxAPRegion5Years,
                                                             ProductIds.HostingGridDeluxeLinuxAPRegionMonthly,
                                                             ProductIds.HostingGridDeluxeLinuxAPRegion1year,
                                                             ProductIds.HostingGridDeluxeLinuxAPRegion2Years,
                                                             ProductIds.HostingGridDeluxeLinuxAPRegion3Years,
                                                             ProductIds.HostingGridDeluxeLinuxAPRegion4Years,
                                                             ProductIds.HostingGridDeluxeLinuxAPRegion5Years,
                                                             ProductIds.HostingGridUltimateLinuxAPRegionMonthly,
                                                             ProductIds.HostingGridUltimateLinuxAPRegion1year,
                                                             ProductIds.HostingGridUltimateLinuxAPRegion2Years,
                                                             ProductIds.HostingGridUltimateLinuxAPRegion3Years,
                                                             ProductIds.HostingGridUltimateLinuxAPRegion4Years,
                                                             ProductIds.HostingGridUltimateLinuxAPRegion5Years,
                                                             ProductIds.HostingGridEconomyWindowsAPRegionMonthly,
                                                             ProductIds.HostingGridEconomyWindowsAPRegion1year,
                                                             ProductIds.HostingGridEconomyWindowsAPRegion2Years,
                                                             ProductIds.HostingGridEconomyWindowsAPRegion3Years,
                                                             ProductIds.HostingGridEconomyWindowsAPRegion4Years,
                                                             ProductIds.HostingGridEconomyWindowsAPRegion5Years,
                                                             ProductIds.HostingGridDeluxeWindowsAPRegionMonthly,
                                                             ProductIds.HostingGridDeluxeWindowsAPRegion1year,
                                                             ProductIds.HostingGridDeluxeWindowsAPRegion2Years,
                                                             ProductIds.HostingGridDeluxeWindowsAPRegion3Years,
                                                             ProductIds.HostingGridDeluxeWindowsAPRegion4Years,
                                                             ProductIds.HostingGridDeluxeWindowsAPRegion5Years,
                                                             ProductIds.HostingGridUltimateWindowsAPRegionMonthly,
                                                             ProductIds.HostingGridUltimateWindowsAPRegion1year,
                                                             ProductIds.HostingGridUltimateWindowsAPRegion2Years,
                                                             ProductIds.HostingGridUltimateWindowsAPRegion3Years,
                                                             ProductIds.HostingGridUltimateWindowsAPRegion4Years,
                                                             ProductIds.HostingGridUltimateWindowsAPRegion5Years,
                                                         });
              }

          return productIds.Contains(productId);
      }
  }
}
