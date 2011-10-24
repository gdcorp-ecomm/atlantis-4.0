using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.GetMobileToken.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.GetMobileToken.Impl
{
  public class GetMobileTokenRequest : IRequest
  {

    private const int _MINUTES_TILL_SESSION_EXPIRE = 20;
    private const string _PROCNAME = "mobile_deviceSessionInsert_sp";
    private const string _SHOPPERIDPARAM = "@s_shopper_id";
    private const string _DEVICEIDPARAM = "@s_deviceID";
    private const string _EXPIRATIONDATEPARAM = "@d_expirationDate";

    private string GetNewSessionToken(GetMobileTokenRequestData request, ConfigElement config)
    {
      string sessionToken = "";

      TimeSpan dtTimeSpan = new TimeSpan(0, _MINUTES_TILL_SESSION_EXPIRE, 0);
      DateTime dtExpire = DateTime.Now + dtTimeSpan;

      string connectionString = LookupConnectionString(request, config);
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
          command.Parameters.Add(new SqlParameter(_DEVICEIDPARAM, request.DeviceGUID));
          command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
          command.Parameters.Add(new SqlParameter(_EXPIRATIONDATEPARAM, dtExpire));
          connection.Open();
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              object sessionTokenObj = reader[0];
              sessionToken = Convert.ToString(sessionTokenObj);
              break;
            }
          }
        }
      }
      return sessionToken;
    }

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetMobileTokenRequestData getMobileTokenRequest = oRequestData as GetMobileTokenRequestData;
      GetMobileTokenResponseData getMobileTokenResponse = null;

      try
      {
        String sessionToken = GetNewSessionToken(getMobileTokenRequest, oConfig);
        getMobileTokenResponse = new GetMobileTokenResponseData(sessionToken);
      }
      catch (AtlantisException exAtlantis)
      {
        getMobileTokenResponse = new GetMobileTokenResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        getMobileTokenResponse = new GetMobileTokenResponseData(oRequestData, ex);
      }

      return getMobileTokenResponse;
    }

    private static string LookupConnectionString(GetMobileTokenRequestData request, ConfigElement config)
    {
      string result = NetConnect.LookupConnectInfo(config.GetConfigValue("DataSourceName"), config.GetConfigValue("CertificateName"), config.GetConfigValue("ApplicationName"), "LookupConnectionString.LookupConnectionString",
                                           ConnectLookupType.NetConnectionString);
      //when an error occurs a ';' is returned not a valid connection string or empty
      if (result.Length <= 1)
      {
        throw new AtlantisException(request, "LookupConnectionString",
                "Database connection string lookup failed", "No ConnectionFound For:" + config.GetConfigValue("DataSourceName") + ":" + config.GetConfigValue("ApplicationName") + ":" + config.GetConfigValue("CertificateName"));
      }

      return result;
    }

  }
}

