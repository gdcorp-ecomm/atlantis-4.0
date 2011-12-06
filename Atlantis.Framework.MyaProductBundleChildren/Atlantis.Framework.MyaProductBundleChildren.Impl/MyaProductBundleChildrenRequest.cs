using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProductBundleChildren.Interface;

namespace Atlantis.Framework.MyaProductBundleChildren.Impl
{
  public class MyaProductBundleChildrenRequest : IRequest
  {
    #region Constants
    private const int EXPRESS_EMAIL_MARKETING = 88;
    private const int SEARCH_ENGINE_VISIBILITY = 39;
    private const string STORED_PROCEDURE = "dbo.mya_getBundleChildListByResourceID_sp";
    private const string BILLING_RESOURCE_ID_PARAM = "@n_resource_id";

    private const string SEV_STORED_PROCEDURE = "tba_userwebsiteStatusGetForMYA_sp";
    private const string SEV_SHOPPER_ID_PARAM = "@shopper_id";
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MyaProductBundleChildrenResponseData responseData;
      List<ChildProduct> childProductList;

      try
      {
        var request = (MyaProductBundleChildrenRequestData)requestData;

        childProductList = GetBundleChildren(config, request);
        ChildProduct searchEngineVisibilityProduct = childProductList.Find(cp => cp.ProductTypeId == SEARCH_ENGINE_VISIBILITY);
        ChildProduct eemProduct = childProductList.Find(cp => cp.ProductTypeId == EXPRESS_EMAIL_MARKETING);

        if (searchEngineVisibilityProduct != null)
        {
          UpdateSEVChildProduct(config, request, searchEngineVisibilityProduct);
        }
        if (eemProduct != null)
        {
          UpdateEEMChildProduct(config, request, eemProduct);
        }

        responseData = new MyaProductBundleChildrenResponseData(childProductList);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MyaProductBundleChildrenResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MyaProductBundleChildrenResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Populate Bundle Child Products
    private static void UpdateEEMChildProduct(ConfigElement config, MyaProductBundleChildrenRequestData request, ChildProduct eemProduct)
    {
      if (eemProduct.CustomerId.Equals(-1))
      {
        eemProduct.CommonName = "New Account";
      }
      else
      {
        var root = new XElement("Customers",
          new XElement("Customer",
            new XElement("customer_id", eemProduct.CustomerId.ToString())));

        string summaryXml;
        using (var campaignBlazerWs = new CampaignBlazer.CampaignBlazer())
        {
          campaignBlazerWs.Url = ((WsConfigElement) config).WSURL;
          campaignBlazerWs.Timeout = (int) request.RequestTimeout.TotalMilliseconds;
          summaryXml = campaignBlazerWs.GetCustomerSummary(root.ToString());
        }

        if (!string.IsNullOrEmpty(summaryXml))
        {
          XDocument summaryDoc = XDocument.Parse(summaryXml);
          XElement customers = summaryDoc.Element("Customers");
          XElement customer = customers.Element("Customer");
          string companyName = customer.Element("company_name").Value;
          eemProduct.CommonName = !string.IsNullOrEmpty(companyName) ? companyName : "New Account";
        }
      }
    }

    private static void UpdateSEVChildProduct(ConfigElement config, MyaProductBundleChildrenRequestData request, ChildProduct sevProduct)
    {
      sevProduct.UserWebsiteId = 0;
      string connectionString = Nimitz.NetConnect.LookupConnectInfo(config.GetConfigValue("TB_DataSourceName")
        , config.GetConfigValue("CertificateName")
        , config.GetConfigValue("ApplicationName")
        , "UpdateSEVChildProduct"
        , Nimitz.ConnectLookupType.NetConnectionString);

      using (var cn = new SqlConnection(connectionString))
      {
        using (var cmd = new SqlCommand(SEV_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter(SEV_SHOPPER_ID_PARAM, request.ShopperID));

          cn.Open();
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              if (reader["recurring_id"].ToString() == sevProduct.BillingResourceId.ToString())
              {
                sevProduct.UserWebsiteId = reader["userwebsite_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["userwebsite_id"]);
                break;
              }
            }
          }
        }
        cn.Close();
      }
    }

    private static List<ChildProduct> GetBundleChildren(ConfigElement config, MyaProductBundleChildrenRequestData request)
    {
      IList<ChildProduct> childProductList = new List<ChildProduct>();
 
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter(BILLING_RESOURCE_ID_PARAM, request.BillingResourceId));

          cn.Open();
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              ChildProduct childProduct = SetChildProductData(reader, request.BillingResourceId);
              if (childProduct != null)
              {
                childProductList.Add(childProduct);
              }
            }
          }
        }
        cn.Close();
      }

      return (List<ChildProduct>)childProductList;
    }

    private static ChildProduct SetChildProductData(IDataReader reader, int parentBillingResourceId)
    {
      ChildProduct childProduct = null;

      if (reader.FieldCount > 0)
      {
        IDictionary<string, object> productProperties = new Dictionary<string, object>();

        for (int i = 0; i < reader.FieldCount; i++)
        {
          if (!productProperties.ContainsKey(reader.GetName(i)))
          {
            productProperties.Add(reader.GetName(i), reader.GetValue(i));
          }
        }

        childProduct = new ChildProduct(productProperties, parentBillingResourceId);
      }

      return childProduct;
    }
    #endregion
  }
}
