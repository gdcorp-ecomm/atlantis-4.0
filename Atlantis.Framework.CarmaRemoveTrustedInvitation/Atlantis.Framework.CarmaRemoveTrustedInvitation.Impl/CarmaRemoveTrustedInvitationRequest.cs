using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.CarmaRemoveTrustedInvitation.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaRemoveTrustedInvitation.Impl
{
  public class CarmaRemoveTrustedInvitationRequest : IRequest
  {
    #region Database Constants
    const string PROC_NAME = "dbo.carma_trustedInvitationRemove_sp";
    const string PARAM_GUID = "@guid_invite";
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CarmaRemoveTrustedInvitationResponseData responseData = null;

      try
      {
        CarmaRemoveTrustedInvitationRequestData request = (CarmaRemoveTrustedInvitationRequestData)requestData;

        using (SqlConnection cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (SqlCommand cmd = new SqlCommand(PROC_NAME, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(PARAM_GUID, request.AuthorizationGuid));
            cn.Open();
            cmd.ExecuteNonQuery();
          }
        }
        responseData = new CarmaRemoveTrustedInvitationResponseData();

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new CarmaRemoveTrustedInvitationResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CarmaRemoveTrustedInvitationResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
