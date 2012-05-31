using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SsoServiceProviderGet.Interface;

namespace Atlantis.Framework.SsoServiceProviderGet.Impl
{
  public class SsoServiceProviderGetRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "dbo.sso_serviceProviderGet_sp";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoServiceProviderGetResponseData responseData;

      try
      {
        var request = (SsoServiceProviderGetRequestData)requestData;
        var serviceProviders = GetSsoServiceProviders(request, config);

        responseData = new SsoServiceProviderGetResponseData(serviceProviders);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new SsoServiceProviderGetResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new SsoServiceProviderGetResponseData(requestData, ex);
      }

      return responseData;
    }

    private static List<SsoServiceProviderItem> GetSsoServiceProviders(SsoServiceProviderGetRequestData requestData, ConfigElement config)
    {
      var serviceProviders = new List<SsoServiceProviderItem>();
      
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;

          if (requestData.ServiceProviderName != null)
            cmd.Parameters.Add(new SqlParameter("@s_serviceProviderName", requestData.ServiceProviderName));

          cn.Open();

          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              serviceProviders.Add(GetSsoServiceProviderItem(reader));
            }
          }
        }

        cn.Close();
      }

      return serviceProviders;
    }

    private static SsoServiceProviderItem GetSsoServiceProviderItem(IDataReader reader)
    {
      var ssoServiceProviderItem = new SsoServiceProviderItem
      {
        ServiceProviderName = Convert.ToString(reader["serviceProviderName"]),
        IdentityProviderName = Convert.ToString(reader["identityProviderName"]),
        ServiceProviderGroupName = Convert.ToString(reader["serviceProviderGroupName"]),
        LoginReceive = reader["loginReceive"] != DBNull.Value ? Convert.ToString(reader["loginReceive"]) : null,
        LoginReceiveType = reader["loginReceiveType"] != DBNull.Value ? Convert.ToString(reader["loginReceiveType"]) : null,
        ServerName = reader["serverName"] != DBNull.Value ? Convert.ToString(reader["serverName"]) : null,
        IsRetired = reader["isRetired"] != DBNull.Value ? Convert.ToBoolean(reader["isRetired"]) : false,
        RetiredDate = GetDateTimeOrNull(reader, "retiredDate"),
        CreateDate = GetDateTimeOrNull(reader, "createDate"),
        ChangedBy = reader["changedBy"] != DBNull.Value ? Convert.ToString(reader["changedBy"]) : null,
        ApprovedBy = reader["approvedBy"] != DBNull.Value ? Convert.ToString(reader["approvedBy"]) : null,
        ActionDescription = reader["actionDescription"] != DBNull.Value ? Convert.ToString(reader["actionDescription"]) : null
      };

      return ssoServiceProviderItem;
    }

    private static Nullable<DateTime> GetDateTimeOrNull(IDataReader reader, string fieldName)
    {
      Nullable<DateTime> retVal = null;

      var rDate = reader[fieldName];

      if (rDate != DBNull.Value)
      {
        DateTime date;
        if (DateTime.TryParse(Convert.ToString(rDate), out date))
          retVal = date;
      }

      return retVal;
    }

  }
}
