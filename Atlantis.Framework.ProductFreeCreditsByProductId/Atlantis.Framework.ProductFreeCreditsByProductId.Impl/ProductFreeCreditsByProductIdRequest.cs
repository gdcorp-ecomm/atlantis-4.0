using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductFreeCreditsByProductId.Interface;

namespace Atlantis.Framework.ProductFreeCreditsByProductId.Impl
{
  public class ProductFreeCreditsByProductIdRequest : IRequest
  {
    #region Parameter Constants

    private const string STORED_PROCEDURE = "dbo.gdshop_free_product_packageGetByUnifiedProductID_sp";
    private const string UNIFIED_PRODUCT_ID_PARAM = "@unifiedProductID";
    private const string PRIVATE_LABEL_ID_PARAM = "@privateLabelID";

    #endregion

    private static ConfigElement _config;

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductFreeCreditsByProductIdResponseData responseData = null;

      try
      {
        _config = config;

        ProductFreeCreditsByProductIdRequestData request = (ProductFreeCreditsByProductIdRequestData)requestData;

        //Get proc to execute
        using (SqlConnection cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          cn.Open();

          using (SqlCommand cmd = new SqlCommand(STORED_PROCEDURE, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(SetSqlParameters(request));

            var productFreeCredits = new List<ProductFreeCredit>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
              while (reader.Read())
              {
                productFreeCredits.Add(GetProductFreeCredit(reader));
              }
            }

            responseData = new ProductFreeCreditsByProductIdResponseData(productFreeCredits);
          }
          cn.Close();
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new ProductFreeCreditsByProductIdResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new ProductFreeCreditsByProductIdResponseData(requestData, ex);
      }

      return responseData;
    }

    #region SQL Parameter Handling
    
    private SqlParameter[] SetSqlParameters(ProductFreeCreditsByProductIdRequestData request)
    {
      List<SqlParameter> paramColl = new List<SqlParameter>();

      paramColl.Add(new SqlParameter(UNIFIED_PRODUCT_ID_PARAM, request.UnifiedProductId));
      paramColl.Add(new SqlParameter(PRIVATE_LABEL_ID_PARAM, request.PrivateLabelId));

      SqlParameter[] paramArray = new SqlParameter[paramColl.Count];
      paramColl.CopyTo(paramArray, 0);

      return paramArray;
    }

    #endregion

    #region ProductFreeCredit

    private ProductFreeCredit GetProductFreeCredit(SqlDataReader reader)
    {
      var productFreeCredit = new ProductFreeCredit();

      productFreeCredit.UnifiedProductId = (int)reader.GetDecimal(0);
      productFreeCredit.BillingNamespace = reader.GetString(1);
      
      return productFreeCredit;
    }

    #endregion ProductFreeCredit

  }
}
