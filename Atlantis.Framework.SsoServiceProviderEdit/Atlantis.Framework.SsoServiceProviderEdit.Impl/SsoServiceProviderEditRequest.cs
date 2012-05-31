using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SsoServiceProviderEdit.Interface;

namespace Atlantis.Framework.SsoServiceProviderEdit.Impl
{
  public class SsoServiceProviderEditRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "dbo.sso_serviceProviderAddUpdate_sp";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoServiceProviderEditResponseData responseData;

      try
      {
        var request = (SsoServiceProviderEditRequestData)requestData;
        UpdateSsoServiceProvider(request, config);

        responseData = new SsoServiceProviderEditResponseData();
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new SsoServiceProviderEditResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new SsoServiceProviderEditResponseData(requestData, ex);
      }

      return responseData;
    }

    private static void UpdateSsoServiceProvider(SsoServiceProviderEditRequestData requestData, ConfigElement config)
    {
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.AddWithValue("@s_serviceProviderName", requestData.ServiceProviderName);
          cmd.Parameters.AddWithValue("@s_identityProviderName", requestData.IdentityProviderName);
          cmd.Parameters.AddWithValue("@s_serviceProviderGroupName", requestData.ServiceProviderGroupName);
          cmd.Parameters.AddWithValue("@s_loginReceive", requestData.LoginReceive);
          cmd.Parameters.AddWithValue("@s_loginReceiveType", requestData.LoginReceiveType);
          cmd.Parameters.AddWithValue("@s_serverName", requestData.ServerName);
          cmd.Parameters.AddWithValue("@n_isRetired", requestData.IsRetired ? 1 : 0);
          cmd.Parameters.AddWithValue("@s_retiredDate", requestData.RetiredDate.ToString());
          cmd.Parameters.AddWithValue("@s_changedBy", requestData.ChangedBy);
          cmd.Parameters.AddWithValue("@s_approvedBy", requestData.ApprovedBy);
          cmd.Parameters.AddWithValue("@s_actionDescription", requestData.ActionDescription);

          cn.Open();
          cmd.ExecuteNonQuery();
          cn.Close();
        }
      }
    }

  }
}