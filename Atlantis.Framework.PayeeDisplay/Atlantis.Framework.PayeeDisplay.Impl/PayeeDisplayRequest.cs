using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.PayeeDisplay.Interface;
using Atlantis.Framework.PayeeDisplay.Impl.PayeeWS;

namespace Atlantis.Framework.PayeeDisplay.Impl
{
  public class PayeeDisplayRequest :IRequest
  {
    private const string PROCNAME = "mya_getVendorDisplayIndicator_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      var request = (PayeeDisplayRequestData)requestData;
      try
      {
        var connectionString = NetConnect.LookupConnectInfo(config);
        bool payeeDisplay;
        using (var connection = new SqlConnection(connectionString))
        {
          using (var command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds / 2;
            command.Parameters.Add(new SqlParameter("@s_shopper_id", request.ShopperID));

            var newparam = command.Parameters.Add("@n_displayFlag", SqlDbType.Int);
            newparam.Direction = ParameterDirection.Output;

            connection.Open();
            command.ExecuteNonQuery();
            payeeDisplay = (int)command.Parameters["@n_displayFlag"].Value == 1;
          }
        }
        if (!payeeDisplay)
        {
          var wsConfigElement = (WsConfigElement)config;
          var wsTimeout = (int)request.RequestTimeout.TotalMilliseconds / 2;
          string errors;
          payeeDisplay = CheckForSetupAccounts(request.ShopperID, wsConfigElement, wsTimeout, out errors);
          if (!string.IsNullOrEmpty(errors))
          {
            var ex = new Exception(errors);
            responseData = new PayeeDisplayResponseData(requestData, ex);
          }
          else
          {
            responseData = new PayeeDisplayResponseData(payeeDisplay);
          }
        }
        else
        {
          responseData = new PayeeDisplayResponseData(true);  
        }        
      }
      catch (Exception ex)
      {
        responseData = new PayeeDisplayResponseData(requestData, ex);
      }

      return responseData;
    }

    private static bool CheckForSetupAccounts(string shopperId, WsConfigElement wsConfigElement, int wsTimeout, out string errors)
    {
      var cert = wsConfigElement.GetClientCertificate();
      if (cert == null)
      {
        throw new Exception("Client certificate not found.");
      }
      cert.Verify();

      int responseCode;

      using (var svc = new WSCgdCAPService())
      {
        svc.Url = wsConfigElement.WSURL;     
        svc.Timeout = wsTimeout;
        svc.ClientCertificates.Add(cert);
        responseCode = svc.ShopperHasAccount(shopperId, out errors);
      }

      return responseCode < 0;  // -1 = has accounts, 0 = no accounts
    }

    #endregion

    }
}
