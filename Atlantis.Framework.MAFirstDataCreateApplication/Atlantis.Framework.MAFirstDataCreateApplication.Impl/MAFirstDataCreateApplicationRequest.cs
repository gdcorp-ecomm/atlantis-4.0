using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MAFirstDataCreateApplication.Interface;

namespace Atlantis.Framework.MAFirstDataCreateApplication.Impl
{
  public class MAFirstDataCreateApplicationRequest : IRequest
  {
    private const string PROC_NAME = "dbo.ma_??????????????_sp";
    private const string MERCHANT_ACCOUNT_ID_PARAM = "@merchantaccountid";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MAFirstDataCreateApplicationResponseData responseData = null;

      try
      {
        var request = (MAFirstDataCreateApplicationRequestData)requestData;

        using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var cmd = new SqlCommand(PROC_NAME, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(MERCHANT_ACCOUNT_ID_PARAM, request.MerchantAccountId));
            cn.Open();
            int x = cmd.ExecuteNonQuery();
          }
        }
        responseData = new MAFirstDataCreateApplicationResponseData();

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MAFirstDataCreateApplicationResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MAFirstDataCreateApplicationResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}