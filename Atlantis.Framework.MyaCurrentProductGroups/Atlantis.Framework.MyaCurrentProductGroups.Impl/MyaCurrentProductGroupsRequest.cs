using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaCurrentProductGroups.Interface;

namespace Atlantis.Framework.MyaCurrentProductGroups.Impl
{
  public class MyaCurrentProductGroupsRequest : IRequest
  {
    private const string _PROCNAME = "mya_getCurrentProductGroups_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      MyaCurrentProductGroupsRequestData currentProductGroupsRequest = (MyaCurrentProductGroupsRequestData)oRequestData;
      MyaCurrentProductGroupsResponseData result;

      try
      {
        HashSet<int> productGroupIds = new HashSet<int>();

        string connectionString = Nimitz.NetConnect.LookupConnectInfo(oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("s_shopper_id", currentProductGroupsRequest.ShopperID));
            command.Parameters.Add(new SqlParameter("s_privateLabelID", currentProductGroupsRequest.PrivateLabelId));
            command.CommandTimeout = (int)currentProductGroupsRequest.RequestTimeout.TotalSeconds;
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                object productGroupId = reader[0];
                if ((productGroupId != null) && (productGroupId.GetType() != typeof(DBNull)))
                {
                  int productGroupIdInt;
                  if (int.TryParse(productGroupId.ToString(), out productGroupIdInt))
                  {
                    productGroupIds.Add(productGroupIdInt);
                  }
                }
              }
            }
          }
        }

        result = new MyaCurrentProductGroupsResponseData(productGroupIds);
      }
      catch (Exception ex)
      {
        result = new MyaCurrentProductGroupsResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion

  }
}
