using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.DBSGetResourceById.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.DBSGetResourceById.Impl
{
  public class DBSGetResourceByIdRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DataTable resultTable = new DataTable();
      DBSGetResourceByIdResponseData responseData = null;

      try
      {
        DBSGetResourceByIdRequestData requestData = (DBSGetResourceByIdRequestData)oRequestData;
        
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        string procName = "dbo.dbs_Resource_getByResourceID_sp";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(procName, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@n_resourceID", requestData.ResourceId));
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

        responseData = new DBSGetResourceByIdResponseData(resultTable);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DBSGetResourceByIdResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DBSGetResourceByIdResponseData(resultTable, oRequestData, ex);
      }
      return responseData;
    }
  }
}
