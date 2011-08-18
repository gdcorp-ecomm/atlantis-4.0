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

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductFreeCreditsByProductIdResponseData responseData;

      try
      {
        var request = (ProductFreeCreditsByProductIdRequestData)requestData;

        using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var cmd = new SqlCommand(STORED_PROCEDURE, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(new[]
                                      {
                                        new SqlParameter(UNIFIED_PRODUCT_ID_PARAM, request.UnifiedProductId),
                                        new SqlParameter(PRIVATE_LABEL_ID_PARAM, request.PrivateLabelId)
                                      });

            var productFreeCredits = new List<ProductFreeCredit>();

            cn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
              while (reader.Read())
              {
                productFreeCredits.Add(new ProductFreeCredit
                                         {
                                           UnifiedProductId = (int) reader.GetDecimal(0),
                                           BillingNamespace = reader.GetString(1)
                                         });
              }
            }
            cn.Close();

            responseData = new ProductFreeCreditsByProductIdResponseData(productFreeCredits);
          }
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
  }
}
