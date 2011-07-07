using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ShopperPrefGetModDate.Interface;

namespace Atlantis.Framework.ShopperPrefGetModDate.Impl
{
  public class ShopperPrefGetModDateRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_shopperPreferenceGetModifyDateByShopper_sp";
    private const string _SHOPPERIDPARAM = "shopper_id";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      string shopperId = string.Empty;
      DateTime modDate = DateTime.MinValue;

      try
      {
        ShopperPrefGetModDateRequestData request = (ShopperPrefGetModDateRequestData)oRequestData;

        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
              shopperId = request.ShopperID;
              while (reader.Read())
              {
                object modDateObj = reader[0];
                if (modDateObj != System.DBNull.Value)
                {
                  modDate = Convert.ToDateTime(modDateObj);
                }
                break;
              }
            }
          }
        }

        oResponseData = new ShopperPrefGetModDateResponseData(shopperId, modDate);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ShopperPrefGetModDateResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ShopperPrefGetModDateResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}
