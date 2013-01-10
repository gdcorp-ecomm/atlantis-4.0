using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ShopperFirstOrder.Interface;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.ShopperFirstOrder.Impl
{
    public class ShopperFirstOrderRequest : IRequest
    {
      const string _PROCNAME = "gdshop_shopperGetFirstOrder_sp";
      const string _SHOPPERIDPARAM = "@ShopperID";
      const string _FIRSTORDERPARAMOUT = "@firstOrder";

      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result = null;

        try
        {
          if (string.IsNullOrEmpty(requestData.ShopperID))
          {
            result = new ShopperFirstOrderResponseData();
          }
          else
          {
            string connectionString = NetConnect.LookupConnectInfo(config);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
              using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
              {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, requestData.ShopperID));

                SqlParameter firstOrderOut = new SqlParameter(_FIRSTORDERPARAMOUT, SqlDbType.VarChar, 20);
                firstOrderOut.Direction = ParameterDirection.Output;
                command.Parameters.Add(firstOrderOut);

                command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
                connection.Open();

                command.ExecuteNonQuery();

                string firstOrderId = firstOrderOut.Value.ToString();
                result = ShopperFirstOrderResponseData.FromFirstOrderId(firstOrderId);
              }
            }
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message + Environment.NewLine + ex.StackTrace;
          AtlantisException aex = new AtlantisException(requestData, "ShopperFirstOrderRequest.RequestHandler", message, requestData.ShopperID);
          result = ShopperFirstOrderResponseData.FromAtlantisException(aex);
        }

        return result;
      }
    }
}
