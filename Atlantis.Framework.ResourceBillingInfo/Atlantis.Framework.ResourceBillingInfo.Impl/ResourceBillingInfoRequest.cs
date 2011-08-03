using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Atlantis.Framework.Interface;
using Atlantis.Framework.ResourceBillingInfo.Interface;

namespace Atlantis.Framework.ResourceBillingInfo.Impl
{
  public class ResourceBillingInfoRequest : IRequest
  {
    private const string PROC_NAME = "dbo.gdshop_getBillingInfo_sp";
    private const string SHOPPER_ID_PARAM = "@s_shopperId";
    private const string RESOURCE_ID_PARAM = "@n_resourceId";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ResourceBillingInfoResponseData responseData;

      try
      {
        var request = (ResourceBillingInfoRequestData)requestData;
        IList<GdshopResourceBillingInfo> resourceBillingInfos;

        GetResourceBillingInfo(request, config, out resourceBillingInfos);

        responseData = new ResourceBillingInfoResponseData(resourceBillingInfos);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new ResourceBillingInfoResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new ResourceBillingInfoResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Private Helpers

    private static void GetResourceBillingInfo(ResourceBillingInfoRequestData request, ConfigElement config, out IList<GdshopResourceBillingInfo> resourceBillingInfos)
    {
      resourceBillingInfos = new List<GdshopResourceBillingInfo>();

      using (var cn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
      {
        using (var cmd = new SqlCommand(PROC_NAME, cn))
        {
          cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          if (request.BillingResourceId.HasValue)
          {
            cmd.Parameters.Add(new SqlParameter(RESOURCE_ID_PARAM, request.BillingResourceId.Value));
          }
          else
          {
            cmd.Parameters.Add(new SqlParameter(SHOPPER_ID_PARAM, request.ShopperID));
          }
          cn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader())
          {
            if (dr != null && dr.HasRows)
            {
              while (dr.Read())
              {
                GdshopResourceBillingInfo resourceBillingInfo = GetResourceBillingInfoFromDB(dr);
                if (resourceBillingInfo != null)
                {
                  resourceBillingInfos.Add(resourceBillingInfo);
                }
              }
            }
          }
        }
        cn.Close();
      }
    }

    private static GdshopResourceBillingInfo GetResourceBillingInfoFromDB(IDataReader idr)
    {
      GdshopResourceBillingInfo rbi = null;

      if (idr.FieldCount > 0)
      {
        IDictionary<string, object> billingPropertiesDictionary = new Dictionary<string, object>();

        for (int i = 0; i < idr.FieldCount; i++)
        {
          if (!billingPropertiesDictionary.ContainsKey(idr.GetName(i)))
          {
            billingPropertiesDictionary.Add(idr.GetName(i), idr.GetValue(i));
          }
        }
        rbi = new GdshopResourceBillingInfo(billingPropertiesDictionary);
      }
      return rbi;
    }
    #endregion
  }
}
