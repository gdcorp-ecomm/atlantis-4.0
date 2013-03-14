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
      AccountExecContactInfoResponseData responseData;

      try
      {
        var request = (AccountExecContactInfoRequestData)requestData;
        var vipInfo = GetVipInfo(request, config);

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

    private static VipInfo GetVipInfo(RequestData request, ConfigElement config)
    {
      VipInfo info = null;

      using (var cn = new SqlConnection(NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(PROC_NAME, cn))
        {
          cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter(SHOPPER_PARAM, request.ShopperID));
          cn.Open();
          using (var dr = cmd.ExecuteReader())
          {
            while (dr.Read())
            {
              info = PopulateObjectFromDb(dr);
            }
          }
        }
      }
      return info;
    }

    private static VipInfo PopulateObjectFromDb(IDataRecord idr)
    {
      PortfolioTypes portfolioTypeId = 0;
      var name = idr["AssignedRepName"].ToString();
      var email = idr["AssignedRepEmail"].ToString();
      int temp;
      if (int.TryParse(idr["crm_portfolioTypeID"].ToString(), out temp))
      {
        portfolioTypeId = (PortfolioTypes) temp;
      }
      var portfolioType = idr["PortfolioType"].ToString();
      var phone = idr["MYAExternalNumberDailIn"].ToString();
      var ext = idr["AssignedRepExt"].ToString();

      return new VipInfo(name, email, portfolioTypeId, portfolioType, phone, ext);
    }
  }
}
