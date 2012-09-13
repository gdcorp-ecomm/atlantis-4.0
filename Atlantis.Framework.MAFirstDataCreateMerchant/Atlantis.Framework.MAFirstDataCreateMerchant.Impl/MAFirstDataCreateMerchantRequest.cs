using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MAFirstDataCreateMerchant.Interface;

namespace Atlantis.Framework.MAFirstDataCreateMerchant.Impl
{
  public class MAFirstDataCreateMerchantRequest : IRequest
  {
    #region Constants
    private const string PROC_NAME = "dbo.ma_merchantAccountUpdateVendorResourceID_sp";
    private const string MERCHANT_ACCOUNT_ID_PARAM = "@merchantaccountid";
    private const string VENDOR_RESOURCE_ID_PARAM = "@vendorResourceID";
    private const string RETVAL_PARAM = "@return_value";
    private const string ERROR_MSG = "Error attempting to update MerchantID (MID) for MerchantAccountId: {0}";
    private const int SUCCESS = 0;
    #endregion

    #region Properties
    private string ErrorMessage { get; set; }
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MAFirstDataCreateMerchantResponseData responseData = null;

      try
      {
        int sqlResponse = -1;
        var request = (MAFirstDataCreateMerchantRequestData)requestData;

        using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var cmd = new SqlCommand(PROC_NAME, cn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(MERCHANT_ACCOUNT_ID_PARAM, request.MerchantAccountId));
            cmd.Parameters.Add(new SqlParameter(VENDOR_RESOURCE_ID_PARAM, request.VendorResourceId));
            SqlParameter returnParam = new SqlParameter(RETVAL_PARAM, SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnParam);
            cn.Open();
            try
            {
              ErrorMessage = string.Empty;
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
          responseData = new MAFirstDataCreateMerchantResponseData();
        }
        else
        {
          ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? string.Format(ERROR_MSG, request.MerchantAccountId) : ErrorMessage;
          Exception ex = new Exception(ErrorMessage);
          responseData = new MAFirstDataCreateMerchantResponseData(requestData, ex);
        } 
      }

      catch (Exception ex)
      {
        responseData = new MAFirstDataCreateMerchantResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
