using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductFreeCreditsByResource.Interface;

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

        ProductFreeCreditsByResourceRequestData request = (ProductFreeCreditsByResourceRequestData)requestData;

        //Get proc to execute
        using (SqlConnection cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          cn.Open();

          ProcConfigItem configItem = ProductFreeCreditsByResourceRequest.ProcConfigList[request.ProductTypeId];
          if (configItem != null)
          {
            using (SqlCommand cmd = new SqlCommand(configItem.StoredProcedure, cn))
            {
              cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Parameters.AddRange(SetSqlParameters(request, configItem));

              var productFreeCredits = new List<ResourceFreeCredit>();
              using (SqlDataReader reader = cmd.ExecuteReader())
              {
                while (reader.Read())
                {
                  productFreeCredits.Add(GetResourceFreeCredit(reader));
                }
              }

              responseData = new ProductFreeCreditsByResourceResponseData(productFreeCredits);
            }
            cn.Close();
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

    #region SQL Parameter Handling
    
    private SqlParameter[] SetSqlParameters(ProductFreeCreditsByResourceRequestData request, ProcConfigItem configItem)
    {
      List<SqlParameter> paramColl = new List<SqlParameter>();

      paramColl.Add(new SqlParameter(BILLING_RESOURCE_ID_PARAM, request.BillingResourceId));

      SqlParameter[] paramArray = new SqlParameter[paramColl.Count];
      paramColl.CopyTo(paramArray, 0);

      return paramArray;
    }

    #endregion

    #region ProductFreeCredit

    private ResourceFreeCredit GetResourceFreeCredit(SqlDataReader reader)
    {
      var resourceFreeCredit = new ResourceFreeCredit();

      resourceFreeCredit.FreeProductPackageId = reader.GetInt32(0);
      resourceFreeCredit.UnifiedProductId = (int)reader.GetDecimal(1);
      resourceFreeCredit.Quantity = reader.GetInt32(3);

      return resourceFreeCredit;
    }

    #endregion ProductFreeCredit

    #region ProcConfigList DataCached

    internal static Dictionary<int, ProcConfigItem> GetProcConfigList(string key)
    {
      var procConfigList = new Dictionary<int, ProcConfigItem>();
      using (SqlConnection cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(_config)))
      {
        cn.Open();

        using (SqlCommand cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandType = CommandType.StoredProcedure;

          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              procConfigList.Add(reader.GetInt32(0), new ProcConfigItem(reader.GetInt32(0), reader.GetString(1)));
            }
          }
        }

        cn.Close();
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
