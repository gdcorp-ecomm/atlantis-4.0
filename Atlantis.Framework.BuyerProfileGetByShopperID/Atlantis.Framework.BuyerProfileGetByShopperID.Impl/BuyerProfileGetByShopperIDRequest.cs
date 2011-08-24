using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BuyerProfileGetByShopperID.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.BuyerProfileGetByShopperID.Impl
{
  public class BuyerProfileGetByShopperIDRequest : IRequest
  {
    private const string PROC_NAME = "mya_GetBuyerProfiles_sp";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      BuyerProfileGetByShopperIDRequestData request = (BuyerProfileGetByShopperIDRequestData)oRequestData;

      DataSet ds = null;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@shopper_id", request.ShopperID));

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
          }
        }
        List<ProfileSummary> _profiles = ProcessProfiles(ds);
        oResponseData = new BuyerProfileGetByShopperIDResponseData(_profiles);
      }
      catch (Exception ex)
      {
        oResponseData = new BuyerProfileGetByShopperIDResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    private List<ProfileSummary> ProcessProfiles(DataSet ds)
    {
      List<ProfileSummary> _profiles = new List<ProfileSummary>();

      if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        string name = string.Empty;
        string id = string.Empty;
        bool isdefault = false;
        
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
          id = ds.Tables[0].Rows[i]["gdshop_BuyerProfileID"].ToString();
          name= ds.Tables[0].Rows[i]["profileName"].ToString();
          isdefault = ds.Tables[0].Rows[i]["defaultProfileFlag"].ToString() == "1";
          ProfileSummary ps = new ProfileSummary(id, name, isdefault);
          _profiles.Add(ps);
          name = id = string.Empty; isdefault = false;
        }
        
      }

      return _profiles;

    }

    #endregion
  }
}
