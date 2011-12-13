using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.GetRemCampBlazDur.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetRemCampBlazDur.Impl
{
  public class GetRemCampBlazDurRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "gdshop_getRemainingCampaignBlazerDuration_sp";
    private const string RECURRING_ID_PARAM = "@n_recurring_id";
    private const string DURATION_PARAM = "@n_duration";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GetRemCampBlazDurResponseData responseData;

      try
      {
        GetRemCampBlazDurRequestData remainingCampBlazDurationRequestData = (GetRemCampBlazDurRequestData)requestData;
        decimal duration = GetDuration(remainingCampBlazDurationRequestData, config);
        responseData = new GetRemCampBlazDurResponseData(duration);

      }
      catch (AtlantisException atlantisException)
      {
        responseData = new GetRemCampBlazDurResponseData(atlantisException);
      }
      catch (Exception ex)
      {
        responseData = new GetRemCampBlazDurResponseData(requestData, ex);
      }

      return responseData;
    }

    private static decimal GetDuration(GetRemCampBlazDurRequestData requestData, ConfigElement config)
    {
      decimal duration = 1M;

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
          durationParam.Precision = 18;
          durationParam.Scale = 3;

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
