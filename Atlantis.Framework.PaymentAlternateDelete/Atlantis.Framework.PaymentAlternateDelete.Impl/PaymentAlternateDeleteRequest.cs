using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PaymentAlternateDelete.Interface;

namespace Atlantis.Framework.PaymentAlternateDelete.Impl
{
  public class PaymentAlternateDeleteRequest : IRequest
  {
    private const string PROC_NAME = "gdshop_delBackupPPShopperProfileID";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      PaymentAlternateDeleteRequestData request = (PaymentAlternateDeleteRequestData)oRequestData;
      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
          }
        }

        oResponseData = new PaymentAlternateDeleteResponseData();
      }
      catch (Exception ex)
      {
        oResponseData = new PaymentAlternateDeleteResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
