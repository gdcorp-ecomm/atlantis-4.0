using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EcommInstoreStatement.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz; 

namespace Atlantis.Framework.EcommInstoreStatement.Impl
{
  public class EcommInstoreStatementRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommInstoreStatementResponseData response = null;
      try
      {
        var connectionString = NetConnect.LookupConnectInfo(config);

        var request = (EcommInstoreStatementRequestData)requestData;

        using (var connection = new SqlConnection(connectionString))
        {
          using (var command = new SqlCommand("mya_instoreCreditStatement_sp", connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
            command.Parameters.AddWithValue("@d_startDate", request.StartDate);
            command.Parameters.AddWithValue("@d_endDate", request.EndDate);
            command.Parameters.AddWithValue("@s_shopper_id", requestData.ShopperID);
            command.Parameters.AddWithValue("@s_nativeCurrencyType", request.Currency);
            
            var da = new SqlDataAdapter(command);
            var ds = new DataSet();
            da.Fill(ds);

            response = new EcommInstoreStatementResponseData(ds);
          }
        }
      }
      catch (AtlantisException aex)
      {
        response = new EcommInstoreStatementResponseData(aex);
      }
      catch (Exception ex)
      {
        response = new EcommInstoreStatementResponseData(requestData, ex);
      }

      return response;
    }

  }
}
