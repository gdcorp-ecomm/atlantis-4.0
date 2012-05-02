using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.AccountExecContactInfo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.AccountExecContactInfo.Impl
{
  public class AccountExecContactInfoRequest : IRequest
  {
    #region Database Constants
    private const string PROC_NAME = "dbo.MYA_PortfolioShopperTag_sp";
    private const string SHOPPER_PARAM = "@shopper_id";
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AccountExecContactInfoResponseData responseData = null;
      VipInfo vipInfo = null;

      try
      {
        var request = (AccountExecContactInfoRequestData)requestData;
        vipInfo = GetVipInfo(request, config);

        responseData = new AccountExecContactInfoResponseData(vipInfo);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new AccountExecContactInfoResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new AccountExecContactInfoResponseData(requestData, ex);
      }

      return responseData;
    }

    private VipInfo GetVipInfo(AccountExecContactInfoRequestData request, ConfigElement config)
    {
      VipInfo info = null;

      using (SqlConnection cn = new SqlConnection(NetConnect.LookupConnectInfo(config)))
      {
        using (SqlCommand cmd = new SqlCommand(PROC_NAME, cn))
        {
          cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));
          cn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader())
          {
            while (dr.Read())
            {
              info = PopulateObjectFromDB(dr);
            }
          }
        }
      }
      return info;
    }

    private VipInfo PopulateObjectFromDB(IDataReader idr)
    {
      string name = idr["AssignedRepName"].ToString();
      string email = idr["AssignedRepEmail"].ToString();
      string portfolioType = idr["PortfolioType"].ToString();
      string phone = idr["MYAExternalNumberDailIn"].ToString();
      string ext = idr["AssignedRepExt"].ToString();

      return new VipInfo(name, email, portfolioType, phone, ext);
    }
  }
}
