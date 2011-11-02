using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.GetRemainPLDur.Interface;
using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.GetRemainPLDur.Impl
{
  public class GetRemainPLDurRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "rex_getRemainingPrivateLabelDuration_sp";
    private const string RECURRING_ID_PARAM = "@n_recurring_id";
    private const string DURATION_PARAM = "@n_duration";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GetRemainPLDurResponseData responseData;

      try
      {
        GetRemainPLDurRequestData remainingPLDurationRequestData = (GetRemainPLDurRequestData)requestData;
        decimal duration = GetDuration(remainingPLDurationRequestData, config);
        responseData = new GetRemainPLDurResponseData(duration);
        
      }
      catch (AtlantisException atlantisException)
      {
        responseData = new GetRemainPLDurResponseData(atlantisException);
      }
      catch (Exception ex)
      {
        responseData = new GetRemainPLDurResponseData(requestData, ex);
      }

      return responseData;
    }

    private static decimal GetDuration(GetRemainPLDurRequestData requestData, ConfigElement config)
    {
      decimal duration = 1;

      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        cn.Open();
        DataSet ds = new DataSet(Guid.NewGuid().ToString());

        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter(RECURRING_ID_PARAM, requestData.EntityId));
          SqlParameter durationParam = cmd.Parameters.Add(DURATION_PARAM, SqlDbType.Decimal);
          durationParam.Direction = ParameterDirection.Output;

          using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
          {
            adp.Fill(ds);
          }
          
          Decimal.TryParse(cmd.Parameters[DURATION_PARAM].Value.ToString(), out duration);
        }
      }

      return duration;
    }
  }
}
