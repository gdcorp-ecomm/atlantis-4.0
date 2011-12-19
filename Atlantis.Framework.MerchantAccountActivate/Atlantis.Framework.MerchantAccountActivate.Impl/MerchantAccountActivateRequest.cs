using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MerchantAccountActivate.Interface;

namespace Atlantis.Framework.MerchantAccountActivate.Impl
{
  public class MerchantAccountActivateRequest : IRequest
  {
    private const string PROC_NAME = "dbo.ma_merchantAccountSubmitApplication_sp";
    private const string SHOPPER_ID_PARAM = "@referralid";
    private const string MERCHANT_ACCOUNT_ID_PARAM = "@merchantaccountid";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MerchantAccountActivateResponseData responseData = null;
      string authenticationGuid = string.Empty;

      try
      {
        var request = (MerchantAccountActivateRequestData)requestData;

        using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var cmd = new SqlCommand(PROC_NAME, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(SHOPPER_ID_PARAM, request.ShopperID));
            cmd.Parameters.Add(new SqlParameter(MERCHANT_ACCOUNT_ID_PARAM, request.MerchantAccountId));
            cn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
              if (dr != null && dr.HasRows)
              {
                while (dr.Read())
                {
                  authenticationGuid = dr["authenticationGUID"] == System.DBNull.Value ? string.Empty : dr["authenticationGUID"].ToString();
                }
              }
            }
          }
          cn.Close();
        }

        responseData = new MerchantAccountActivateResponseData(authenticationGuid); 
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MerchantAccountActivateResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MerchantAccountActivateResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
