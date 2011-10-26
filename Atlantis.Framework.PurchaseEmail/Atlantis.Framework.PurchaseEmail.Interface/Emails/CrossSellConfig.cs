using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Atlantis.Framework.DataProvider.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.PurchaseEmail.Interface.Providers;

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

    public CrossSellConfigId Id { get { return _id; } }
    public string Description { get { return _description; } }
    public List<CrossSellConfigProductId> CrossSellProductList { get { return _crossSellProductList; } }
    public int CiCode { get { return _ciCode; } }
    public string ItemTrackingCode { get { return _itemTrackingCode; } }

    public CrossSellConfig(OrderData orderData, DepartmentIds departmentIds, IProductProvider products)
    {
      _orderData = orderData;
      _departmentIds = departmentIds;
      _products = products;

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
      DataTable dataTable = DataCache.DataCache.GetCacheDataTable(callXML);
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        _description = (string)dataTable.Rows[0]["configurationDescription"];
        _itemTrackingCode = (string)dataTable.Rows[0]["itemTrackingCode"];
        _ciCode = int.Parse((string)dataTable.Rows[0]["clickImpressionCode"]);
        string productsToShow = (string)dataTable.Rows[0]["showOnly"];
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
    }

    private void ValidateProductsToShow()
    {
      Dictionary<string, int> shopperProductData = GetShopperProductDictionary(_orderData.ShopperId, _orderData.PrivateLabelId.ToString(), true);
      bool isValid;
      foreach (CrossSellConfigProductId productId in _productsToShow)
      {
        isValid = false;
        switch (productId)
        {
          case CrossSellConfigProductId.COLD_FUSION:
            if (_products.IsProductGroupOffered(ProductGroups.Hosting) && !shopperProductData.ContainsKey("hosting"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.LOGODESIGN:
            if (_products.IsProductGroupOffered(ProductGroups.Logo) && !shopperProductData.ContainsKey(ProductIds.LogoDesign.ToString()))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.TRAFFIC_FACTS:
            if (_products.IsProductGroupOffered(ProductGroups.Hosting) && !shopperProductData.ContainsKey("deluxestat"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.HOSTING:
            if (_products.IsProductGroupOffered(ProductGroups.Hosting) && !shopperProductData.ContainsKey("hosting"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.WST:
            if (_products.IsProductGroupOffered(ProductGroups.WebSiteTonight)
              && !shopperProductData.ContainsKey(ProductIds.website01pg.ToString())
              && !shopperProductData.ContainsKey(ProductIds.website05pg.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_E_1year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_E_2year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_E_3year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_E_4year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_E_5year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.website10pg.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_D_1year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_D_2year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_D_3year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_D_4year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_D_5year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.website20pg.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_P_1year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_P_2year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_P_3year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_P_4year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.WST_P_5year.ToString())
               )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.TRAFFICB:
            if (_products.IsProductGroupOffered(ProductGroups.TrafficBlazer) && !shopperProductData.ContainsKey("trafblazer"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.EMAIL:
          case CrossSellConfigProductId.OFF:
            if (_products.IsProductGroupOffered(ProductGroups.Email) && !shopperProductData.ContainsKey("email"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.FAXTHRU_EMAIL:
            if (_products.IsProductGroupOffered(ProductGroups.FaxThruEmail)
              && !shopperProductData.ContainsKey(ProductIds.FTE_Local_E.ToString())
              && !shopperProductData.ContainsKey(ProductIds.FTE_Local_D.ToString())
              && !shopperProductData.ContainsKey(ProductIds.FTE_Local_P.ToString())
              && !shopperProductData.ContainsKey(ProductIds.FTE_Toll_E.ToString())
              && !shopperProductData.ContainsKey(ProductIds.FTE_Toll_D.ToString())
              && !shopperProductData.ContainsKey(ProductIds.FTE_Toll_P.ToString())
            )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.EEM:
            if (_products.IsProductGroupOffered(ProductGroups.ExpressEmailMarketing)
              && !shopperProductData.ContainsKey(ProductIds.OGC_E.ToString())
              && !shopperProductData.ContainsKey(ProductIds.OGC_D.ToString())
              && !shopperProductData.ContainsKey(ProductIds.OGC_P.ToString())
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.GROUP_CALENDAR:
            if (_products.IsProductGroupOffered(ProductGroups.OnlineGroupCalendar)
              && !shopperProductData.ContainsKey(ProductIds.Calendar_E.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Calendar_D.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Calendar_P.ToString())
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.SSL_TURBO:
          case CrossSellConfigProductId.SSL_HIGH:
            if (_products.IsProductGroupOffered(ProductGroups.SSLCertificates) && !shopperProductData.ContainsKey("sslcert"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.CART:
            if (_products.IsProductGroupOffered(ProductGroups.QuickShoppingCart)
              && !shopperProductData.ContainsKey(ProductIds.Cart_E_Monthly.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_D_Monthly.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_P_Monthly.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_E_1year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_E_2year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_E_3year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_E_4year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_E_5year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_D_1year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_D_2year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_D_3year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_D_4year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_D_5year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_P_1year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_P_2year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_P_3year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_P_4year.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Cart_P_5year.ToString())
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.CSITE:
            if (_products.IsProductGroupOffered(ProductGroups.CSite) && !shopperProductData.ContainsKey("ideareg"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.DNA:
            if (_products.IsProductGroupOffered(ProductGroups.DomainNameAftermarket)
              && !shopperProductData.ContainsKey(ProductIds.DNA_DomainPurchase.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_ManagedOffer.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_ManagedAuction.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_TransAssuredOffer.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_PrivateOffer.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_PrivateAuction.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_TransAssuredAuction.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_Subscription.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_SubscriptionRenewal.ToString())
              && !shopperProductData.ContainsKey(ProductIds.DNA_SubscriptionMonitorBundle.ToString())
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.MERCH_ACCT:
            if (_products.IsProductGroupOffered(ProductGroups.MerchantAccount) && !shopperProductData.ContainsKey("merchacct"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.PRO_RESELLER:
            if (_products.IsProductGroupOffered(ProductGroups.InstantReseller)
              && !shopperProductData.ContainsKey(ProductIds.Reseller.ToString())
              && !shopperProductData.ContainsKey(ProductIds.Reseller_Renewal.ToString())
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.SUPER_RESELLER:
            if (_products.IsProductGroupOffered(ProductGroups.InstantReseller)
              && !shopperProductData.ContainsKey(ProductIds.ResellerPro.ToString())
              && !shopperProductData.ContainsKey(ProductIds.ResellerPro_Renewal.ToString())
              )
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.PRIVATE_DOMAINS:
            if (_products.IsProductGroupOffered(ProductGroups.PrivateRegistrations) && !shopperProductData.ContainsKey("domains"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.DELUXE_WHOIS:
            if (_products.IsProductGroupOffered(ProductGroups.BusinessRegistration) && !shopperProductData.ContainsKey("domains"))
            {
              isValid = true;
            }
            break;
          case CrossSellConfigProductId.APPRAISAL:
            if (_products.IsProductGroupOffered(ProductGroups.DomainNameAppraisal) && !shopperProductData.ContainsKey("domains"))
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

    #region Shopper data
    private bool IsMirageCurrent(string shopperId)
    {
      bool result = true;

      try
      {
        Dictionary<string, object> parms = new Dictionary<string, object>();
        parms.Add("shopper_id", shopperId);
        DataProviderRequestData request = new DataProviderRequestData(
          shopperId, string.Empty, string.Empty, string.Empty, 0,
          "ismiragecurrent", parms);
        request.RequestTimeout = new TimeSpan(0,0,2);
        DataProviderResponseData response = (DataProviderResponseData)Engine.Engine.ProcessRequest(request, PurchaseEmailEngineRequests.DataProvider);
        DataSet ds = response.GetResponseObject() as DataSet;

        if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
        {
          if (shopperId == (ds.Tables[0].Rows[0][0]).ToString())
          {
            result = false;
          }
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException("CrossSellConfig.IsMirageCurrent", string.Empty, "0",
          message, "ShopperId=" + _orderData.ShopperId, _orderData.ShopperId, _orderData.OrderId,
          string.Empty, string.Empty, 0);
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;
    }

    private DataSet GetDbDataSet(string shopperId, int privateLabelId)
    {
      string queryName = "currentproductgroups";
      string useROSetting = DataCache.DataCache.GetAppSetting("PC_PRODUCTS_USE_RO").ToLowerInvariant();
      if (useROSetting == "true")
      {
        queryName = "currentproductgroups_ro";
      }

      Dictionary<string, object> parms = new Dictionary<string, object>();
      parms.Add("s_shopper_id", shopperId);
      parms.Add("s_privateLabelID", privateLabelId);
      DataProviderRequestData request = new DataProviderRequestData(shopperId,
        string.Empty, string.Empty, string.Empty, 0, queryName, parms);
      request.RequestTimeout = new TimeSpan(0,0,8);
      DataProviderResponseData response = (DataProviderResponseData)Engine.Engine.ProcessRequest(request, PurchaseEmailEngineRequests.DataProvider);
      return response.GetResponseObject() as DataSet;
    }

    private void AddRecentlyPurchasedProducts(string privateLabelId, string shopperId, Dictionary<string, int> products)
    {
      if (!string.IsNullOrEmpty(privateLabelId) && !string.IsNullOrEmpty(shopperId))
      {
        if (!IsMirageCurrent(shopperId))
        {
          DataSet dbSet = GetDbDataSet(shopperId, int.Parse(privateLabelId));
          if (null != dbSet && dbSet.Tables.Count > 0)
          {
            if (dbSet.Tables[0].Rows.Count > 0)
            {
              foreach (DataRow dr in dbSet.Tables[0].Rows)
              {
                string key = "pg|" + dr.ItemArray[0];
                if (!products.ContainsKey(key))
                {
                  products[key] = 1;
                }
              }
            }
          }
        }
      }
    }

    public Dictionary<string, int> GetShopperProductDictionary(string shopperId, string privateLabelId, bool includeRecent)
    {
      Dictionary<string, int> result = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
      if (!string.IsNullOrEmpty(privateLabelId) && !string.IsNullOrEmpty(shopperId))
      {
        try
        {
          result = new Dictionary<string, int>(DataCache.DataCache.GetShopperProduct(shopperId));
          if (includeRecent)
          {
            AddRecentlyPurchasedProducts(privateLabelId, shopperId, result);
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + System.Environment.NewLine + ex.StackTrace;
          AtlantisException aex = new AtlantisException("CrossSellConfig.GetShopperProductDictionary", string.Empty, "0",
            message, "ShopperId=" + _orderData.ShopperId, _orderData.ShopperId, _orderData.OrderId,
            string.Empty, string.Empty, 0);
          Engine.Engine.LogAtlantisException(aex);
        }
      }

      return result;
    }

    #endregion
  }
}
