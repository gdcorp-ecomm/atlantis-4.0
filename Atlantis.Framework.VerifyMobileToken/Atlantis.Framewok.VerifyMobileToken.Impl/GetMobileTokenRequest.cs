using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.VerifyMobileToken.Interface;

namespace Atlantis.Framework.VerifyMobileToken.Impl
{
  public class VerifyMobileTokenRequest : IRequest
  {
    private const string _PROCNAME = "mobile_deviceSessionValidate_sp";
    private const string _SHOPPERIDPARAM = "@s_shopper_id";
    private const string _SESSIONIDPARAM = "@u_mobile_deviceSessionID";
    private const string _DEVICEIDPARAM = "@s_deviceID";

    private bool ValidateSessionToken(VerifyMobileTokenRequestData request)
    {
      bool valid = false;

      if (!String.IsNullOrEmpty(request.SessionToken) && request.SessionToken.Length == 36)
      {
        string connectionString = LookupConnectionString(request);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter(_SESSIONIDPARAM, request.SessionToken));
            command.Parameters.Add(new SqlParameter(_DEVICEIDPARAM, request.DeviceGUID));
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                object sessionTokenObj = reader[0];
                int result = Convert.ToInt32(sessionTokenObj);
                if (result == 1)
                  valid = true;
                break;
              }
            }
          }
        }
      }

      return valid;
    }
 
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      VerifyMobileTokenRequestData VerifyMobileTokenRequest = oRequestData as VerifyMobileTokenRequestData;
      VerifyMobileTokenResponseData VerifyMobileTokenResponse = null;

      try
      {
        bool result = ValidateSessionToken(VerifyMobileTokenRequest);
        VerifyMobileTokenResponse = new VerifyMobileTokenResponseData(result, VerifyMobileTokenRequest.SessionToken);
      }
      catch (AtlantisException exAtlantis)
      {
        VerifyMobileTokenResponse = new VerifyMobileTokenResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        VerifyMobileTokenResponse = new VerifyMobileTokenResponseData(oRequestData, ex);
      }

      return VerifyMobileTokenResponse;
    }

    private string LookupConnectionString(VerifyMobileTokenRequestData request)
    {
      string result = NetConnect.LookupConnectInfo(request.DataSourceName, request.CertificateName, request.ApplicationName, "DomainBundleIdRequest.LookupConnectionString",
                                            ConnectLookupType.NetConnectionString);

      //when an error occurs a ';' is returned not a valid connection string or empty
      if (result.Length <= 1)
      {
        throw new AtlantisException(request, "LookupConnectionString",
                "Database connection string lookup failed", "No ConnectionFound For:" + request.DataSourceName + ":" + request.ApplicationName + ":" + request.CertificateName);
      }

      return result;
    }

  }
}

