using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.GoodAsGoldReport.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.GoodAsGoldReport.Impl
{
  public class GoodAsGoldReportRequest : IRequest
  {
    private const string PROC_NAME = "reports_prepaidServiceStatement_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      GoodAsGoldReportRequestData request = (GoodAsGoldReportRequestData)oRequestData;

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
            command.Parameters.Add(new SqlParameter("@d_startDate", request.StateDate));
            command.Parameters.Add(new SqlParameter("@d_endDate", request.EndDate));
            command.Parameters.Add(new SqlParameter("@s_shopper_id", request.ShopperID));

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);

          }
        }

        oResponseData = new GoodAsGoldReportResponseData(ds);
      }
      catch (Exception ex)
      {
        oResponseData = new GoodAsGoldReportResponseData(oRequestData, ex);
      }

      return oResponseData;
    }
    
    #endregion

  }
}
