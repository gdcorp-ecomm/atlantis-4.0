using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BuyerProxyAccountsGet.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BuyerProxyAccountsGet.Impl
{
  public class BuyerProxyAccountsGetRequest : IRequest
  {
    private const string PROCNAME = "mya_dbp_logins_by_shopper_id_sp";


    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      BuyerProxyAccountsGetRequestData request = (BuyerProxyAccountsGetRequestData)oRequestData;
      DataSet ds = null;
      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));

            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);

          }
        }

        oResponseData = new BuyerProxyAccountsGetResponseData(ds);
      }
      catch (Exception ex)
      {
        oResponseData = new BuyerProxyAccountsGetResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
