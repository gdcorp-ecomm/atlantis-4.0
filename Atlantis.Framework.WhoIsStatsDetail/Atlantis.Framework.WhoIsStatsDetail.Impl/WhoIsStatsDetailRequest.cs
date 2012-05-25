using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.WhoIsStatsDetail.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.WhoIsStatsDetail.Impl
{
  public class WhoIsStatsDetailRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      var resultTable = new DataTable();
      WhoIsStatsDetailResponseData responseData = null;

      try
      {
        var requestData = (WhoIsStatsDetailRequestData)oRequestData;

        var connectionString = NetConnect.LookupConnectInfo(oConfig);
        const string procName = "dbo.whois_StatsLogDetailInsert_sp";

        using (var connection = new SqlConnection(connectionString))
        {
          using (var command = new SqlCommand(procName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;

            //AddInsertParameters(cm, criteria);
            command.Parameters.AddWithValue("@StatsLogID", requestData.StatsLogId);
            command.Parameters.AddWithValue("@LogDetail", requestData.LogDetail);
            command.Parameters.AddWithValue("@StartTime", requestData.StartTime);
            command.Parameters.AddWithValue("@EndTime", requestData.EndTime);
            command.Parameters.AddWithValue("@ElapsedTime", requestData.ElapsedTime);
            var logId = new SqlParameter("NewStatsLogDetailID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(logId);
            connection.Open();

            command.ExecuteNonQuery();
          }
          connection.Close();
        }

        responseData = new WhoIsStatsDetailResponseData(resultTable) { IsSuccess = true };
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new WhoIsStatsDetailResponseData(exAtlantis) { IsSuccess = false };
      }
      catch (Exception ex)
      {
        responseData = new WhoIsStatsDetailResponseData(resultTable, oRequestData, ex) { IsSuccess = false };
      }
      return responseData;
    }
  }
}
