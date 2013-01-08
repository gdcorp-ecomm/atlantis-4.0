using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperFirstOrderGet.Interface;

namespace Atlantis.Framework.ShopperFirstOrderGet.Impl
{
  public class ShopperFirstOrderGetRequest : IRequest
  {
    private const string _PROCNAMESCHEDULEGET = "gdshop_shopperAffiliateIsFirstOrder_sp";
    private const string _SHOPPERIDPARAM = "ShopperId";
    private const string _ORDERIDPARAM = "OrderID";
    private const string _ISFIRSTORDERPARAM = "IsFirstOrder";
    
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ShopperFirstOrderGetResponseData responseData = null;
       bool IsCustomerNew = false;
       var request = (ShopperFirstOrderGetRequestData)requestData;

       try
       {
         int orderId;
         if (Int32.TryParse(request.OrderID, out orderId))
         {

           string connectionString = Nimitz.NetConnect.LookupConnectInfo(config);
           using (var connection = new SqlConnection(connectionString))
           {
             using (var command = new SqlCommand(_PROCNAMESCHEDULEGET, connection))
             {
               command.CommandType = CommandType.StoredProcedure;
               command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
               command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
               command.Parameters.Add(new SqlParameter(_ORDERIDPARAM, request.OrderID));
               var firstOrderParam = new SqlParameter(_ISFIRSTORDERPARAM, SqlDbType.Bit);
               firstOrderParam.Direction = ParameterDirection.Output;
               command.Parameters.Add(firstOrderParam);

               connection.Open();

               command.ExecuteNonQuery();
               IsCustomerNew = (bool)firstOrderParam.Value;

               responseData = new ShopperFirstOrderGetResponseData(IsCustomerNew);
             }
           }
         }
         else
         {
           responseData = new ShopperFirstOrderGetResponseData(false);
         }
       }
       catch (AtlantisException exAtlantis)
       {
         responseData = new ShopperFirstOrderGetResponseData(exAtlantis);
       }
       catch (Exception ex)
       {
         responseData = new ShopperFirstOrderGetResponseData(request, ex);
       }

      return responseData;
    }    

  }
}
