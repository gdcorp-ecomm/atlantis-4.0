using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SsoIdentityProviderGet.Interface;

namespace Atlantis.Framework.SsoIdentityProviderGet.Impl
{
  public class SsoIdentityProviderGetRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "dbo.sso_identityProviderGet_sp";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SsoIdentityProviderGetResponseData responseData;

      try
      {
        var request = (SsoIdentityProviderGetRequestData)requestData;
        var identityProviders = GetSsoIdentityProviders(request, config);

        responseData = new SsoIdentityProviderGetResponseData(identityProviders);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new SsoIdentityProviderGetResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new SsoIdentityProviderGetResponseData(requestData, ex);
      }

      return responseData;
    }

    private static List<SsoIdentityProviderItem> GetSsoIdentityProviders(SsoIdentityProviderGetRequestData requestData, ConfigElement config)
    {
      var identityProviders = new List<SsoIdentityProviderItem>();

      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;

          if (requestData.IdentityProviderName != null)
            cmd.Parameters.Add(new SqlParameter("@s_identityProviderName", requestData.IdentityProviderName));

          cn.Open();

          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              identityProviders.Add(GetSsoIdentityProviderItem(reader));
            }
          }
        }

        cn.Close();
      }

      return identityProviders;
    }

    private static SsoIdentityProviderItem GetSsoIdentityProviderItem(IDataReader reader)
    {
      var ssoIdentityProviderItem = new SsoIdentityProviderItem
      {
        IdentityProviderName = Convert.ToString(reader["identityProviderName"]),
        LoginUrl = GetStringOrNull(reader, "loginURL"),
        LogoutUrl = GetStringOrNull(reader, "logoutURL"),
        PublicKey = GetStringOrNull(reader, "publicKey"),
        CertificateName = GetStringOrNull(reader, "certificateName"),
        CreateDate = Convert.ToDateTime(reader["createDate"]),
        ChangedBy = GetStringOrNull(reader, "changedBy"),
        ApprovedBy = GetStringOrNull(reader, "approvedBy"),
        ActionDescription = GetStringOrNull(reader, "actionDescription")
      };

      return ssoIdentityProviderItem;
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