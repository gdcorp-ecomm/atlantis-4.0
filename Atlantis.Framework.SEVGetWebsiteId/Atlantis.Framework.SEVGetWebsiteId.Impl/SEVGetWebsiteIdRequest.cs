using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SEVGetWebsiteId.Interface;

namespace Atlantis.Framework.SEVGetWebsiteId.Impl
{
  public class SEVGetWebsiteIdRequest : IRequest
  {
    #region Constants
    private const string PROC_NAME = "dbo.tba_userwebsiteStatusGetForMYA_sp";
    private const string SHOPPER_ID_PARAM = "@shopper_id";
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SEVGetWebsiteIdResponseData responseData = null;
      Dictionary<int, SEVReplacementData> replacementDataDictionary = new Dictionary<int,SEVReplacementData>();

      try
      {
        var request = (SEVGetWebsiteIdRequestData)requestData;

        using (SqlConnection cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (SqlCommand cmd = new SqlCommand(PROC_NAME, cn))
          {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));
            cn.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
              while (dr.Read())
              {
                SEVReplacementData sevRD = new SEVReplacementData(dr["userwebsite_id"] == DBNull.Value ? "new" : dr["userwebsite_id"].ToString(), dr["websiteurl"] == DBNull.Value ? "New Account" : dr["websiteurl"].ToString().Trim());
                replacementDataDictionary.Add(Convert.ToInt32(dr["recurring_id"]), sevRD);
              }
            }
          }
        }
        responseData = new SEVGetWebsiteIdResponseData(replacementDataDictionary);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new SEVGetWebsiteIdResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new SEVGetWebsiteIdResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
