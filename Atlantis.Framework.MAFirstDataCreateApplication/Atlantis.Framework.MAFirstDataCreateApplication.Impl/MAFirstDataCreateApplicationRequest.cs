using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MAFirstDataCreateApplication.Interface;

namespace Atlantis.Framework.MAFirstDataCreateApplication.Impl
{
  public class MAFirstDataCreateApplicationRequest : IRequest
  {
    #region Constants
    private const string PROC_NAME = "dbo.ma_merchantAccountUpdateApplicationDate_sp";
    private const string MERCHANT_ACCOUNT_ID_PARAM = "@merchantaccountid";
    private const string RETVAL_PARAM = "@return_value";
    private const string ERROR_MSG = "Error attempting to update application date for MerchantAccountId: {0}";
    private const int SUCCESS = 0;
    #endregion

    #region Properties
    private string ErrorMessage { get; set; }
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MAFirstDataCreateApplicationResponseData responseData = null;
      ErrorMessage = string.Empty;

      try
      {
        int sqlResponse = -1;
        var request = (MAFirstDataCreateApplicationRequestData)requestData;

        using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var cmd = new SqlCommand(PROC_NAME, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(MERCHANT_ACCOUNT_ID_PARAM, request.MerchantAccountId));
            SqlParameter returnParam = new SqlParameter(RETVAL_PARAM, SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnParam);
            cn.Open();
            try
            {
              cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
              ErrorMessage = ex.Message.Contains("No rows updated") ? "No rows updated.  " + string.Format(ERROR_MSG, request.MerchantAccountId) : string.Format(ERROR_MSG, request.MerchantAccountId);
            }
            sqlResponse = (int)cmd.Parameters[RETVAL_PARAM].Value;
          }
        }
        if (sqlResponse.Equals(SUCCESS) && string.IsNullOrEmpty(ErrorMessage))
        {
          responseData = new MAFirstDataCreateApplicationResponseData();
        }
        else
        {
          ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? string.Format(ERROR_MSG, request.MerchantAccountId) : ErrorMessage;
          Exception ex = new Exception(ErrorMessage);
          responseData = new MAFirstDataCreateApplicationResponseData(requestData, ex);
        }       
      }

      catch (Exception ex)
      {
        responseData = new MAFirstDataCreateApplicationResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}