using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.Manager.Impl
{
  public class ManagerGetProductDetailRequest : IRequest
  {
    private const string PROCNAME = "gdshop_getProductCatalogDetail_sp";
    private const string PFIDPARAM = "@n_pf_id";
    private const string PRIVATELABELIDPARAM = "@n_PrivateLabelID";
    private const string ADMINFLAG = "@n_administratorFlag";
    private const string MGRUSRID = "@mgr_user_id";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData oResponseData;

      try
      {
        var request = (ManagerGetProductDetailRequestData) requestData;

        var connStr = NetConnect.LookupConnectInfo(config);
        DataTable dt;
        using (var conn = new SqlConnection(connStr))
        {
          using (var cmd = new SqlCommand(PROCNAME, conn))
          {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(PFIDPARAM, SqlDbType.Decimal).Value = request.Pfid;
            cmd.Parameters.Add(PRIVATELABELIDPARAM, SqlDbType.Int).Value = request.PrivateLabelId;
            cmd.Parameters.Add(ADMINFLAG, SqlDbType.Int).Value = request.AdminFlag;
            cmd.Parameters.Add(MGRUSRID, SqlDbType.Int).Value = request.ManagerUserId;
            cmd.CommandTimeout = (int) Math.Truncate(requestData.RequestTimeout.TotalSeconds);

            using (var adapter = new SqlDataAdapter(cmd))
            {
              dt = new DataTable("ProductCatalogDetail");
              adapter.Fill(dt);
            }
          }
        }
        oResponseData = new ManagerGetProductDetailResponseData(dt);
      }
      catch (AtlantisException atlantisEx)
      {
        oResponseData = new ManagerGetProductDetailResponseData(atlantisEx);
      }
      catch (Exception ex)
      {
        oResponseData = new ManagerGetProductDetailResponseData(requestData, ex);
      }

      return oResponseData;
    }
  }
}