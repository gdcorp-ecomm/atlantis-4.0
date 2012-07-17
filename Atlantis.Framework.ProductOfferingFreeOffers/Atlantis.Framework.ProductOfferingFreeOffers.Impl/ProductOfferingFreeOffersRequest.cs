using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ProductOfferingFreeOffers.Interface;

namespace Atlantis.Framework.ProductOfferingFreeOffers.Impl
{
  public class ProductOfferingFreeOffersRequest : IRequest
  {

    private const string PROCNAME = "rex_getProductOfferingFreeOffers_sp";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductOfferingFreeOffersResponseData response = null;
      List<ResellerFreeProductItem> freeProductsList = new List<ResellerFreeProductItem>(5);
      try
      {
        var request = (ProductOfferingFreeOffersRequestData)requestData;

        string connectionString = NetConnect.LookupConnectInfo(config);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            command.Parameters.Add(new SqlParameter("@n_privateLabelID", request.ResellerId));

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
              if (reader.HasRows)
              {
                int CATEGORY_ID = reader.GetOrdinal("categoryid");
                int DESCRIPTION = reader.GetOrdinal("description");
                int ISCHECKED = reader.GetOrdinal("ischecked");
                int CHECKED_VALUE = reader.GetOrdinal("checkedValue");
                int REQUIRED_GROUP = reader.GetOrdinal("requiredGroupID");

                while (reader.Read())
                {
                  ResellerFreeProductItem item = new ResellerFreeProductItem();
                  if (!reader.IsDBNull(CATEGORY_ID))
                  {
                    item.PLCateogryId = reader.GetInt32(CATEGORY_ID);
                  }
                  if (!reader.IsDBNull(DESCRIPTION))
                  {
                    item.Description = reader.GetString(DESCRIPTION);
                  }
                  if (!reader.IsDBNull(ISCHECKED))
                  {
                    item.IsFree = reader.GetInt32(ISCHECKED) == 1;
                  }
                  if (!reader.IsDBNull(CHECKED_VALUE))
                  {
                    item.CheckedValue = reader.GetInt32(CHECKED_VALUE);
                  }
                  if (!reader.IsDBNull(REQUIRED_GROUP))
                  {
                    item.RequiredGroupId = reader.GetInt32(REQUIRED_GROUP);
                  }
                  else
                  {
                    item.RequiredGroupId =-1;
                  }
                  freeProductsList.Add(item);
                }
              }
            }
          }
        }

        response = new ProductOfferingFreeOffersResponseData(freeProductsList);

      }
      catch (AtlantisException aex)
      {
        response = new ProductOfferingFreeOffersResponseData(aex);
      }
      catch (Exception ex)
      {
        response = new ProductOfferingFreeOffersResponseData(requestData, ex);
      }

      return response;
    }

  }
}
