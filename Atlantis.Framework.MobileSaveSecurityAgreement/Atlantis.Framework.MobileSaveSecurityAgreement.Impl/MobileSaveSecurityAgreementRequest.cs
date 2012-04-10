using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MobileSaveSecurityAgreement.Interface;
using System.Data.SqlClient;
using System.Data;
using netConnect;

namespace Atlantis.Framework.MobileSaveSecurityAgreement.Impl
{
  public class MobileSaveSecurityAgreementRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MobileSaveSecurityAgreementResponseData responseData = null;

      try
      {
        MobileSaveSecurityAgreementRequestData shopperDataUpdateRequestData = (MobileSaveSecurityAgreementRequestData)requestData;
        bool result = UpdateShopperData(shopperDataUpdateRequestData, config);
        responseData = new MobileSaveSecurityAgreementResponseData(result);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new MobileSaveSecurityAgreementResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new MobileSaveSecurityAgreementResponseData(requestData, ex);
      }

      return responseData;
    }

    #endregion

    private bool UpdateShopperData(MobileSaveSecurityAgreementRequestData requestData, ConfigElement config)
    {
      string procName = "dbo.gdshop_mobileSecurityAgreementInsert_sp";
      bool successful = false;

      using (SqlConnection conn = new SqlConnection(LookupConnectionString(requestData, config)))
      {
        using (SqlCommand cmd = new SqlCommand(procName, conn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@shopper_id", requestData.ShopperID));
          cmd.Parameters.Add(new SqlParameter("@gdshop_mobileApplicationTypeID",requestData.MobileAppType ));
          cmd.Parameters.Add(new SqlParameter("@gdshop_mobileDeviceTypeID", requestData.AppId));
          cmd.Parameters.Add(new SqlParameter("@gdshop_mobileLegalAgreementTypeID", requestData.AgreementType));
          cmd.Parameters.Add(new SqlParameter("@deviceID", requestData.DeviceId));
          conn.Open();
          cmd.ExecuteNonQuery();

          successful = true;
        }        
      }

      return successful;
    }

    private string LookupConnectionString(MobileSaveSecurityAgreementRequestData requestData, ConfigElement config)
    {
      string result = string.Empty;

      netConnect.Info nc = new netConnect.Info();
      result = nc.Get(config.GetConfigValue("DataSourceName")
        , config.GetConfigValue("ApplicationName")
        , config.GetConfigValue("CertificateName")
        , ConnectTypeEnum.CONNECT_TYPE_NET);

      if (string.IsNullOrEmpty(result) || result.Length <= 2)
      {
        throw new Exception("Invalid Connection String");
      }
      return result;
    }
  }
}
