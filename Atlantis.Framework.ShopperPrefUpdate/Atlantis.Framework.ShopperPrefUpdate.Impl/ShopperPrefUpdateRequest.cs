using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ShopperPrefUpdate.Interface;

namespace Atlantis.Framework.ShopperPrefUpdate.Impl
{
  public class ShopperPrefUpdateRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_shopperPreferenceUpdate_sp";
    private const string _SHOPPERIDPARAM = "shopper_id";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      int resultCode = -1;
      Dictionary<string, string> preferences = new Dictionary<string, string>();

      try
      {
        ShopperPrefUpdateRequestData request = (ShopperPrefUpdateRequestData)oRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));

            foreach (string key in request.GetPreferenceKeys())
            {
              command.Parameters.Add(new SqlParameter(key, request.GetPreference(key)));
            }

            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();
            resultCode = command.ExecuteNonQuery();
          }
        }

        oResponseData = new ShopperPrefUpdateResponseData(resultCode);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ShopperPrefUpdateResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ShopperPrefUpdateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
