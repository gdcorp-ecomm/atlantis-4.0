using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BuyerProfileSetDefault.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BuyerProfileSetDefault.Impl
{
  public class BuyerProfileSetDefaultRequest : IRequest
  {
    private const string PROC_NAME = "mya_MakeBuyerProfileDefault_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      BuyerProfileSetDefaultRequestData request = (BuyerProfileSetDefaultRequestData)oRequestData;

      int result = -1;
      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@profile_id", request.ProfileID));
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@defaultProfileFlag", "1"));

            SqlParameter newparam = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
            newparam.Direction = ParameterDirection.ReturnValue;

            connection.Open();
            command.ExecuteNonQuery();
            result = (int)command.Parameters["@ReturnValue"].Value;
          }
        }
        oResponseData = new BuyerProfileSetDefaultResponseData(result);
      }
      catch (Exception ex)
      {
        oResponseData = new BuyerProfileSetDefaultResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
