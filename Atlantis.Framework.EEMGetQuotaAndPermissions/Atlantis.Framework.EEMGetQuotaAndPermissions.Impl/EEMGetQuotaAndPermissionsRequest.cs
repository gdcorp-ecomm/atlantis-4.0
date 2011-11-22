using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.EEMGetQuotaAndPermissions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetQuotaAndPermissions.Impl
{
  public class EEMGetQuotaAndPermissionsRequest : IRequest
  {
    private const string PROC_NAME = "dbo.mya_GetCampaignBlazerProductList_sp";
    private const string PFID_PARAM = "@pfid";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EEMGetQuotaAndPermissionsResponseData responseData = null;
      int quota = 0;
      int permissions = 0;

      try
      {
        var request = (EEMGetQuotaAndPermissionsRequestData)requestData;

        using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var cmd = new SqlCommand(PROC_NAME, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(PFID_PARAM, request.Pfid));
            cn.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
              if (dr != null && dr.HasRows)
              {
                if (dr.Read())
                {
                  quota = Convert.ToInt32(dr["quota"]);
                  permissions = Convert.ToInt32(dr["permissions"]);
                }
              }
            }
          }
          cn.Close();
        }

        responseData = new EEMGetQuotaAndPermissionsResponseData(quota, permissions);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EEMGetQuotaAndPermissionsResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EEMGetQuotaAndPermissionsResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
