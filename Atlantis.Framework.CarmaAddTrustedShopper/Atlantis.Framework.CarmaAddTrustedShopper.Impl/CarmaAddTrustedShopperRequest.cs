using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Atlantis.Framework.CarmaAddTrustedShopper.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaAddTrustedShopper.Impl
{
  public class CarmaAddTrustedShopperRequest : IRequest
  {
    #region Database Constants
    const string PROC_NAME = "dbo.carma_trustedShopperAdd_sp";
    const string PARAM_PRIMARY_SHOPPER_ID = "@s_primary_id";
    const string PARAM_SECONDARY_SHOPPER_ID = "@s_secondary_id";
    #endregion

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CarmaAddTrustedShopperResponseData responseData = null;

      try
      {
        CarmaAddTrustedShopperRequestData request = (CarmaAddTrustedShopperRequestData)requestData;

        if (string.Compare(request.PrimaryShopperId, request.ShopperID, true, CultureInfo.InvariantCulture).Equals(0))
        {
          AtlantisException aex = new AtlantisException(request, "CarmaAddTrustedShopperRequest::RequestHandler", "Account owner cannot be assigned as an AccountExec", string.Empty);
          responseData = new CarmaAddTrustedShopperResponseData(aex);
        }
        else
        {
          using (SqlConnection cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
          {
            using (SqlCommand cmd = new SqlCommand(PROC_NAME, cn))
            {
              cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Parameters.Add(new SqlParameter(PARAM_PRIMARY_SHOPPER_ID, request.PrimaryShopperId));
              cmd.Parameters.Add(new SqlParameter(PARAM_SECONDARY_SHOPPER_ID, request.ShopperID));
              cn.Open();
              cmd.ExecuteNonQuery();
            }
          }
          responseData = new CarmaAddTrustedShopperResponseData();
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new CarmaAddTrustedShopperResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CarmaAddTrustedShopperResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
