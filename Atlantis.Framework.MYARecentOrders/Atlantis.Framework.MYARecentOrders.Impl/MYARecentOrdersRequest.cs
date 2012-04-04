using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYARecentOrders.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MYARecentOrders.Impl
{
  public class MYARecentOrdersRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MYARecentOrdersResponseData responseData = null;
      List<RecentOrder> recentOrders = new List<RecentOrder>();

      try
      {
        MYARecentOrdersRequestData recentOrderRequestData = (MYARecentOrdersRequestData)requestData;
        recentOrders = GetRecentOrders(recentOrderRequestData, config);

        responseData = new MYARecentOrdersResponseData(recentOrders);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MYARecentOrdersResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MYARecentOrdersResponseData(requestData, ex);
      }

      return responseData;
    }

    public List<RecentOrder> GetRecentOrders(MYARecentOrdersRequestData requestData, ConfigElement config)
    {
      List<RecentOrder> recentOrders = new List<RecentOrder>();
      string procName = "dbo.mya_getRecentOrdersByShopper_sp";

      string connectionString = NetConnect.LookupConnectInfo(config);
      using (SqlConnection cn = new SqlConnection(connectionString))
      {
        using (SqlCommand cmd = new SqlCommand(procName, cn))
        {
          cmd.CommandTimeout = (int)requestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add(new SqlParameter("@shopper_id", requestData.ShopperID));
          cmd.Parameters.Add(new SqlParameter("@numberToGet", requestData.OrderCount));
          cmd.Parameters.Add(new SqlParameter("@daysToSearch", requestData.DaysToSearch));
          cn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader())
          {
            while (dr.Read())
            {
              recentOrders.Add(PopulateObjectFromDB(dr, requestData.ShopperID));
            }
          }
        }
      }
      return recentOrders;
    }

    private RecentOrder PopulateObjectFromDB(SqlDataReader dr, string shopperId)
    {
      RecentOrder recentOrder = new RecentOrder();
      recentOrder.ShopperId = shopperId;
      recentOrder.DateEntered = Convert.ToDateTime(dr["date_entered"]);
      recentOrder.IsRefund = Convert.ToBoolean(dr["isRefund"]);
      recentOrder.OrderId = Convert.ToString(dr["order_id"]);
      recentOrder.OrderSource = Convert.ToString(dr["order_source"]);
      recentOrder.TransactionTotal = Convert.ToInt32(dr["transactionTotal"]);
      recentOrder.TransactionCurrency = Convert.ToString(dr["transactionCurrency"]);
      return recentOrder;
    }

  }
}
