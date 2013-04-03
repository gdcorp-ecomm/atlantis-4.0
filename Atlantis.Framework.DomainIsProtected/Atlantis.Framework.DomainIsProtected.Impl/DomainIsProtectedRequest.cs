using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.DomainIsProtected.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.DomainIsProtected.Impl
{
  public class DomainIsProtectedRequest : IRequest
  {

    private const string PROC_NAME = "mya_getDomainProtectionStatus_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      bool isDomainProtected = false;
      try
      {

        DomainIsProtectedRequestData request = oRequestData as DomainIsProtectedRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.NetConnectionString);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {

          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.AddWithValue("@DomainID", request.ResourceId);
            
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
              while (reader.Read())
              {
                isDomainProtected = Convert.ToBoolean(reader["isDomainProtected"]);
              }
            }
          }
        }
        oResponseData = new DomainIsProtectedResponseData(isDomainProtected);
      }
      
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainIsProtectedResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainIsProtectedResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
