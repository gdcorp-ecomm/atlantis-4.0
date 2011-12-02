using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ManagerGetProductDetail.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ManagerGetProductDetail.Impl
{
  public class ManagerGetProductDetailRequest: IRequest
  {
    private const string _PROCNAME = "gdshop_getProductCatalogDetail_sp";
    private const string _PFIDPARAM = "@n_pf_id";
    private const string _PRIVATELABELIDPARAM = "@n_PrivateLabelID";
    private const string _ADMINFLAG = "@n_administratorFlag";
    private const string _MGRUSRID = "@mgr_user_id";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData oResponseData = null;
      DataTable dt = null;

      try
      {
        ManagerGetProductDetailRequestData request = (ManagerGetProductDetailRequestData)requestData;

        string connStr = NetConnect.LookupConnectInfo(config);
        using (SqlConnection conn = new SqlConnection(connStr))
        {
          using (SqlCommand cmd = new SqlCommand(_PROCNAME, conn))
          {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(_PFIDPARAM, SqlDbType.Decimal).Value = request.Pfid;
            cmd.Parameters.Add(_PRIVATELABELIDPARAM, SqlDbType.Int).Value = request.PrivateLabelId;
            cmd.Parameters.Add(_ADMINFLAG, SqlDbType.Int).Value = request.AdminFlag;
            cmd.Parameters.Add(_MGRUSRID, SqlDbType.Int).Value = request.ManagerUserId;
            cmd.CommandTimeout = (int)Math.Truncate(requestData.RequestTimeout.TotalSeconds);

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
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
