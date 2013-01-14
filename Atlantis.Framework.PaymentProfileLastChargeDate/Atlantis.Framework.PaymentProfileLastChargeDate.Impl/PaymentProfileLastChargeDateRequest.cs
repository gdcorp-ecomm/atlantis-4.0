using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PaymentProfileLastChargeDate.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.PaymentProfileLastChargeDate.Impl
{
  public class PaymentProfileLastChargeDateRequest : IRequest
  {

    private const string PROCNAME = "mya_GetLastPaymentProfileDate_sp";


    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      PaymentProfileLastChargeDateRequestData request = (PaymentProfileLastChargeDateRequestData)oRequestData;

      DateTime chargedDate = new DateTime();
      bool hasDate = false;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.AddWithValue("@shopper_id", request.ShopperID);
            command.Parameters.AddWithValue("@profileid", request.ProfileId);

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
              if (reader.HasRows)
              {
                int DATE = reader.GetOrdinal("maxOfDateEntered");
                while (reader.Read())
                {
                  chargedDate = reader.GetDateTime(DATE);
                  hasDate = true;
                }
              }
            }
          }
        }

        oResponseData = new PaymentProfileLastChargeDateResponseData(hasDate, chargedDate);
      }
      catch (Exception ex)
      {
        oResponseData = new PaymentProfileLastChargeDateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
