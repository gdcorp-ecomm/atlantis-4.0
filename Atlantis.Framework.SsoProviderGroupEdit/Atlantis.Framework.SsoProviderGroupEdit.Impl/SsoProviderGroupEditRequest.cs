using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SsoProviderGroupEdit.Interface;

namespace Atlantis.Framework.SsoProviderGroupEdit.Impl
{
  public class SsoProviderGroupEditRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "dbo.sso_serviceProviderGroupAddUpdate_sp";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoProviderGroupEditResponseData responseData;

      try
      {
        var request = (SsoProviderGroupEditRequestData)requestData;
        UpdateServiceProviderGroup(request, config);

        responseData = new SsoProviderGroupEditResponseData();
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new SsoProviderGroupEditResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new SsoProviderGroupEditResponseData(requestData, ex);
      }

      return responseData;
    }

    private static void UpdateServiceProviderGroup(SsoProviderGroupEditRequestData requestData, ConfigElement config)
    {
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.AddWithValue("@s_serviceProviderGroupName", requestData.ServiceProviderGroupName);
          cmd.Parameters.AddWithValue("@s_redirectLoginUrl", requestData.RedirectLoginUrl);
          cmd.Parameters.AddWithValue("@s_logoutUrl", requestData.LogoutUrl);
          cmd.Parameters.AddWithValue("@s_redirectLogoutUrl", requestData.RedirectLogoutUrl);
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
