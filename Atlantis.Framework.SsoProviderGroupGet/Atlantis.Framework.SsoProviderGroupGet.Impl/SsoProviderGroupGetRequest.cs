using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SsoProviderGroupGet.Interface;

namespace Atlantis.Framework.SsoProviderGroupGet.Impl
{
  public class SsoProviderGroupGetRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "dbo.sso_serviceProviderGroupGet_sp";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoProviderGroupGetResponseData responseData;

      try
      {
        var request = (SsoProviderGroupGetRequestData)requestData;
        var providerGroups = GetServiceProviderGroups(request, config);

        responseData = new SsoProviderGroupGetResponseData(providerGroups);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new SsoProviderGroupGetResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new SsoProviderGroupGetResponseData(requestData, ex);
      }

      return responseData;
    }

    private static List<ServiceProviderGroupItem> GetServiceProviderGroups(SsoProviderGroupGetRequestData requestData, ConfigElement config)
    {
      var providerGroups = new List<ServiceProviderGroupItem>();

      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;

          if (requestData.ServiceProviderGroupName != null)
            cmd.Parameters.Add(new SqlParameter("@s_serviceProviderGroupName", requestData.ServiceProviderGroupName));

          cn.Open();

          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              providerGroups.Add(GetServiceProviderGroupItem(reader));
            }
          }
        }

        cn.Close();
      }

      return providerGroups;
    }

    private static ServiceProviderGroupItem GetServiceProviderGroupItem(IDataReader reader)
    {
      var serviceProviderGroupItem = new ServiceProviderGroupItem
      {
        ServiceProviderGroupName = Convert.ToString(reader["serviceProviderGroupName"]),
        RedirectLoginUrl = GetStringOrNull(reader, "redirectLoginURL"),
        LogoutUrl = GetStringOrNull(reader, "logoutURL"),
        RedirectLogoutUrl = GetStringOrNull(reader, "redirectLogoutURL"),
        CreateDate = Convert.ToDateTime(reader["createDate"]),
        ChangedBy = GetStringOrNull(reader, "changedBy"),
        ApprovedBy = GetStringOrNull(reader, "approvedBy"),
        ActionDescription = GetStringOrNull(reader, "actionDescription")
      };

      return serviceProviderGroupItem;
    }

    private static string GetStringOrNull(IDataReader reader, string fieldName)
    {
      string retval = null;

      var value = reader[fieldName];
      if (value != DBNull.Value)
        retval = Convert.ToString(value);

      return retval;
    }

  }
}