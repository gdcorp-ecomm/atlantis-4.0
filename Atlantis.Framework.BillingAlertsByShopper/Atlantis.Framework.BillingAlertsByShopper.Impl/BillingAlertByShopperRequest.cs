using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.BillingAlertsByShopper.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BillingAlertsByShopper.Impl
{
  public class BillingAlertsByShopperRequest : IRequest
  {

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      BillingAlertsByShopperResponseData responseData = null;
      List<BillingAlert> billingAlerts = new List<BillingAlert>();
      try
      {
        BillingAlertsByShopperRequestData billingAlertRequestData = (BillingAlertsByShopperRequestData)requestData;
              
        foreach (int productGroup in billingAlertRequestData.ProductGroupIds)
        {
          string procName = GetProcName(productGroup);
          if (!string.IsNullOrEmpty(procName))
          {
            GetBillingAlert((int)productGroup, procName, ref billingAlerts, billingAlertRequestData, config);

            if ((billingAlertRequestData.TotalAlertsThreshold != BillingAlertThreshold.All) &&
              (billingAlerts.Count >= billingAlertRequestData.TotalAlertsThreshold))
            {
              break;
            }
          }
        }

        responseData = new BillingAlertsByShopperResponseData(billingAlerts);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new BillingAlertsByShopperResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new BillingAlertsByShopperResponseData(requestData, ex);
      }

      return responseData;
    }

    private void GetBillingAlert(int productGroupId, string procName, ref List<BillingAlert> billingAlerts, BillingAlertsByShopperRequestData requestData, ConfigElement config)
    {
      int billingFailureResourceId = 0;
      int setUpResourceId = 0;
      int expiringResourceId = 0;

      using (SqlConnection connection = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (SqlCommand command = new SqlCommand(procName, connection))
        {
          command.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.Add(new SqlParameter("shopper_id", requestData.ShopperID));
          connection.Open();
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              int resourceId = reader["resource_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["resource_id"]);
              bool billingFailure = reader["isBillingFailure"] == DBNull.Value ? false : Convert.ToBoolean(reader["isBillingFailure"]);
              
              if (billingFailureResourceId == 0)
              {
                if (billingFailure)
                {
                  billingFailureResourceId = resourceId;
                }
              }
              if (setUpResourceId == 0)
              {
                if (reader["isSetup"] != DBNull.Value && Convert.ToBoolean(reader["isSetup"]) == false)
                {
                  setUpResourceId = resourceId;
                }
              }

              if (expiringResourceId == 0)
              {
                if (reader["isExpiring"] != DBNull.Value && Convert.ToBoolean(reader["isExpiring"]) == true)
                {
                  if (!billingFailure)
                  {
                    expiringResourceId = resourceId;
                  }
                }
              }

              if (expiringResourceId != 0 && setUpResourceId != 0 && billingFailureResourceId != 0)
              {
                break;
              }
            }
          }
          
          if (expiringResourceId != 0 || setUpResourceId != 0 || billingFailureResourceId != 0)
          {
            BillingAlert alert = new BillingAlert(productGroupId, billingFailureResourceId, setUpResourceId, expiringResourceId);
            billingAlerts.Add(alert);
          }          
        }
      }
    }

    private string GetProcName(int productGroup)
    {
      string procName = string.Empty;

      switch (productGroup)
      {
        case 1:
          procName = "gdshop_billingGetAlertsByShopperHosting_sp";
          break;

        case 24:
        case 25:
          procName = "gdshop_billingGetAlertsByShopperDedHost_sp";
          break;

        //TODO: use this case when billing tables are split in prod for ded and vded
        //case 25:
          //procName = "gdshop_billingGetAlertsByShopperVirtualHosting_sp";
          //break;

        case 23:
          procName = "gdshop_billingGetAlertsByShopperHostingQSC_sp";
          break;

        case 15:
          procName = "gdshop_billingGetAlertsByShopperHostingWST_sp";
          break;

        case 28:
          procName = "gdshop_billingGetAlertsByShopperPrivateLabel_sp";
          break;

        case 54:
          procName = "gdshop_billingGetAlertsByShopperCustomDesign_sp";
          break;

        case 59:
          procName = "gdshop_billingGetAlertsByShopperSmartDomain_sp";
          break;

        case 21:
          procName = "gdshop_billingGetAlertsByShopperCampaignBlazer_sp";
          break;

        case 38:
          procName = "gdshop_billingGetAlertsByShopperWebLog_sp";
          break;

        case 49:
          procName = "gdshop_billingGetAlertsByShopperDeluxeStat_sp";
          break;

        case 13:
          procName = "gdshop_billingGetAlertsByShopperTrafficBlazer_sp";
          break;

        case 22:
          procName = "gdshop_billingGetAlertsByShopperSSL_sp";
          break;

        case 26:
          procName = "gdshop_billingGetAlertsByShopperFaxEmail_sp";
          break;

        case 4:
          procName = "gdshop_billingGetAlertsByShopperEmail_sp";
          break;

        case 66:
          procName = "gdshop_billingGetAlertsByShopperCustMgr_sp";
          break;

        case 74:
          procName = "gdshop_billingGetAlertsByShopperSurvey_sp";
          break;
      }

      return procName;
    }
  }
}
