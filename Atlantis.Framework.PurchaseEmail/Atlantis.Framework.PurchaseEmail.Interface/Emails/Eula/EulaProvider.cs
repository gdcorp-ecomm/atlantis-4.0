using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using Atlantis.Framework.DataProvider.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;
using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.Providers.Interface.Products;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails.Eula
{
  public class EulaProvider
  {
    OrderData _orderData;
    ILinkProvider _links;
    IProductProvider _products;
    private DepartmentIds _departmentIds;

    private Dictionary<EULARuleType, EULAItem> _possibleEulaItems = new Dictionary<EULARuleType, EULAItem>();
    private List<EULAItem> _usedEulaItems;
    private Dictionary<string, EULAItem> ListPageIds = new Dictionary<string, EULAItem>();

    private const string LEGAL_RELATIVE_PATH = "agreements/ShowDoc.aspx";
    private const string TOPIC_RELATIVE_PATH = "topic/";
    private const string ARTICLE_RELATIVE_PATH = "article/";

    public EulaProvider(OrderData orderData, ILinkProvider links, ObjectProviderContainer providerContainer)
    {
      try
      {
        _orderData = orderData;
        _links = links;
        _products = providerContainer.Resolve<IProductProvider>();
        _departmentIds = new DepartmentIds(_orderData.PrivateLabelId);
        SetupEulaItems();
      }
      catch (System.Exception ex)
      {
        string message = ex.Message + "\n" + ex.StackTrace;
        AtlantisException aex = new AtlantisException("Error Creating Provider", string.Empty, "0", message, string.Empty,
              _orderData.ShopperId, _orderData.OrderId, string.Empty, string.Empty, 0);
        Engine.Engine.LogAtlantisException(aex);
      }
    }

    public Dictionary<EULARuleType, EULAItem> PossibleEulaItems
    {
      get
      {
        return _possibleEulaItems;
      }
    }

    private void SetupEulaItems()
    {
      _possibleEulaItems[EULARuleType.Reg] = SetupEulaData(EULARuleType.Reg);
      _possibleEulaItems[EULARuleType.Transfer] = SetupEulaData(EULARuleType.Transfer);
      _possibleEulaItems[EULARuleType.Dbp] = SetupEulaData(EULARuleType.Dbp);
      _possibleEulaItems[EULARuleType.Hosting] = SetupEulaData(EULARuleType.Hosting);
      _possibleEulaItems[EULARuleType.QSC] = SetupEulaData(EULARuleType.QSC);
      _possibleEulaItems[EULARuleType.Csite] = SetupEulaData(EULARuleType.Csite);
      _possibleEulaItems[EULARuleType.EmailCounts] = SetupEulaData(EULARuleType.EmailCounts);
      _possibleEulaItems[EULARuleType.StealthRay] = SetupEulaData(EULARuleType.StealthRay);
      _possibleEulaItems[EULARuleType.TB] = SetupEulaData(EULARuleType.TB);
      _possibleEulaItems[EULARuleType.WSC] = SetupEulaData(EULARuleType.WSC);
      _possibleEulaItems[EULARuleType.WST] = SetupEulaData(EULARuleType.WST);
      _possibleEulaItems[EULARuleType.DedHostingIP] = SetupEulaData(EULARuleType.DedHostingIP);
      _possibleEulaItems[EULARuleType.FaxThruEmail] = SetupEulaData(EULARuleType.FaxThruEmail);
      _possibleEulaItems[EULARuleType.SMTPRelay] = SetupEulaData(EULARuleType.SMTPRelay);
      _possibleEulaItems[EULARuleType.Starter] = SetupEulaData(EULARuleType.Starter);
      _possibleEulaItems[EULARuleType.FreeHosting] = SetupEulaData(EULARuleType.FreeHosting);
      _possibleEulaItems[EULARuleType.TrafficFacts] = SetupEulaData(EULARuleType.TrafficFacts);
      _possibleEulaItems[EULARuleType.SSLCerts] = SetupEulaData(EULARuleType.SSLCerts);
      _possibleEulaItems[EULARuleType.Merchant] = SetupEulaData(EULARuleType.Merchant);
      _possibleEulaItems[EULARuleType.DedVirtHosting] = SetupEulaData(EULARuleType.DedVirtHosting);
      _possibleEulaItems[EULARuleType.DedHosting] = SetupEulaData(EULARuleType.DedHosting);
      _possibleEulaItems[EULARuleType.FreeWebmail] = SetupEulaData(EULARuleType.FreeWebmail);
      _possibleEulaItems[EULARuleType.Spam] = SetupEulaData(EULARuleType.Spam);
      _possibleEulaItems[EULARuleType.VSDB] = SetupEulaData(EULARuleType.VSDB);
      _possibleEulaItems[EULARuleType.DA] = SetupEulaData(EULARuleType.DA);
      _possibleEulaItems[EULARuleType.WebMail] = SetupEulaData(EULARuleType.WebMail);
      _possibleEulaItems[EULARuleType.QB] = SetupEulaData(EULARuleType.QB);
      _possibleEulaItems[EULARuleType.Whois] = SetupEulaData(EULARuleType.Whois);
      _possibleEulaItems[EULARuleType.DOP] = SetupEulaData(EULARuleType.DOP);
      _possibleEulaItems[EULARuleType.CashPark] = SetupEulaData(EULARuleType.CashPark);
      _possibleEulaItems[EULARuleType.CashParkHdr] = SetupEulaData(EULARuleType.CashParkHdr);
      _possibleEulaItems[EULARuleType.DotCert] = SetupEulaData(EULARuleType.DotCert);
      _possibleEulaItems[EULARuleType.GiftCard] = SetupEulaData(EULARuleType.GiftCard);
      _possibleEulaItems[EULARuleType.Broker] = SetupEulaData(EULARuleType.Broker);
      _possibleEulaItems[EULARuleType.FTP] = SetupEulaData(EULARuleType.FTP);
      _possibleEulaItems[EULARuleType.EZPrint] = SetupEulaData(EULARuleType.EZPrint);
      _possibleEulaItems[EULARuleType.Photo] = SetupEulaData(EULARuleType.Photo);
      _possibleEulaItems[EULARuleType.Club] = SetupEulaData(EULARuleType.Club);
      _possibleEulaItems[EULARuleType.AssistedServices] = SetupEulaData(EULARuleType.AssistedServices);
      _possibleEulaItems[EULARuleType.Logo] = SetupEulaData(EULARuleType.Logo);
      _possibleEulaItems[EULARuleType.Banner] = SetupEulaData(EULARuleType.Banner);
      _possibleEulaItems[EULARuleType.CustomWST] = SetupEulaData(EULARuleType.CustomWST);
      _possibleEulaItems[EULARuleType.WST_WithMaint] = SetupEulaData(EULARuleType.WST_WithMaint);
      _possibleEulaItems[EULARuleType.WST_Update] = SetupEulaData(EULARuleType.WST_Update);
      _possibleEulaItems[EULARuleType.Training] = SetupEulaData(EULARuleType.Training);
      _possibleEulaItems[EULARuleType.Super] = SetupEulaData(EULARuleType.Super);
      _possibleEulaItems[EULARuleType.Reseller] = SetupEulaData(EULARuleType.Reseller);
      _possibleEulaItems[EULARuleType.premListing] = SetupEulaData(EULARuleType.premListing);
      _possibleEulaItems[EULARuleType.loadedDomain] = SetupEulaData(EULARuleType.loadedDomain);
      _possibleEulaItems[EULARuleType.dad] = SetupEulaData(EULARuleType.dad);
      _possibleEulaItems[EULARuleType.crm] = SetupEulaData(EULARuleType.crm);
      _possibleEulaItems[EULARuleType.survey_EULA] = SetupEulaData(EULARuleType.survey_EULA);
      _possibleEulaItems[EULARuleType.OutlookMail] = SetupEulaData(EULARuleType.OutlookMail);
      _possibleEulaItems[EULARuleType.WebStore] = SetupEulaData(EULARuleType.WebStore);
      _possibleEulaItems[EULARuleType.BusinessAccel] = SetupEulaData(EULARuleType.BusinessAccel);
      _possibleEulaItems[EULARuleType.AdSpace] = SetupEulaData(EULARuleType.AdSpace);
      _possibleEulaItems[EULARuleType.XXX] = SetupEulaData(EULARuleType.XXX);
      _possibleEulaItems[EULARuleType.InstantPage] = SetupEulaData(EULARuleType.InstantPage);
    }

    private bool isConfigured = false;

    public List<EULAItem> ConfiguredEULA
    {
      get
      {
        if (!isConfigured)
        {
          try
          {
            pagIdsUsed = new List<string>();
            _usedEulaItems = new List<EULAItem>();
            string pageID = string.Empty;
            if (_orderData.PrivateLabelId == 1695)
            {
              pageID = "domain_nameproxy";

            }
            else
            {
              pageID = "UTOS";
            }
            EULAItem currentItem = GetEULAData(pageID);
            string legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageID, "isc", "{isc}", "prog_id", _orderData.ProgId);
            currentItem.PageId = pageID;
            currentItem.LegalAgreementURL = legalAgreementURL;
            currentItem.ProductName = "Universal Terms of Service";
            AddUniqueEULAItem(ref _usedEulaItems, currentItem);
            BuildItemEulaList(ref _usedEulaItems);
            BuildAutoActivationEulaList(ref _usedEulaItems);
            isConfigured = true;
          }
          catch (System.Exception ex)
          {
            string message = ex.Message + "\n" + ex.StackTrace;
            AtlantisException aex = new AtlantisException("Error Setting up EULA", string.Empty, "0", message, string.Empty,
              _orderData.ShopperId, _orderData.OrderId, string.Empty, string.Empty, 0);
            Engine.Engine.LogAtlantisException(aex);
            isConfigured = true;
          }
        }
        return _usedEulaItems;
      }
    }

    #region ConfigureEULA
    private void BuildAutoActivationEulaList(ref List<EULAItem> eulaList)
    {
      XmlNode detailNode = _orderData.OrderXmlDoc.SelectSingleNode("/ORDER/ORDERDETAIL");
      XmlElement detailElement = (XmlElement)detailNode;
      string pfidList = detailElement.GetAttribute("_auto_activate_pfids");
      string[] uniquePFids = pfidList.Split(',');
      foreach (string uniquepfid in uniquePFids)
      {
        int productId = 0;
        int.TryParse(uniquepfid, out productId);
        XmlDocument agreements = GetEULAS(productId);
        XmlNodeList agreementList = agreements.SelectNodes("/data/item");
        foreach (XmlNode tempAgreement in agreementList)
        {
          XmlElement agreeElement = (XmlElement)tempAgreement;
          string pageID = agreeElement.GetAttribute("pageid");
          string title = agreeElement.GetAttribute("agreementName");
          EULAItem currentItem = GetEULAData(pageID);
          if (currentItem.RuleType == EULARuleType.Unknown)
          {
            string legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageID, "isc", "{isc}", "prog_id", _orderData.ProgId);
            currentItem.PageId = pageID;
            currentItem.LegalAgreementURL = legalAgreementURL;
            currentItem.ProductName = title;
          }
          if (ProductIds.isCloudServer(productId) || ProductIds.isHostingProduct(productId) || ProductIds.isWordPressHosting(productId))
          {
            currentItem = DetermineNameOverride(productId, currentItem);
          }
          AddUniqueEULAItem(ref eulaList, currentItem);
        }
      }
    }

    private EULAItem DetermineNameOverride(int productID, EULAItem currentItem)
    {
      if (ProductIds.isCloudServer(productID))
      {
        currentItem.ProductName = "Cloud Server";
      }
      if (ProductIds.isWordPressHosting(productID))
      {
        currentItem.ProductName = "WordPress Hosting";
      }
      else if (ProductIds.isHostingProduct(productID))
      {
        currentItem.ProductName = "Hosting";
      }
      return currentItem;
    }

    private void BuildItemEulaList(ref List<EULAItem> eulaList)
    {
      XmlNodeList itemNodes = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement itemElement in itemNodes)
      {
        string isDisplayedInCart = itemElement.GetAttribute("isdisplayedincart");
        bool shouldDisplay = isDisplayedInCart != "0";
        if (shouldDisplay)
        {
          int productId = 0;
          int.TryParse(itemElement.GetAttribute("_product_unifiedproductid"), out productId);
          XmlDocument agreements = GetEULAS(productId);
          XmlNodeList agreementList = agreements.SelectNodes("/data/item");
          foreach (XmlNode tempAgreement in agreementList)
          {
            XmlElement agreeElement = (XmlElement)tempAgreement;
            string pageID = agreeElement.GetAttribute("pageid");
            string title = agreeElement.GetAttribute("agreementName");
            EULAItem currentItem = GetEULAData(pageID);
            if (currentItem.RuleType == EULARuleType.Unknown)
            {
              string legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageID, "isc", "{isc}", "prog_id", _orderData.ProgId);
              currentItem.PageId = pageID;
              currentItem.LegalAgreementURL = legalAgreementURL;
              currentItem.ProductName = title;
            }
            if (ProductIds.isCloudServer(productId) || ProductIds.isHostingProduct(productId) || ProductIds.isWordPressHosting(productId))
            {
              currentItem = DetermineNameOverride(productId, currentItem);
            }
            AddUniqueEULAItem(ref eulaList, currentItem);
          }
          BuildItemEulaCustomRules(ref eulaList, itemElement);
        }
      }
    }

    private void BuildItemEulaCustomRules(ref List<EULAItem> eulaList, XmlElement itemElement)
    {
      int productTypeId = 0;
      int.TryParse(itemElement.GetAttribute("gdshop_product_typeid"), out productTypeId);

      int productId = 0;
      int.TryParse(itemElement.GetAttribute("_product_unifiedproductid"), out productId);

      string departmentId = itemElement.GetAttribute("dept_id");
      string name = itemElement.GetAttribute("name");

      string isDisplayedInCart = itemElement.GetAttribute("isdisplayedincart");
      bool shouldDisplay = isDisplayedInCart != "0";

      if (shouldDisplay)
      {

        switch (productTypeId)
        {
          case 2:
          case 3:
          case 4:
          case 5:
            string domain = itemElement.GetAttribute("domain");
            if (domain.Contains(".XXX"))
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.XXX), true, productId);
            }
            if (FreeBlogWithDomain)
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.QB), true, productId);
            }
            if (DataCache.DataCache.GetPLData(_orderData.PrivateLabelId, PLDataCategory.HostingOffer) == "1")
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Starter), true, productId);
            }
            if (DataCache.DataCache.GetPLData(_orderData.PrivateLabelId, PLDataCategory.EmailAccountOffer) == "1")
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.FreeWebmail), true, productId);
            }
            break;
          case 14:
          case 15:
            if (name.ToLowerInvariant().Contains("tonight"))
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.WST), true, productId);
            }
            else
            {
              if (name.ToLowerInvariant().Contains("quick"))
              {
                AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.QSC), true, productId);
              }
              else
              {
                AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Hosting), true, productId);
              }
            }
            break;
          case 16:
          case 17:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.WebMail), true, productId);
            break;
          case 19:
          case 20:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Transfer), true, productId);
            break;
          case 23:
          case 24:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Dbp), true, productId);
            break;
          case 38:
          case 41:
          case 45:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.DA), true, productId);
            break;
          case 39:
          case 40:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.TB), true, productId);
            break;
          case 42:
          case 63:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Csite), true, productId);
            break;
          case 86:
          case 87:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.VSDB), true, productId);
            break;
          case 88:
          case 89:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.EmailCounts), true, productId);
            break;
          case 140:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Whois), true, productId);
            break;
          case 132:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.QSC), true, productId);
            break;
          case 130:
            if (departmentId == "28" && name.ToLowerInvariant().Contains("update"))
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.WST_WithMaint), true, productId);
            }
            else
            {
              if ((departmentId == _departmentIds[DepartmentType.CustomSiteDeptId].ToString()) || (departmentId == "225"))
              {
                AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.CustomWST), true, productId);
              }
              else
              {
                if (departmentId == "36")
                {
                  AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.WebStore), true, productId);
                }
                else
                {
                  AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.WST), true, productId);
                }
              }
            }
            break;
          case 75:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.SSLCerts), true, productId);
            break;
          case 84:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.DedHostingIP), true, productId);
            break;
          case 96:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.FaxThruEmail), true, productId);
            break;
          case 64:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Merchant), true, productId);
            break;
          case 82:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.TrafficFacts), true, productId);
            break;
          case 54:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.SMTPRelay), true, productId);
            break;
          case 135:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.QB), true, productId);
            break;
          case 156:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.DOP), true, productId);
            break;
          case 159:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.CashPark), true, productId);
            break;
          case 303:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.CashParkHdr), true, productId);
            break;
          case 170:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.DotCert), true, productId);
            break;
          case 165:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.GiftCard), true, productId);
            break;
          case 175:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Broker), true, productId);
            break;
          case 107:
            if (name.ToLowerInvariant().Contains("ftp backup"))
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.FTP), true, productId);
            }
            if (name.ToLowerInvariant().Contains("assisted service plan"))
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.AssistedServices), true, productId);
            }
            break;
          case 173:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.EZPrint), true, productId);
            break;
          case 176:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Photo), true, productId);
            break;
          case 77:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Club), true, productId);
            break;
          case 187:
            switch (productId)
            {
              case ProductIds.LogoDesign:
              case ProductIds.DeluxeLogoDesign:
              case ProductIds.BusinessCardDesign:
              case ProductIds.LetterHeadDesign:
                AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Logo), true, productId);
                break;
              case ProductIds.CustomDesignBannerAnimated:
              case ProductIds.CustomDesignBannerStatic:
              case ProductIds.CustomDesignFavicon:
                AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Banner), true, productId);
                break;
            }
            break;
          case 181:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.CustomWST), true, productId);
            break;
          case 184:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.WST_Update), true, productId);
            break;
          case 18:
            if (departmentId == _departmentIds[DepartmentType.TrainingDeptId].ToString())
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Training), true, productId);
            }
            break;
          case 56:
            if (name.ToLowerInvariant().Contains("super reseller"))
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Super), true, productId);
            }
            else
            {
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.Reseller), true, productId);
            }
            break;
          case 199:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.premListing), true, productId);
            break;
          case 300:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.loadedDomain), true, productId);
            break;
          case 305:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.OutlookMail), true, productId);
            break;
          case 307:
          case 311:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.dad), true, productId);
            break;
          case 331:
          case 333:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.crm), true, productId);
            break;
          case 338:
            AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.survey_EULA), true, productId);
            break;
        }

        //extra check if EULA check for domain alert
        if (!eulaList.Contains(GetEULAData(EULARuleType.DA)))
        {
          switch (productId)
          {
            case ProductIds.domainAlert01Pk:
            case ProductIds.domainAlert100Pk:
            case ProductIds.domainAlertPLSub:
            case ProductIds.domainAlertBOrder:
            case ProductIds.domainAlertPrvBOrder:

            case ProductIds.domainAlert01PkRenewal:
            case ProductIds.domainAlert10PkRenewal:
            //case ProductIds.domainAlert100PkRenewal:    this productid is the same as ProductIds.domainAlert10PkRenewal, that's why this line is commented out.
            case ProductIds.domainAlertPLSubRenewal:
            case ProductIds.domainAlertBOrderRenewal:
            case ProductIds.domainAlertPrvBOrderRenewal:
            case ProductIds.domainAlertSTBOrder:

            case ProductIds.domainAlertMon:
            case ProductIds.domainAlertBOrderMon:
            case ProductIds.domainAlertMonRenewal:
            case ProductIds.domainAlertBOrderMonRenewal:
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.DA), true, productId);
              break;
          }
        }
        if (!eulaList.Contains(GetEULAData(EULARuleType.BusinessAccel)))
        {
          switch (productId)
          {
            case ProductIds.SEVBusinessAccel:
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.BusinessAccel), true, productId);
              break;
          }
        }

        if (!eulaList.Contains(GetEULAData(EULARuleType.AdSpace)))
        {
          switch (productId)
          {
            case ProductIds.AdSpaceEconomyMonth:
            case ProductIds.AdSpaceEconomyQuarterly:
            case ProductIds.AdSpaceEconomyYear:
            case ProductIds.AdSpaceEconomyTwoYears:
            case ProductIds.AdSpaceEconomyThreeYears:
            case ProductIds.AdSpaceDeluxeMonth:
            case ProductIds.AdSpaceDeluxeQuarterly:
            case ProductIds.AdSpaceDeluxeYear:
            case ProductIds.AdSpaceDeluxeTwoYears:
            case ProductIds.AdSpaceDeluxeThreeYears:
            case ProductIds.AdSpacePremiumMonth:
            case ProductIds.AdSpacePremiumQuarterly:
            case ProductIds.AdSpacePremiumYear:
            case ProductIds.AdSpacePremiumTwoYears:
            case ProductIds.AdSpacePremiumThreeYears:
            case ProductIds.AdSpaceEconomyRecurringMonth:
            case ProductIds.AdSpaceEconomyRecurringQuarterly:
            case ProductIds.AdSpaceEconomyRecurringYear:
            case ProductIds.AdSpaceEconomyRecurringTwoYears:
            case ProductIds.AdSpaceEconomyRecurringThreeYears:
            case ProductIds.AdSpaceDeluxeRecurringMonth:
            case ProductIds.AdSpaceDeluxeRecurringQuarterly:
            case ProductIds.AdSpaceDeluxeRecurringYear:
            case ProductIds.AdSpaceDeluxeRecurringTwoYears:
            case ProductIds.AdSpaceDeluxeRecurringThreeYears:
            case ProductIds.AdSpacePremiumRecurringMonth:
            case ProductIds.AdSpacePremiumRecurringQuarterly:
            case ProductIds.AdSpacePremiumRecurringYear:
            case ProductIds.AdSpacePremiumRecurringTwoYears:
            case ProductIds.AdSpacePremiumRecurringThreeYears:
              AddUniqueEULAItem(ref eulaList, GetEULAData(EULARuleType.AdSpace), true, productId);
              break;
          }
        }
      }
    }

    private bool AddUniqueEULAItem(ref List<EULAItem> eulaList, EULAItem currentItem)
    {
      bool added = false;
      if (!pagIdsUsed.Contains(currentItem.PageId.ToLower()))
      {
        eulaList.Add(currentItem);
        added = true;
        pagIdsUsed.Add(currentItem.PageId.ToLower());
      }
      return added;
    }

    private List<string> pagIdsUsed = new List<string>();

    private bool AddUniqueEULAItem(ref List<EULAItem> eulaList, EULAItem currentItem, bool logCustomRule, int productPfid)
    {
      bool added = AddUniqueEULAItem(ref eulaList, currentItem);
      if (logCustomRule && added)
      {
        string logPFIDEulaNotFound = DataCache.DataCache.GetAppSetting("CART_ACOS_EMAIL_EULA_LOG");
        if (logPFIDEulaNotFound == "true")
        {
          //Log Custom Rule Fired
          string message = string.Concat("Product EULA Not Found for:", productPfid, " PageID:", currentItem.PageId);
          AtlantisException aex = new AtlantisException("Product Email: EULA NotFound", string.Empty, "0", message, string.Empty,
            _orderData.ShopperId, _orderData.OrderId, string.Empty, string.Empty, 0);
          Engine.Engine.LogAtlantisException(aex);
          System.Diagnostics.Debug.WriteLine(message);
        }
      }
      return added;
    }

    public EULAItem GetEULAData(string pageId)
    {
      EULAItem tempItem = new EULAItem(string.Empty, string.Empty, string.Empty);
      if (ListPageIds.ContainsKey(pageId.ToLower()))
      {
        tempItem = ListPageIds[pageId.ToLower()];
      }
      return tempItem;
    }

    public EULAItem GetEULAData(EULARuleType EULARule)
    {
      EULAItem tempItem = new EULAItem(string.Empty, string.Empty, string.Empty);
      if (_possibleEulaItems.ContainsKey(EULARule))
      {
        tempItem = _possibleEulaItems[EULARule];
      }
      return tempItem;
    }

    private string DetermineHelpURL(string relativepath, string pageid, QueryParamMode queryParamMode, bool isSecure, params string[] additionalQueryParameters)
    {
      string helpLink = string.Empty;
      if (_orderData.IsGodaddy)
      {
        if (!relativepath.Contains("help/"))
        {
          helpLink = _links.GetUrl(LinkTypes.Community, "help/" + relativepath + pageid, QueryParamMode.CommonParameters, false, additionalQueryParameters);
        }
        else
        {
          helpLink = _links.GetUrl(LinkTypes.Community, relativepath + pageid, QueryParamMode.CommonParameters, false, additionalQueryParameters);
        }
      }
      else
      {
        helpLink = _links.GetUrl(LinkTypes.Help, relativepath + pageid, QueryParamMode.CommonParameters, false, additionalQueryParameters);
      }
      return helpLink;
    }

    private EULAItem SetupEulaData(EULARuleType EULARule)
    {

      string[] queryStringArgs = { "isc", "{isc}", "prog_id", _orderData.ProgId };
      string productName = string.Empty;
      string productInfoURL = string.Empty;
      string legalAgreementURL = string.Empty;
      string pageid = string.Empty;
      string ruleType = "Legal";
      EULAType agreementType = EULAType.Legal;
      switch (EULARule)
      {
        case EULARuleType.XXX:
          productName = ".XXX Domain Registration";
          pageid = "7333";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, pageid, QueryParamMode.CommonParameters, false, queryStringArgs);
          ruleType = "Article";
          break;
        case EULARuleType.BusinessAccel:
          productName = "Business Accelerator";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "5864", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Business_Accelerator";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Reg:
          productName = "Domain Registration";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "158", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "REG_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Transfer:
          productName = "Domain Transfer";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "160", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "TRANSFER_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Dbp:
          productName = "Private Registration";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "248", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "DOMAIN_NAMEPROXY";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Hosting:
          productName = "Hosting";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "4", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "HOSTING_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.QSC:
          productName = ProductNames.ShoppingCart;
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "267", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "QSC_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Csite:
          productName = "Online Copyright Registration";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "182", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "CSITE_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.EmailCounts:
          productName = "Express Email Marketing";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "185", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "QS_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.StealthRay: break;
        case EULARuleType.TB:
          productName = "Search Engine Visibility";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "736", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "TB_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.WSC:
          break;
        case EULARuleType.WST:
          productName = "WebSite Tonight&#174;";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "178", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "WST_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DedHostingIP:
          productName = "Dedicated Hosting IP";
          pageid = "1057";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, pageid, QueryParamMode.CommonParameters, false, queryStringArgs);
          ruleType = "Article";
          break;
        case EULARuleType.FaxThruEmail:
          productName = "Fax Thru Email";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "175", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "FAXEMAIL_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.SMTPRelay:
          productName = "SMTP Relay";
          pageid = "345";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, pageid, QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.Starter:
          productName = "Starter Web Page or For Sale Page";
          pageid = "825";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, pageid, QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.FreeHosting:
          productName = "Free Hosting w/Web Site Builder";
          pageid = "6304";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, pageid, QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.TrafficFacts:
          productName = "Site Analytics";
          pageid = "233";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, pageid, QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.SSLCerts:
          productName = "SSL Certificates";
          pageid = "186";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, pageid, QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.Merchant:
          productName = "Merchant Account";
          pageid = "237";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, pageid, QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.DedVirtHosting:
          productName = "Virtual Dedicated Server";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "1122", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "hosting_sa";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DedHosting:
          productName = "Dedicated Server";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "1122", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Dedicated Hosting SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.FreeWebmail:
          productName = "Free Personal Email";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "154", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "WEBMAIL_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Spam: break;
        case EULARuleType.VSDB:
          productName = "Online File Folder";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "261", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "VSDB_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DA:
          productName = "DomainAlert&#174;";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "247", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "DOMAIN_BACK";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.WebMail:
          productName = "Webmail";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "154", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "WEBMAIL_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.InstantPage:
          productName = "InstantPage&#174;";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "866", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "InstantPage_TOS";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.QB:
          productName = ProductNames.QuickBlogcast;
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "477", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "QUICKBLOG_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Whois:
          productName = "Business Registration";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "255", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Deluxe_WhoIs_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DOP:
          productName = "Domain Ownership Protection";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "614", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Domain_Protect_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.CashPark:
          productName = "CashParking";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "285", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Cash_Park_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.CashParkHdr:
          productName = "CashParking Custom Header";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "659", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Cash_Park_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.DotCert:
          productName = "Certified Domain";
          pageid = "297";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "297", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.GiftCard:
          productName = "Gift Card";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "302", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "gift_card";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Broker:
          productName = "Domain Buy Service";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "304", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Domain_Brokerage_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.FTP:
          productName = "FTP Backup";
          pageid = "1686";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "1686", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.EZPrint:
          productName = "Photo Store";
          pageid = "305";
          ruleType = "Article";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "305", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.Photo:
          productName = (_orderData.IsGodaddy) ? "Go Daddy Photo Album" : "Photo Album";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "340", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Photo_Prod_SA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Club:
          productName = "Discount Domain Club";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "2398", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "discountclub";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.AssistedServices:
          productName = "Assisted Service Plan";
          pageid = "2466";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "2466", QueryParamMode.CommonParameters, false, queryStringArgs);
          break;
        case EULARuleType.Logo:
          productName = "Logo Design";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "475", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "logo_design_sa";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Banner:
          productName = "Web Banner Design";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "475", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "web_banner_design_sa";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.CustomWST:
          productName = "Dream Website Design";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "449", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "custom_website";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.WST_WithMaint:
          productName = "Website Tonight w/Maintenance";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "449", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "WST_maintenance_eula";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.WST_Update:
          productName = "Website Update";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "449", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "website_update_sa";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Training:
          productName = "GoDaddy University terms of service:";
          pageid = "training";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.Super:
          productName = "Super Reseller";
          if (_orderData.ContextId == ContextIds.WildWestDomains)
          {
            productInfoURL = _links.GetUrl(LinkTypes.Starfield, "guides/getting_started_super_reseller.pdf");
          }
          else
          {
            productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "3340", QueryParamMode.CommonParameters, false, queryStringArgs);
          }
          pageid = "superreseller_eula";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.Reseller:
          productName = "Reseller Plans";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "220", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "reseller_sa";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Service;
          break;
        case EULARuleType.premListing:
          productName = "Premium Domain Name";
          productInfoURL = DetermineHelpURL(ARTICLE_RELATIVE_PATH, "3497", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Premium_Domain";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          agreementType = EULAType.Membership;
          break;
        case EULARuleType.loadedDomain:
          productName = "SmartSpace&#174;";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "686", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "LOADED_DOMAIN";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.dad:
          productName = "Power Content Plans";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "680", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Domain_Development";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.crm:
          productName = (_orderData.IsGodaddy) ? "Go Daddy Contact Manager" : "Contact Manager";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "717", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "Customer_Manager_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.survey_EULA:
          productName = (_orderData.IsGodaddy) ? "Go Daddy Site Surveys" : "Site Surveys";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "725", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "site_survey_EULA";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.OutlookMail:
          productName = "Hosted Exchange Email";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "663", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "hosted_exchange";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.WebStore:
          productName = "Web Store Design";
          productInfoURL = DetermineHelpURL(TOPIC_RELATIVE_PATH, "735", QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "webstore_design_svc_and_mtce_agmt";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
        case EULARuleType.AdSpace:
          productName = "Ad Space";
          productInfoURL = DetermineHelpURL("help/" + ARTICLE_RELATIVE_PATH, "6161",
                                         QueryParamMode.CommonParameters, false, queryStringArgs);
          pageid = "AdSpace_TOS";
          legalAgreementURL = _links.GetUrl(LinkTypes.SiteRoot, LEGAL_RELATIVE_PATH, QueryParamMode.CommonParameters, true, "pageid", pageid, "isc", "{isc}", "prog_id", _orderData.ProgId);
          break;
      }

      EULAItem eulaItem = new EULAItem(productName, productInfoURL, legalAgreementURL);
      eulaItem.AgreementType = agreementType;
      eulaItem.PageId = pageid;
      eulaItem.Agreement_Url_Type = ruleType;
      eulaItem.RuleType = EULARule;
      ListPageIds[pageid.ToLower()] = eulaItem;
      return eulaItem;
    }

    #endregion
    #region DataAccess
    private XmlDocument GetEULAS(int pfID)
    {
      XmlDocument results = new XmlDocument();
      try
      {

        string query = "<GetTermsOfServiceByProduct><param name=\"pf_id\" value=\"{0}\"/></GetTermsOfServiceByProduct>";
        string dcQuery = string.Format(query, pfID);
        string response = DataCache.DataCache.GetCacheData(dcQuery);
        results.LoadXml(response);
      }
      catch (Exception ex)
      {
        string message = ex.Message + System.Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException("EulaRules.GetEULAS", string.Empty, "0",
          message, "ShopperId=" + _orderData.ShopperId, _orderData.ShopperId, _orderData.OrderId,
          string.Empty, string.Empty, 0);
        Engine.Engine.LogAtlantisException(aex);
      }
      return results;
    }

    #endregion

    #region SpecializedRules
    private bool FreeBlogWithDomain
    {
      get
      {
        return (_products.IsProductGroupOffered(ProductGroups.QuickBlogcast)) &&
          (DataCache.DataCache.GetPLData(_orderData.PrivateLabelId, PLDataCategory.BlogOffer) == "1");
      }
    }

    #endregion
  }
}
