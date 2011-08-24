using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BuyerProfileDelete.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BuyerProfileDelete.Impl
{
  public class BuyerProfileDeleteRequest : IRequest
  {
    private const string PROC_NAME = "mya_DeleteBuyerProfile_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      BuyerProfileDeleteRequestData request = (BuyerProfileDeleteRequestData)oRequestData;

      DataSet ds = null;

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

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }
        oResponseData = new BuyerProfileDeleteResponseData();
      }
      catch (Exception ex)
      {
        oResponseData = new BuyerProfileDeleteResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
