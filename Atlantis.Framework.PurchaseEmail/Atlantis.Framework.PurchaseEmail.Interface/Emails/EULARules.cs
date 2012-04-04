using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class EULARules
  {
    private Dictionary<EULARuleType, bool> _applicableEULADictionary;
    public Dictionary<EULARuleType, bool> ApplicableEULADictionary
    {
      get { return _applicableEULADictionary; }
    }

    OrderData _orderData;
    DepartmentIds _departmentIds;
    IProductProvider _products; 

    public EULARules(OrderData orderData, DepartmentIds departmentIds, IProductProvider products)
    {
      _orderData = orderData;
      _departmentIds = departmentIds;
      _products = products;
      BuildApplicableEULADictionary();
    }

    private bool FreeBlogWithDomain
    {
      get
      {
        return (_products.IsProductGroupOffered(ProductGroups.QuickBlogcast)) &&
          (DataCache.DataCache.GetPLData(_orderData.PrivateLabelId, PLDataCategory.BlogOffer) == "1");
      }
    }

    private void BuildEULADictionaryAutoActivate()
    {
      XmlNode detailNode = _orderData.OrderXmlDoc.SelectSingleNode("/ORDER/ORDERDETAIL");
      XmlElement detailElement = (XmlElement)detailNode;
      string pfidList = detailElement.GetAttribute("_auto_activate_pfids");
      string[] uniquePFids = pfidList.Split(',');
      foreach (string uniquepfid in uniquePFids)
      {
        switch (uniquepfid)
        {
          case "1865":
          case "6413":
          case "7413":
            _applicableEULADictionary[EULARuleType.WebMail] = true;
            break;
          case "70761":
            _applicableEULADictionary[EULARuleType.InstantPage] = true;
            break;
        }
      }
    }

    private void BuildApplicableEULADictionary()
    {
      _applicableEULADictionary = new Dictionary<EULARuleType, bool>();
      BuildEULADictionaryAutoActivate();
      XmlNodeList itemNodes = _orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
      foreach (XmlElement itemElement in itemNodes)
      {
        string isDisplayedInCart = itemElement.GetAttribute("isdisplayedincart");
        bool shouldDisplay = isDisplayedInCart != "0";
        if (shouldDisplay)
        {

          int productTypeId = 0;
          int.TryParse(itemElement.GetAttribute("gdshop_product_typeid"), out productTypeId);

          int productId = 0;
          int.TryParse(itemElement.GetAttribute("_product_unifiedproductid"), out productId);

          string departmentId = itemElement.GetAttribute("dept_id");
          string name = itemElement.GetAttribute("name");

          switch (productTypeId)
          {
            case 2:
            case 3:
            case 4:
            case 5:
              _applicableEULADictionary[EULARuleType.Reg] = true;
              string domain = itemElement.GetAttribute("domain");
              if (domain.Contains(".XXX"))
              {
                _applicableEULADictionary[EULARuleType.XXX] = true;
              }
              if (FreeBlogWithDomain)
              {
                _applicableEULADictionary[EULARuleType.QB] = true;
              }
              if (DataCache.DataCache.GetPLData(_orderData.PrivateLabelId, PLDataCategory.HostingOnlyOffer) == "1")
              {
                _applicableEULADictionary[EULARuleType.FreeHosting] = true;
              }
              if (DataCache.DataCache.GetPLData(_orderData.PrivateLabelId, PLDataCategory.HostingOffer) == "1")
              {
                _applicableEULADictionary[EULARuleType.Starter] = true;
              }
              if (DataCache.DataCache.GetPLData(_orderData.PrivateLabelId, PLDataCategory.EmailAccountOffer) == "1")
              {
                _applicableEULADictionary[EULARuleType.FreeWebmail] = true;
              }
              break;
            case 14:
            case 15:
              if (name.ToLowerInvariant().Contains("tonight"))
              {
                _applicableEULADictionary[EULARuleType.WST] = true;
              }
              else
              {
                if (name.ToLowerInvariant().Contains("quick"))
                {
                  _applicableEULADictionary[EULARuleType.QSC] = true;
                }
                else
                {
                  _applicableEULADictionary[EULARuleType.Hosting] = true;
                }
              }
              break;
            case 16:
            case 17:
              _applicableEULADictionary[EULARuleType.WebMail] = true;
              break;
            case 19:
            case 20:
              _applicableEULADictionary[EULARuleType.Transfer] = true;
              break;
            case 23:
            case 24:
              _applicableEULADictionary[EULARuleType.Dbp] = true;
              break;
            case 38:
            case 41:
            case 45:
              _applicableEULADictionary[EULARuleType.DA] = true;
              break;
            case 39:
            case 40:
              _applicableEULADictionary[EULARuleType.TB] = true;
              break;
            case 42:
            case 63:
              _applicableEULADictionary[EULARuleType.Csite] = true;
              break;
            case 86:
            case 87:
              _applicableEULADictionary[EULARuleType.VSDB] = true;
              break;
            case 88:
            case 89:
              _applicableEULADictionary[EULARuleType.EmailCounts] = true;
              break;
            case 140:
              _applicableEULADictionary[EULARuleType.Whois] = true;
              break;
            case 132:
              _applicableEULADictionary[EULARuleType.QSC] = true;
              break;
            case 130:
              if (departmentId == "28" && name.ToLowerInvariant().Contains("update"))
              {
                _applicableEULADictionary[EULARuleType.WST_WithMaint] = true;
              }
              else
              {
                if ((departmentId == _departmentIds[DepartmentType.CustomSiteDeptId].ToString()) || (departmentId == "225"))
                {
                  _applicableEULADictionary[EULARuleType.CustomWST] = true;
                }
                else
                {
                  if (departmentId == "36")
                  {
                    _applicableEULADictionary[EULARuleType.WebStore] = true;
                  }
                  else
                  {
                    _applicableEULADictionary[EULARuleType.WST] = true;
                  }
                }
              }
              break;
            case 75:
              _applicableEULADictionary[EULARuleType.SSLCerts] = true;
              break;
            case 84:
              _applicableEULADictionary[EULARuleType.DedHostingIP] = true;
              break;
            case 96:
              _applicableEULADictionary[EULARuleType.FaxThruEmail] = true;
              break;
            case 64:
              _applicableEULADictionary[EULARuleType.Merchant] = true;
              break;
            case 82:
              _applicableEULADictionary[EULARuleType.TrafficFacts] = true;
              break;
            case 54:
              _applicableEULADictionary[EULARuleType.SMTPRelay] = true;
              break;
            case 135:
              _applicableEULADictionary[EULARuleType.QB] = true;
              break;
            case 156:
              _applicableEULADictionary[EULARuleType.DOP] = true;
              break;
            case 159:
              _applicableEULADictionary[EULARuleType.CashPark] = true;
              break;
            case 303:
              _applicableEULADictionary[EULARuleType.CashParkHdr] = true;
              break;
            case 170:
              _applicableEULADictionary[EULARuleType.DotCert] = true;
              break;
            case 165:
              _applicableEULADictionary[EULARuleType.GiftCard] = true;
              break;
            case 175:
              _applicableEULADictionary[EULARuleType.Broker] = true;
              break;
            case 107:
              if (name.ToLowerInvariant().Contains("ftp backup"))
              {
                _applicableEULADictionary[EULARuleType.FTP] = true;
              }
              if (name.ToLowerInvariant().Contains("assisted service plan"))
              {
                _applicableEULADictionary[EULARuleType.AssistedServices] = true;
              }
              break;
            case 173:
              _applicableEULADictionary[EULARuleType.EZPrint] = true;
              break;
            case 176:
              _applicableEULADictionary[EULARuleType.Photo] = true;
              break;
            case 77:
              _applicableEULADictionary[EULARuleType.Club] = true;
              break;
            case 187:
              switch (productId)
              {
                case ProductIds.LogoDesign:
                case ProductIds.DeluxeLogoDesign:
                case ProductIds.BusinessCardDesign:
                case ProductIds.LetterHeadDesign:

                  _applicableEULADictionary[EULARuleType.Logo] = true;
                  break;
                case ProductIds.CustomDesignBannerAnimated:
                case ProductIds.CustomDesignBannerStatic:
                case ProductIds.CustomDesignFavicon:
                  _applicableEULADictionary[EULARuleType.Banner] = true;
                  break;
              }
              break;
            case 181:
              _applicableEULADictionary[EULARuleType.CustomWST] = true;
              break;
            case 184:
              _applicableEULADictionary[EULARuleType.WST_Update] = true;
              break;
            case 18:
              if (departmentId == _departmentIds[DepartmentType.TrainingDeptId].ToString())
              {
                _applicableEULADictionary[EULARuleType.Training] = true;
              }
              break;
            case 56:
              if (name.ToLowerInvariant().Contains("super reseller"))
              {
                _applicableEULADictionary[EULARuleType.Super] = true;
              }
              else
              {
                _applicableEULADictionary[EULARuleType.Reseller] = true;
              }
              break;
            case 199:
              _applicableEULADictionary[EULARuleType.premListing] = true;
              break;
            case 300:
              _applicableEULADictionary[EULARuleType.loadedDomain] = true;
              break;
            case 305:
              _applicableEULADictionary[EULARuleType.OutlookMail] = true;
              break;
            case 307:
            case 311:
              _applicableEULADictionary[EULARuleType.dad] = true;
              break;
            case 331:
            case 333:
              _applicableEULADictionary[EULARuleType.crm] = true;
              break;
            case 338:
              _applicableEULADictionary[EULARuleType.survey_EULA] = true;
              break;
          }

          //extra check if EULA check for domain alert
          if (!_applicableEULADictionary.ContainsKey(EULARuleType.DA))
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
                _applicableEULADictionary[EULARuleType.DA] = true;
                break;
            }
          }
          if (!_applicableEULADictionary.ContainsKey(EULARuleType.BusinessAccel))
          {
            switch (productId)
            {
              case ProductIds.SEVBusinessAccel:
                _applicableEULADictionary[EULARuleType.BusinessAccel] = true;
                break;
            }
          }
          if (!_applicableEULADictionary.ContainsKey(EULARuleType.AdSpace))
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
                _applicableEULADictionary[EULARuleType.AdSpace] = true;
                break;
            }
          }
        }
      }
    }
  }
}
