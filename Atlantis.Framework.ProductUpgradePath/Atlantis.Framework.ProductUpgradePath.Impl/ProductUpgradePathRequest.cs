using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ProductUpgradePath.Interface;

namespace Atlantis.Framework.ProductUpgradePath.Impl
{
  public class ProductUpgradePathRequest : IRequest
  {
    #region Parameters

    private const string STORED_PROCEDURE = "gdshop_getUnifiedBillingSyncByPrivateLabelAndPFID_sp";
    private const string PARAM_PRODUCT_ID = "@n_pf_id";
    private const string PARAM_PRIVATELABEL_ID = "@n_privatelabelID";

    private const string PRODUCT_ID = "catalog_productUnifiedID";
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductUpgradePathResponseData responseData = null;
      try
      {
        ProductUpgradePathRequestData currentRequest = (ProductUpgradePathRequestData)requestData;
        string connectionString = LookupConnectionString(config);
        if (connectionString.Length > 1)
        {
          Dictionary<int, UpgradeProductInfo> productList = new Dictionary<int, UpgradeProductInfo>();
          using (SqlConnection connection = new SqlConnection(connectionString))
          {
            using (SqlCommand command = new SqlCommand(STORED_PROCEDURE, connection))
            {
              command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
              command.CommandType = CommandType.StoredProcedure;
              command.Parameters.Add(new SqlParameter(PARAM_PRODUCT_ID, currentRequest.ProductID));
              command.Parameters.Add(new SqlParameter(PARAM_PRIVATELABEL_ID, currentRequest.PrivateLabelID));
              connection.Open();
              SqlDataReader reader = command.ExecuteReader();
              using (reader)
              {
                while (reader.Read())
                {
                  int productID = FieldReader.ReadField<int>(reader, PRODUCT_ID, -1);
                  if (productID != -1)
                  {
                    if (!productList.ContainsKey(productID))
                    {
                      UpgradeProductInfo newProduct = new UpgradeProductInfo(reader);
                      productList[productID] = newProduct;
                    }
                  }
                }
              }
            }
          }
          responseData = new ProductUpgradePathResponseData(productList,currentRequest.ProductOptions);
        }
        else
        {
          string dataSource = config.GetConfigValue("DataSourceName");
          string applicationName = config.GetConfigValue("ApplicationName");
          string certificateName = config.GetConfigValue("CertificateName");
          string errorMessage = string.Concat(dataSource, ":", applicationName, ":", certificateName);
          throw new AtlantisException(requestData, "Load Product Upgrade Path", "Could not find Connection string:", errorMessage);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new ProductUpgradePathResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new ProductUpgradePathResponseData(requestData, ex);
      }

      return responseData;
    }

    private string LookupConnectionString(ConfigElement config)
    {
      string result = string.Empty;
      string dataSource = config.GetConfigValue("DataSourceName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string certificateName = config.GetConfigValue("CertificateName");
      if (!String.IsNullOrEmpty(dataSource) && !String.IsNullOrEmpty(applicationName) && !String.IsNullOrEmpty(certificateName))
      {
        result = NetConnect.LookupConnectInfo(config);
      }
      return result;
    }
   
  }
}
