using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ResellerCalculator.Interface;

namespace Atlantis.Framework.ResellerCalculator.Impl
{
  public class ResellerCalculatorRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DataTable resultTable = new DataTable();
      ResellerCalculatorResponseData responseData = null;

      try
      {
        ResellerCalculatorRequestData requestData = (ResellerCalculatorRequestData)oRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        string procName = "dbo.rex_resellerCommissionCalculator_sp";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(procName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@xmldoc", requestData.XmlDoc));
            command.CommandTimeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalSeconds);
            connection.Open();

            using (SqlDataReader dr = command.ExecuteReader())
            {
              if ((dr.FieldCount > 0) && (dr.HasRows))
              {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                  string sColumnName = dr.GetName(i);
                  resultTable.Columns.Add(dr.GetName(i));
                }

                while (dr.Read())
                {
                  object[] oDataArray = new object[dr.FieldCount];
                  for (int i = 0; i < dr.FieldCount; i++)
                  {
                    oDataArray[i] = dr.GetValue(i);
                  }
                  resultTable.Rows.Add(oDataArray);
                }
              }
            }
          }
          connection.Close();
        }

        responseData = new ResellerCalculatorResponseData(resultTable);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new ResellerCalculatorResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new ResellerCalculatorResponseData(resultTable, oRequestData, ex);
      }
      return responseData;
    }
  }
}
