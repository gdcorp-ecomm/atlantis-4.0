using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.CarmaGetTrustedInvitation.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaGetTrustedInvitation.Impl
{
  public class CarmaGetTrustedInvitationRequest : IRequest
  {
    #region Database Constants
    const string PROC_NAME = "dbo.carma_trustedInvitationGet_sp";
    const string PARAM_GUID = "@guid_invite";
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      CarmaGetTrustedInvitationResponseData responseData = null;

      try
      {
        string primaryShopperId = string.Empty;
        CarmaGetTrustedInvitationRequestData request = (CarmaGetTrustedInvitationRequestData)requestData;

        using (SqlConnection cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (SqlCommand cmd = new SqlCommand(PROC_NAME, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(PARAM_GUID, request.AuthorizationGuid));
            cn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
              while (reader.Read())
              {
                primaryShopperId = Convert.ToString(reader["primary_shopper_id"]);
              }
            }
          }
        }
        responseData = new CarmaGetTrustedInvitationResponseData(primaryShopperId);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new CarmaGetTrustedInvitationResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CarmaGetTrustedInvitationResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
