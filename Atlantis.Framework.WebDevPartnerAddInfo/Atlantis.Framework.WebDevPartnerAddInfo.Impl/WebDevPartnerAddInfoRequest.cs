using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.WebDevPartnerAddInfo.Interface;

namespace Atlantis.Framework.WebDevPartnerAddInfo.Impl
{
  public class WebDevPartnerAddInfoRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_webDevPartnerSubmissionInsert_sp";
    private const string _SHOPPERIDPARAM = "shopper_id";
    private const string _PRIVACYHASHKEY = "gpa_contactHashKey";
    private const string _APPLICATIONTYPE = "applicationType";
    private const string _APPLICATIONSTATE = "applicationState";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      WebDevPartnerAddInfoRequestData request = (WebDevPartnerAddInfoRequestData)oRequestData;
      int result = -1;
      try
      {
        string connectionString = Nimitz.NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = (int)request.RequestTimeout.TotalSeconds })
          {
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.Parameters.Add(new SqlParameter(_PRIVACYHASHKEY, request.PrivacyHashKey));
            command.Parameters.Add(new SqlParameter(_APPLICATIONTYPE, request.ApplicationType));
            command.Parameters.Add(new SqlParameter(_APPLICATIONSTATE, request.ApplicationState));
            SqlParameter newparam = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
            newparam.Direction = ParameterDirection.ReturnValue;

            connection.Open();
            command.ExecuteNonQuery();
            result = (int)command.Parameters["@ReturnValue"].Value;
          }
          oResponseData = new WebDevPartnerAddInfoResponseData(result);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new WebDevPartnerAddInfoResponseData(oRequestData, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new WebDevPartnerAddInfoResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}
