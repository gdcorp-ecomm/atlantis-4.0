using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PaymentAlternateAddUpdate.Interface;


namespace Atlantis.Framework.PaymentAlternateAddUpdate.Impl
{
  public class PaymentAlternateAddUpdateRequest : IRequest
  {
    private const string PROC_NAME = "gdshop_AddUpdateBackupPPShopperProfileID";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      PaymentAlternateAddUpdateRequestData request = (PaymentAlternateAddUpdateRequestData)oRequestData;
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
            command.Parameters.Add(new SqlParameter("@pp_backupShopperProfileID", request.PaymentProfileId));

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
          }
        }

        oResponseData = new PaymentAlternateAddUpdateResponseData();
      }
      catch (Exception ex)
      {
        oResponseData = new PaymentAlternateAddUpdateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
