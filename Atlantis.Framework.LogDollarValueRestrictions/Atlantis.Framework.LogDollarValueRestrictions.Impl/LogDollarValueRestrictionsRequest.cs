using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LogDollarValueRestrictions.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.LogDollarValueRestrictions.Impl
{
  public class LogDollarValueRestrictionsRequest : IRequest
  {
    private const string PROC_NAME = "gdshop_dollarValueRestrictionInsert_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData = null;
      LogDollarValueRestrictionsRequestData request = (LogDollarValueRestrictionsRequestData)oRequestData;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;

            command.Parameters.Add(new SqlParameter("@s_shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@s_order_id", request.OrderID));
            command.Parameters.Add(new SqlParameter("@n_total", request.TotalPrice));
            command.Parameters.Add(new SqlParameter("@n_mgrFlag", request.IsManager));

            connection.Open();
            command.ExecuteNonQuery();
          }
        }

        responseData = new LogDollarValueRestrictionsResponseData(string.Empty);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new LogDollarValueRestrictionsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new LogDollarValueRestrictionsResponseData(oRequestData, ex);
      }

      return responseData;
    }
    #endregion
  }
}
