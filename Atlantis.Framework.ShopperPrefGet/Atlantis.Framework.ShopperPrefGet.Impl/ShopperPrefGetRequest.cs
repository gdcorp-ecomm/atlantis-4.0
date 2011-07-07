using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ShopperPrefGet.Interface;

namespace Atlantis.Framework.ShopperPrefGet.Impl
{
  public class ShopperPrefGetRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_shopperPreferenceGetByShopper_sp";
    private const string _SHOPPERIDPARAM = "shopper_id";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      Dictionary<string, string> preferences = new Dictionary<string, string>();

      bool hasData = false;
      string shopperIdFromDb = null;
      DateTime createDate = DateTime.MinValue;
      DateTime lastModifiedDate = DateTime.MinValue;

      try
      {
        ShopperPrefGetRequestData request = (ShopperPrefGetRequestData)oRequestData;

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
              while (reader.Read())
              {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                  if (reader.GetValue(i) != DBNull.Value)
                  {
                    string fieldName = reader.GetName(i);
                    switch (fieldName)
                    {
                      case "shopper_id":
                        shopperIdFromDb = reader.GetString(i);
                        break;
                      case "createDate":
                        createDate = reader.GetDateTime(i);
                        break;
                      case "modifyDate":
                        lastModifiedDate = reader.GetDateTime(i);
                        break;
                      default:
                        preferences.Add(fieldName, reader.GetValue(i).ToString());
                        break;
                    }

                  }
                }
                break;
              }
            }
          }
        }

        if ((hasData) && (shopperIdFromDb != request.ShopperID))
        {
          preferences.Clear();
        }

        if (preferences.Count == 0)
        {
          preferences = null;
        }

        oResponseData = new ShopperPrefGetResponseData(request.ShopperID, createDate, lastModifiedDate, preferences);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ShopperPrefGetResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ShopperPrefGetResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}