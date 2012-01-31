using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EcommInstoreBalance.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.EcommInstoreBalance.Impl
{
  public class EcommInstoreBalanceRequest: IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommInstoreBalanceResponseData response = null;
      try
      {
        string connectionString = NetConnect.LookupConnectInfo(config);
        
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand("instore_creditGetBalanceWithCurrencyByShopper_sp", connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
            command.Parameters.AddWithValue("@s_shopper_id", requestData.ShopperID);


            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);

            response = new EcommInstoreBalanceResponseData(dt);
          }
        }
      }
      catch (AtlantisException aex)
      {
        response = new EcommInstoreBalanceResponseData(aex);
      }
      catch (Exception ex)
      {
        response = new EcommInstoreBalanceResponseData(requestData, ex);
      }

      return response;
    }
  }
}
