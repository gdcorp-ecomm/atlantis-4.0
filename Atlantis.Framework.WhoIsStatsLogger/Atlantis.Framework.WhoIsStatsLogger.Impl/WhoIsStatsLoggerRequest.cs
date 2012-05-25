using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.WhoIsStatsLogger.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.WhoIsStatsLogger.Impl
{
  public class WhoIsStatsLoggerRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      WhoIsStatsLoggerResponseData responseData = null;

      try
      {
        var requestData = (WhoIsStatsLoggerRequestData)oRequestData;
        int newID = 0;
        var connectionString = NetConnect.LookupConnectInfo(oConfig);
        const string procName = "dbo.whois_StatsLogInsert_sp";

        using (var connection = new SqlConnection(connectionString))
        {
          using (var command = new SqlCommand(procName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;

            //AddInsertParameters(cm, criteria);
            command.Parameters.AddWithValue("@HostName", requestData.HostName);
            command.Parameters.AddWithValue("@ClientPlatform", requestData.ClientPlatform);
            command.Parameters.AddWithValue("@Browser", requestData.Browser);
            command.Parameters.AddWithValue("@DomainName", requestData.DomainName);
            command.Parameters.AddWithValue("@TopLevelDomain", requestData.TopLevelDomain);
            command.Parameters.AddWithValue("@PrivateLabelID", requestData.PrivateLabelId);
            command.Parameters.AddWithValue("@StartTime", requestData.StartTime);
            command.Parameters.AddWithValue("@EndTime", requestData.EndTime);
            command.Parameters.AddWithValue("@ElapsedTime", requestData.ElapsedTime);
            command.Parameters.AddWithValue("@DetailThreshold", requestData.DetailThreshold);
            command.Parameters.AddWithValue("@IsDetailAvailable", requestData.IsDetailAvailable);
            command.Parameters.AddWithValue("@IsDetailForced", requestData.IsDetailForced);
            command.Parameters.AddWithValue("@IsBusinessRegistration", requestData.IsBusinessRegistration);
            command.Parameters.AddWithValue("@IsInternalIP", requestData.IsInternalIp);
            command.Parameters.AddWithValue("@ResultCode", requestData.ResultCode);
            var logId = new SqlParameter("@NewStatsLogID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(logId);
            connection.Open();

            command.ExecuteNonQuery();

            newID = (int)logId.Value;
          }
          connection.Close();

        }

        responseData = new WhoIsStatsLoggerResponseData(newID) { IsSuccess = true };
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new WhoIsStatsLoggerResponseData(exAtlantis) { IsSuccess = false };
      }
      catch (Exception ex)
      {
        responseData = new WhoIsStatsLoggerResponseData(oRequestData, ex) { IsSuccess = false };
      }
      return responseData;
    }
  }
}
