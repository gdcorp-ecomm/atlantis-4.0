using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductFreeCreditsByProductId.Interface.Types;
using Atlantis.Framework.ProductFreeCreditsByResource.Interface;
using Atlantis.Framework.ProductFreeCreditsByResource.Interface.Types;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Impl
{
  public class ProductFreeCreditsByResourceRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "gdshop_product_typeGetFreeProductProc_sp";
    private const string BILLING_RESOURCE_ID_PARAM = "@resource_id";

    #endregion

    private static ConfigElement _config;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductFreeCreditsByResourceResponseData responseData = null;

      try
      {
        _config = config;

        var request = (ProductFreeCreditsByResourceRequestData)requestData;

        //Get proc to execute
        using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          ProcConfigItem configItem = ProcConfigList[request.ProductTypeId];
          if (configItem != null)
          {
            using (var cmd = new SqlCommand(configItem.StoredProcedure, cn))
            {
              cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Parameters.Add(new SqlParameter(BILLING_RESOURCE_ID_PARAM, request.BillingResourceId));

              var productFreeCredits = new Dictionary<int, List<IProductFreeCredit>>();

              cn.Open();
              using (SqlDataReader reader = cmd.ExecuteReader())
              {
                while (reader.Read())
                {
                  int freeProductGroupId = Convert.ToInt32(reader["redemptionGroup"]);
                  
                  if (!productFreeCredits.ContainsKey(freeProductGroupId))
                    productFreeCredits.Add(freeProductGroupId, new List<IProductFreeCredit>());

                  productFreeCredits[freeProductGroupId].Add(new ResourceFreeCredit
                                                               {
                                                                 UnifiedProductId = Convert.ToInt32(reader["catalog_productUnifiedProductID"]),
                                                                 Quantity = Convert.ToInt32(reader["quantity"]),
                                                                 ProductNamespace = reader["nameSpace"].ToString()
                                                               });
                }
              }
              cn.Close();

              responseData = new ProductFreeCreditsByResourceResponseData(productFreeCredits);
            }
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new ProductFreeCreditsByResourceResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new ProductFreeCreditsByResourceResponseData(requestData, ex);
      }

      return responseData;
    }

    #region ProcConfigList DataCached

    internal static Dictionary<int, ProcConfigItem> GetProcConfigList(string key)
    {
      var procConfigList = new Dictionary<int, ProcConfigItem>();
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(_config)))
      {
        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandType = CommandType.StoredProcedure;

          cn.Open();
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              procConfigList.Add(reader.GetInt32(0), new ProcConfigItem(reader.GetInt32(0), reader.GetString(1)));
            }
          }
          cn.Close();
        }
      }

      return procConfigList;
    }

    private static Dictionary<int, ProcConfigItem> ProcConfigList
    {
      get
      {
        return DataCache.DataCache.GetCustomCacheData("{freeProductConfigItems}", GetProcConfigList);
      }
    }

    #endregion ProcConfigList DataCached
  }
}
