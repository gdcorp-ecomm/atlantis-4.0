using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ResellerTopAcctByDate.Interface;

namespace Atlantis.Framework.ResellerTopAcctByDate.Impl
{
  public class ResellerTopAcctByDateRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DataTable resultTable = null;
      ResellerTopAcctByDateResponseData responseData = null;

      try
      {
        ResellerTopAcctByDateRequestData requestData = (ResellerTopAcctByDateRequestData)oRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        string procName = "dbo.rex_ResellerTopAcctByDate_sp";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(procName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@StartDate", requestData.StartDate));
            command.Parameters.Add(new SqlParameter("@EndDate", requestData.EndDate));
            command.Parameters.Add(new SqlParameter("@NumRows", requestData.NumRows));
            command.CommandTimeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalSeconds);

            DataSet dataSet = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
              adapter.Fill(dataSet);
            }

            if (dataSet.Tables.Count > 1)
            {
              resultTable = dataSet.Tables[1];
            }
          }
        }
        responseData = new ResellerTopAcctByDateResponseData(resultTable);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new ResellerTopAcctByDateResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new ResellerTopAcctByDateResponseData(resultTable, oRequestData, ex);
      }
      return responseData;
    }
  }
}
