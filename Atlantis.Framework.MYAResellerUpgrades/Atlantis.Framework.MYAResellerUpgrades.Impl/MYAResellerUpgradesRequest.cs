using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAResellerUpgrades.Interface;
//using netConnect;
using System.Data.SqlClient;
using System.Data;

namespace Atlantis.Framework.MYAResellerUpgrades.Impl
{
  public class MYAResellerUpgradesRequest : IRequest
  {
    #region Parameter Constants

    private const string CONFIG_STORED_PROCEDURE = "gdshop_resellerUpgradeByRecurringID_sp";
    private const string RESOURCE_ID_PARAM = "@n_recurring_id";

    #endregion Parameter Constants

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MYAResellerUpgradesResponseData responseData = null;
      List<ResellerUpgrade> resellerUpgrades = new List<ResellerUpgrade>();

      try
      {
        MYAResellerUpgradesRequestData myaResellerUpgradeRequestData = (MYAResellerUpgradesRequestData)requestData;
        resellerUpgrades = GetUpgrades(myaResellerUpgradeRequestData, config);

        responseData = new MYAResellerUpgradesResponseData(resellerUpgrades);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MYAResellerUpgradesResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MYAResellerUpgradesResponseData(requestData, ex);
      }

      return responseData;
    }

    private List<ResellerUpgrade> GetUpgrades(MYAResellerUpgradesRequestData requestData, ConfigElement config)
    {
      List<ResellerUpgrade> resellerUpgrades = new List<ResellerUpgrade>();
      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        cn.Open();

        using (var cmd = new SqlCommand(CONFIG_STORED_PROCEDURE, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter(RESOURCE_ID_PARAM, requestData.BillingResourceId));

          using (SqlDataReader dr = cmd.ExecuteReader())
          {
            while (dr.Read())
            {
              resellerUpgrades.Add(PopulateObjectFromDB(dr));
            }
          }
        }
      }

      return resellerUpgrades;
    }

    private ResellerUpgrade PopulateObjectFromDB(IDataReader dr)
    {
      ResellerUpgrade resellerUpgrade = new ResellerUpgrade();

      resellerUpgrade.ProductId = Convert.ToInt32(dr["upgrade_pf_id"]);
      resellerUpgrade.Description = dr["name"] == DBNull.Value ? string.Empty : Convert.ToString(dr["name"].ToString());

      return resellerUpgrade;
    }
  }
}
