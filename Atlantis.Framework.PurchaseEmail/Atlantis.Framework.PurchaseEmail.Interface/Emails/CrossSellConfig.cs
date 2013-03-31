using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class CrossSellConfig
  {
    CrossSellConfigId _id;
    string _description;
    List<CrossSellConfigProductId> _productsToShow;
    List<CrossSellConfigProductId> _crossSellProductList;
    int _ciCode;
    string _itemTrackingCode;

    DepartmentIds _departmentIds;
    OrderData _orderData;
    IProductProvider _products;
    ShopperProductProvider _shopperProducts;

    public CrossSellConfigId Id { get { return _id; } }
    public string Description { get { return _description; } }
    public List<CrossSellConfigProductId> CrossSellProductList { get { return _crossSellProductList; } }
    public int CiCode { get { return _ciCode; } }
    public string ItemTrackingCode { get { return _itemTrackingCode; } }

    public CrossSellConfig(OrderData orderData, DepartmentIds departmentIds, IProviderContainer container)
    {
      _orderData = orderData;
      _departmentIds = departmentIds;

      _products = container.Resolve<IProductProvider>();
      _shopperProducts = container.Resolve<ShopperProductProvider>();

      _productsToShow = new List<CrossSellConfigProductId>();
      _crossSellProductList = new List<CrossSellConfigProductId>();

      //determine config id to use
      _id = GetCrossSellConfigId();

      //get config data
      GetCrossSellConfigData();

      //validate config data
      ValidateProductsToShow();
    }

    private CrossSellConfigId GetCrossSellConfigId()
    {
      CrossSellConfigId configId = CrossSellConfigId.ec_default;

      int firstProductTypeId = 0;
      XmlElement firstItem = _orderData.OrderXmlDoc.SelectSingleNode("/ORDER/ITEMS/ITEM") as XmlElement;
      if (firstItem != null)
      {
        int.TryParse(firstItem.GetAttribute("gdshop_product_typeid"), out firstProductTypeId);
      }

      switch (firstProductTypeId)
      {
        case 2:
        case 3:
        case 4:
        case 5:
        case 19:
        case 20:
        case 23:
        case 24:
        case 38:
        case 41:
        case 140:
        case 135:
          configId = CrossSellConfigId.ec_domain;
          break;
        case 14:
        case 15:
          if (firstItem.Name.ToLowerInvariant().Contains("tonight"))
          {
            configId = CrossSellConfigId.ec_WST;
          }
          else
          {
            if (firstItem.Name.ToLowerInvariant().Contains("quick"))
            {
              configId = CrossSellConfigId.ec_qsc;
            }
            else
            {
              configId = CrossSellConfigId.ec_hosting;
            }
          }
          break;
        case 16:
        case 17:
          configId = CrossSellConfigId.ec_email;
          break;
        case 39:
        case 40:
          configId = CrossSellConfigId.ec_tb;
          break;
        case 42:
        case 63:
          configId = CrossSellConfigId.ec_cSite;
          break;
        case 49:
          configId = CrossSellConfigId.ec_email;
          break;
        case 50:
        case 51:
        case 52:
          configId = CrossSellConfigId.ec_default;
          break;
        case 86:
        case 87:
          configId = CrossSellConfigId.ec_off;
          break;
        case 88:
        case 89:
          configId = CrossSellConfigId.ec_eem;
          break;
        case 132:
          configId = CrossSellConfigId.ec_qsc;
          break;
        case 130:

          if (firstItem.GetAttribute("dept_id") == _departmentIds[DepartmentType.CustomSiteDeptId].ToString())
            configId = CrossSellConfigId.ec_customsite;
          else
            configId = CrossSellConfigId.ec_WST;
          break;
        case 75:
          configId = CrossSellConfigId.ec_ssl;
          break;
        case 84:
          configId = CrossSellConfigId.ec_dedicatedip;
          break;
        case 96:
          configId = CrossSellConfigId.ec_FTE;
          break;
        case 64:
          configId = CrossSellConfigId.ec_merchacct;
          break;
        case 103:
          configId = CrossSellConfigId.ec_default;
          break;
        case 82:
          configId = CrossSellConfigId.ec_tf;
          break;
        case 54:
          configId = CrossSellConfigId.ec_email;
          break;
        case 98:
          configId = CrossSellConfigId.ec_servers;
          break;
        case 114:
          configId = CrossSellConfigId.ec_dnamember;
          break;
        case 18:
          configId = CrossSellConfigId.ec_photocd;
          break;
        case 126:
          configId = CrossSellConfigId.ec_OGC;
          break;
        case 138:
        case 139:
          configId = CrossSellConfigId.ec_appraisal;
          break;
        case 56: // resellers
          configId = CrossSellConfigId.ec_appraisal;
          break;
        default:
          if (firstItem.Name.ToLowerInvariant().Contains("basic"))
          {
            configId = CrossSellConfigId.ec_basic;
            break;
          }
          if (firstItem.Name.ToLowerInvariant().Contains("pro"))
          {
            configId = CrossSellConfigId.ec_pro;
            break;
          }
          if (firstItem.Name.ToLowerInvariant().Contains("super"))
          {
            configId = CrossSellConfigId.ec_super;
            break;
          }
          if (firstItem.Name.ToLowerInvariant().Contains("api"))
          {
            configId = CrossSellConfigId.ec_api;
            break;
          }
          configId = CrossSellConfigId.ec_reseller;
          break;
      }

      return configId;
    }

    private void GetCrossSellConfigData()
    {
      string callXML = string.Format("<GetCrossSellConfig><param name=\"configurationID\" value=\"{0}\"/></GetCrossSellConfig>", _id.ToString());
      string dataXml = DataCache.DataCache.GetCacheData(callXML);

      if (!string.IsNullOrEmpty(dataXml))
      {
        try
        {
          XDocument dataDoc = XDocument.Parse(dataXml);
          XElement itemElement = dataDoc.Descendants("item").First();

          _description = itemElement.Attribute("configurationDescription").Value;
          _itemTrackingCode = itemElement.Attribute("itemTrackingCode").Value;
          _ciCode = int.Parse(itemElement.Attribute("clickImpressionCode").Value);
          string productsToShow = itemElement.Attribute("showOnly").Value;

          if (!string.IsNullOrEmpty(productsToShow))
          {
            string[] productsToShowParts = productsToShow.Split("|".ToCharArray());
            foreach (string strCrossSellProductId in productsToShowParts)
            {
              int intCrossSellProductId = -1;
              if (!int.TryParse(strCrossSellProductId, out intCrossSellProductId))
              {
                intCrossSellProductId = -1;
              }
              if (intCrossSellProductId != -1)
              {
                if (Enum.IsDefined(typeof(CrossSellConfigProductId), intCrossSellProductId))
                {
                  CrossSellConfigProductId productId = (CrossSellConfigProductId)intCrossSellProductId;
                  _productsToShow.Add(productId);
                }
              }
            }
          }

        }
        catch (Exception ex)
        {
          AtlantisException exception = new AtlantisException("PurchaseEmail.CrossSellConfig.GetCrossSellConfigData", "0", ex.Message + ex.StackTrace, dataXml, null, null);
          Engine.Engine.LogAtlantisException(exception);
        }

      }
    }

    private void ValidateProductsToShow()
    {
      bool isValid;
      foreach (CrossSellConfigProductId productId in _productsToShow)
      {
        isValid = false;
        switch (productId)
        {
          case CrossSellConfigProductId.COLD_FUSION:
            if (_products.IsProductGroupOffered(ProductGroups.Hosting) && !_shopperProducts.HasProductNamespace("hosting"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.LOGODESIGN:
            if (_products.IsProductGroupOffered(ProductGroups.Logo) && !_shopperProducts.HasProductId(ProductIds.LogoDesign))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.TRAFFIC_FACTS:
            if (_products.IsProductGroupOffered(ProductGroups.Hosting) && !_shopperProducts.HasProductNamespace("deluxestat"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.HOSTING:
            if (_products.IsProductGroupOffered(ProductGroups.Hosting) && !_shopperProducts.HasProductNamespace("hosting"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.WST:
            if (_products.IsProductGroupOffered(ProductGroups.WebSiteTonight)
              && !_shopperProducts.HasProductId(ProductIds.website01pg)
              && !_shopperProducts.HasProductId(ProductIds.website05pg)
              && !_shopperProducts.HasProductId(ProductIds.WST_E_1year)
              && !_shopperProducts.HasProductId(ProductIds.WST_E_2year)
              && !_shopperProducts.HasProductId(ProductIds.WST_E_3year)
              && !_shopperProducts.HasProductId(ProductIds.WST_E_4year)
              && !_shopperProducts.HasProductId(ProductIds.WST_E_5year)
              && !_shopperProducts.HasProductId(ProductIds.website10pg)
              && !_shopperProducts.HasProductId(ProductIds.WST_D_1year)
              && !_shopperProducts.HasProductId(ProductIds.WST_D_2year)
              && !_shopperProducts.HasProductId(ProductIds.WST_D_3year)
              && !_shopperProducts.HasProductId(ProductIds.WST_D_4year)
              && !_shopperProducts.HasProductId(ProductIds.WST_D_5year)
              && !_shopperProducts.HasProductId(ProductIds.website20pg)
              && !_shopperProducts.HasProductId(ProductIds.WST_P_1year)
              && !_shopperProducts.HasProductId(ProductIds.WST_P_2year)
              && !_shopperProducts.HasProductId(ProductIds.WST_P_3year)
              && !_shopperProducts.HasProductId(ProductIds.WST_P_4year)
              && !_shopperProducts.HasProductId(ProductIds.WST_P_5year)
               )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.TRAFFICB:
            if (_products.IsProductGroupOffered(ProductGroups.TrafficBlazer) && !_shopperProducts.HasProductNamespace("trafblazer"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.EMAIL:
          case CrossSellConfigProductId.OFF:
            if (_products.IsProductGroupOffered(ProductGroups.Email) && !_shopperProducts.HasProductNamespace("email"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.FAXTHRU_EMAIL:
            if (_products.IsProductGroupOffered(ProductGroups.FaxThruEmail)
              && !_shopperProducts.HasProductId(ProductIds.FTE_Local_E)
              && !_shopperProducts.HasProductId(ProductIds.FTE_Local_D)
              && !_shopperProducts.HasProductId(ProductIds.FTE_Local_P)
              && !_shopperProducts.HasProductId(ProductIds.FTE_Toll_E)
              && !_shopperProducts.HasProductId(ProductIds.FTE_Toll_D)
              && !_shopperProducts.HasProductId(ProductIds.FTE_Toll_P)
            )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.EEM:
            if (_products.IsProductGroupOffered(ProductGroups.ExpressEmailMarketing)
              && !_shopperProducts.HasProductId(ProductIds.OGC_E)
              && !_shopperProducts.HasProductId(ProductIds.OGC_D)
              && !_shopperProducts.HasProductId(ProductIds.OGC_P)
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.GROUP_CALENDAR:
            if (_products.IsProductGroupOffered(ProductGroups.OnlineGroupCalendar)
              && !_shopperProducts.HasProductId(ProductIds.Calendar_E)
              && !_shopperProducts.HasProductId(ProductIds.Calendar_D)
              && !_shopperProducts.HasProductId(ProductIds.Calendar_P)
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.SSL_TURBO:
          case CrossSellConfigProductId.SSL_HIGH:
            if (_products.IsProductGroupOffered(ProductGroups.SSLCertificates) && !_shopperProducts.HasProductNamespace("sslcert"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.CART:
            if (_products.IsProductGroupOffered(ProductGroups.QuickShoppingCart)
              && !_shopperProducts.HasProductId(ProductIds.Cart_E_Monthly)
              && !_shopperProducts.HasProductId(ProductIds.Cart_D_Monthly)
              && !_shopperProducts.HasProductId(ProductIds.Cart_P_Monthly)
              && !_shopperProducts.HasProductId(ProductIds.Cart_E_1year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_E_2year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_E_3year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_E_4year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_E_5year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_D_1year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_D_2year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_D_3year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_D_4year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_D_5year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_P_1year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_P_2year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_P_3year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_P_4year)
              && !_shopperProducts.HasProductId(ProductIds.Cart_P_5year)
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.CSITE:
            if (_products.IsProductGroupOffered(ProductGroups.CSite) && !_shopperProducts.HasProductNamespace("ideareg"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.DNA:
            if (_products.IsProductGroupOffered(ProductGroups.DomainNameAftermarket)
              && !_shopperProducts.HasProductId(ProductIds.DNA_DomainPurchase)
              && !_shopperProducts.HasProductId(ProductIds.DNA_ManagedOffer)
              && !_shopperProducts.HasProductId(ProductIds.DNA_ManagedAuction)
              && !_shopperProducts.HasProductId(ProductIds.DNA_TransAssuredOffer)
              && !_shopperProducts.HasProductId(ProductIds.DNA_PrivateOffer)
              && !_shopperProducts.HasProductId(ProductIds.DNA_PrivateAuction)
              && !_shopperProducts.HasProductId(ProductIds.DNA_TransAssuredAuction)
              && !_shopperProducts.HasProductId(ProductIds.DNA_Subscription)
              && !_shopperProducts.HasProductId(ProductIds.DNA_SubscriptionRenewal)
              && !_shopperProducts.HasProductId(ProductIds.DNA_SubscriptionMonitorBundle)
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.MERCH_ACCT:
            if (_products.IsProductGroupOffered(ProductGroups.MerchantAccount) && !_shopperProducts.HasProductNamespace("merchacct"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.PRO_RESELLER:
            if (_products.IsProductGroupOffered(ProductGroups.InstantReseller)
              && !_shopperProducts.HasProductId(ProductIds.Reseller)
              && !_shopperProducts.HasProductId(ProductIds.Reseller_Renewal)
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.SUPER_RESELLER:
            if (_products.IsProductGroupOffered(ProductGroups.InstantReseller)
              && !_shopperProducts.HasProductId(ProductIds.ResellerPro)
              && !_shopperProducts.HasProductId(ProductIds.ResellerPro_Renewal)
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.PRIVATE_DOMAINS:
            if (_products.IsProductGroupOffered(ProductGroups.PrivateRegistrations) && !_shopperProducts.HasProductNamespace("domains"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.DELUXE_WHOIS:
            if (_products.IsProductGroupOffered(ProductGroups.BusinessRegistration) && !_shopperProducts.HasProductNamespace("domains"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.APPRAISAL:
            if (_products.IsProductGroupOffered(ProductGroups.DomainNameAppraisal) && !_shopperProducts.HasProductNamespace("domains"))
            {
              isValid = true;
            }
            break;
        }
        if (isValid)
        {
          _crossSellProductList.Add(productId);
          if (_crossSellProductList.Count >= 3)
          {
            break;
          }
        }
      }
    }
  }
}
