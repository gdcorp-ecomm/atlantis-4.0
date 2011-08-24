using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BuyerProfileAddUpdate.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BuyerProfileAddUpdate.Impl
{
  
  public class BuyerProfileAddUpdateRequest : IRequest
  {
    private const string PROC_NAME = "mya_updateBuyerProfile_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      BuyerProfileAddUpdateRequestData request = (BuyerProfileAddUpdateRequestData)oRequestData;

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
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));
            command.Parameters.Add(new SqlParameter("@XMLDoc", request.ToXML()));

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }
        oResponseData = new BuyerProfileAddUpdateResponseData();
      }
      catch (Exception ex)
      {
        oResponseData = new BuyerProfileAddUpdateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
