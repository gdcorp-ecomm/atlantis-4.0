using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MktgSetShopperCommPrivacyHash.Interface;

namespace Atlantis.Framework.MktgSetShopperCommPrivacyHash.Impl
{
  public class MktgSetShopperCommPrivacyHashRequest : IRequest
  {
    private const string _PROCNAME = "gdshop_shopper_mtm_gdshop_shopperCommunicationTypeAddUpdate_sp";
    private const string _SHOPPERIDPARAM = "shopper_id";
    private const string _COMMUNICATIONTYPEID = "gdshop_shopperCommunicationTypeID";
    private const string _PRIVACYHASH = "privacyHash";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData oResponseData = null;
      MktgSetShopperCommPrivacyHashRequestData request = null;
      try
      {
        request = (MktgSetShopperCommPrivacyHashRequestData)requestData;
        string connectionString = Nimitz.NetConnect.LookupConnectInfo(config);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.Parameters.Add(new SqlParameter(_COMMUNICATIONTYPEID, request.CommunicationTypeID));
            command.Parameters.Add(new SqlParameter(_PRIVACYHASH, request.PrivacyHash));
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            connection.Open();
            command.ExecuteNonQuery();
          }
        }
        oResponseData = new MktgSetShopperCommPrivacyHashResponseData();
      }
      catch (Exception ex)
      {
        oResponseData = new MktgSetShopperCommPrivacyHashResponseData(request, ex);
      }

      return oResponseData;
    }
  }
}
