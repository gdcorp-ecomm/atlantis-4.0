using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaResourceReverseQty.Interface;

namespace Atlantis.Framework.MyaResourceReverseQty.Impl
{
  public class MyaResourceReverseQtyRequest : IRequest
  {
    private const string STORED_PROCEDURE = "dbo.mya_getResourceReverseQuantity_sp";
    private const string BILLING_RESOURCE_ID_PARAM = "@n_resource_id";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MyaResourceReverseQtyResponseData responseData;

      try
      {
        var request = (MyaResourceReverseQtyRequestData)requestData;

        using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          cn.Open();

          using (var cmd = new SqlCommand(STORED_PROCEDURE, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(BILLING_RESOURCE_ID_PARAM, request.BillingResourceId));

            var results = new List<ResourceReverseQty>();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
              while (reader.Read())
              {
                results.Add(
                  new ResourceReverseQty(
                    reader["order_id"] == DBNull.Value ? string.Empty : Convert.ToString(reader["order_id"]),
                    reader["order_id"] == DBNull.Value ? -1 : Convert.ToInt32(reader["row_id"]),
                    reader["order_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["canBeReversed"])));
              }
            }

            responseData = new MyaResourceReverseQtyResponseData(results);
          }
          cn.Close();
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new MyaResourceReverseQtyResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        var atlEx = new AtlantisException(requestData, "MyaResourceReverseQtyRequest.RequestHandler", ex.Message, string.Empty, ex);
        responseData = new MyaResourceReverseQtyResponseData(atlEx);
      }

      return responseData;
    }
  }
}
