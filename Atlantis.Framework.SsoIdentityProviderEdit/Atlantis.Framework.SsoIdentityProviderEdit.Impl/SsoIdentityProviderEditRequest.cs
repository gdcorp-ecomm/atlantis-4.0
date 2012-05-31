using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SsoIdentityProviderEdit.Interface;

namespace Atlantis.Framework.SsoIdentityProviderEdit.Impl
{
  public class SsoIdentityProviderEditRequest :IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "dbo.sso_identityProviderAddUpdate_sp";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoIdentityProviderEditResponseData responseData;

      try
      {
        var request = (SsoIdentityProviderEditRequestData)requestData;
        UpdateSsoIdentityProvider(request, config);

        responseData = new SsoIdentityProviderEditResponseData();
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new SsoIdentityProviderEditResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new SsoIdentityProviderEditResponseData(requestData, ex);
      }

      return responseData;
    }

    private static void UpdateSsoIdentityProvider(SsoIdentityProviderEditRequestData requestData, ConfigElement config)
    {
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.AddWithValue("@s_identityProviderName", requestData.IdentityProviderName);
          cmd.Parameters.AddWithValue("@s_loginUrl", requestData.LoginUrl);
          cmd.Parameters.AddWithValue("@s_logoutUrl", requestData.LogoutUrl);
          cmd.Parameters.AddWithValue("@s_publicKey", requestData.PublicKey);
          cmd.Parameters.AddWithValue("@s_certificateName", requestData.CertificateName);
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