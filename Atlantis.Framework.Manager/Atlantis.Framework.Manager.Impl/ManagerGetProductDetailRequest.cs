using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.Manager.Impl
{
  public class ManagerGetProductDetailRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_getProductCatalogDetail_sp";
    private const string _PFIDPARAM = "@n_pf_id";
    private const string _PRIVATELABELIDPARAM = "@n_PrivateLabelID";
    private const string _ADMINFLAG = "@n_administratorFlag";
    private const string _MGRUSRID = "@mgr_user_id";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (ManagerGetProductDetailRequestData)requestData;

      var connStr = NetConnect.LookupConnectInfo(config);
      DataTable dataTable;

      using (var conn = new SqlConnection(connStr))
      {
        using (var cmd = new SqlCommand(_PROCNAME, conn))
        {
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.Add(_PFIDPARAM, SqlDbType.Decimal).Value = request.NonUnifiedPfid;
          cmd.Parameters.Add(_PRIVATELABELIDPARAM, SqlDbType.Int).Value = request.PrivateLabelId;
          cmd.Parameters.Add(_ADMINFLAG, SqlDbType.Int).Value = request.AdminFlag;
          cmd.Parameters.Add(_MGRUSRID, SqlDbType.Int).Value = request.ManagerUserId;
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;

          using (var adapter = new SqlDataAdapter(cmd))
          {
            dataTable = new DataTable("ProductCatalogDetail");
            adapter.Fill(dataTable);
          }
        }
      }

      return ManagerGetProductDetailResponseData.FromDataTable(dataTable);
    }
  }
}