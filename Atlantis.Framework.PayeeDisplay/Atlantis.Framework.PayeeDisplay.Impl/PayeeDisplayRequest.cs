using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PayeeDisplay.Interface;

namespace Atlantis.Framework.PayeeDisplay.Impl
{
  public class PayeeDisplayRequest :IRequest
  {
    private const string PROCNAME = "mya_getVendorDisplayIndicator_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      PayeeDisplayRequestData request = (PayeeDisplayRequestData)oRequestData;
      bool payeeDisplay = false;
      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@s_shopper_id", request.ShopperID));

            SqlParameter newparam = command.Parameters.Add("@n_displayFlag", SqlDbType.Int);
            newparam.Direction = ParameterDirection.Output;

            connection.Open();
            command.ExecuteNonQuery();
            payeeDisplay = (int)command.Parameters["@n_displayFlag"].Value == 1;
          }
        }

        oResponseData = new PayeeDisplayResponseData(payeeDisplay);
      }
      catch (Exception ex)
      {
        oResponseData = new PayeeDisplayResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
