using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetProduct.Interface;
using Atlantis.Framework.MYAGetProduct.Interface.ProductHelper;
using Atlantis.Framework.MYAGetProduct.Interface.Products;
using netConnect;

namespace Atlantis.Framework.MYAGetProduct.Impl
{
  public class MYAGetProductRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MYAGetProductResponseData responseData = null;
      List<MyaProduct> myaProductLites = new List<MyaProduct>();

      try
      {
        MYAGetProductRequestData myaProductsRequestData = (MYAGetProductRequestData)requestData;
        myaProductLites = GetProducts(myaProductsRequestData, config);

        responseData = new MYAGetProductResponseData(myaProductLites);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new MYAGetProductResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new MYAGetProductResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Database Request Method
    private List<MyaProduct> GetProducts(MYAGetProductRequestData myaProductRequestData, ConfigElement config)
    {
      List<MyaProduct> myaProducts = new List<MyaProduct>();
      string procName = string.Empty;
      SqlParameter parm2 = new SqlParameter();
      SqlParameter[] parmArray;
      PagingInfo pagingInfo = new PagingInfo(myaProductRequestData.PagingInfo);

      SetProcInfo(myaProductRequestData, pagingInfo, out procName, out parmArray);

      using (SqlConnection cn = new SqlConnection(LookupConnectionString(myaProductRequestData, config)))
      {
        using (SqlCommand cmd = new SqlCommand(procName, cn))
        {
          cmd.CommandTimeout = (int)myaProductRequestData.RequestTimeout.TotalSeconds;
          cmd.CommandType = CommandType.StoredProcedure;
          if (parmArray.Length > 0)
          {
            cmd.Parameters.AddRange(parmArray);
          }
          pagingInfo.AddParameters(cmd);

          cn.Open();
          using (SqlDataReader dr = cmd.ExecuteReader())
          {
            bool hasIsFreeColumn = false;
            DataTable dt = dr.GetSchemaTable();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
              if (string.Compare(dt.Columns[i].ColumnName, "isFree", true) == 0)
              {
                hasIsFreeColumn = true;
                break;
              }
            }
            dt = null;

            //Process preliminary resultSets
            try
            {
              switch (myaProductRequestData.ProductTypeId)
              {
                case MyaProductTypeId.ExpressEmailMarketing:
                case MyaProductTypeId.MerchantAccount:
                  HandlePreliminaryResultSets(pagingInfo, dr);
                  break;
              }
            }
            catch (Exception ex)
            {
              throw new AtlantisException(myaProductRequestData, "MYAGetProductRequest::HandlePreliminaryResultSets", "Error Setting PagingInfo DataReader", ex.Message, ex);
            }

            while (dr.Read())
            {
              myaProducts.Add(PopulateObjectFromDB(dr, myaProductRequestData, hasIsFreeColumn));
            }
          }
          if (pagingInfo.HasOutputParameters)
          {
            try
            {
              pagingInfo.RetreiveOutputParameters(cmd);
            }
            catch (Exception ex)
            {
              throw new AtlantisException(myaProductRequestData, "MYAGetProduct::PagingInfo.RetreiveOutputParameters", "Error Retrieving Database Output Parameters For PagingInfo", ex.Message, ex);
            }
          }
        }
      }
      UpdateRequestDataPagingInfo(myaProductRequestData, pagingInfo);

      return myaProducts;
    }
    #endregion

    #region PagingInfo Reconcilliation
    private void UpdateRequestDataPagingInfo(MYAGetProductRequestData request, PagingInfo pagingInfo)
    {
      request.PagingInfo.NumberOfPages = pagingInfo.NumberOfPages;
      request.PagingInfo.NumberOfRecords = pagingInfo.NumberOfRecords;
    }
    #endregion

    #region Database Response Methods
    private MyaProduct PopulateObjectFromDB(IDataReader dr, MYAGetProductRequestData myaProductRequestData, bool hasIsFreeColumn)
    {
      MyaProduct mp = new MyaProduct();

      try
      {
        switch (myaProductRequestData.ProductTypeId)
        {
          case MyaProductTypeId.WebsiteTonight:
          case MyaProductTypeId.CustomWebsiteDesign:
            mp = new WebsiteTonightProduct();
            break;
          case MyaProductTypeId.SSL:
            mp = new SSLProduct();
            break;
          case MyaProductTypeId.ExpressEmailMarketing:
            mp = new EEMProduct();
            break;
          case MyaProductTypeId.SocialDomains:
            mp = new SocialDomainProduct();
            break;
          case MyaProductTypeId.CustomerManager:
            mp = new CustomerManagerProduct();
            break;
          case MyaProductTypeId.MarketPlace:
            mp = new MarketPlaceProduct();
            break;
          case MyaProductTypeId.TrafficBlazer:
            mp = new TrafficBlazerProduct();
            break;
          case MyaProductTypeId.Reseller:
            mp = new ResellerProduct();
            break;
          case MyaProductTypeId.MerchantAccount:
            mp = new MerchantAccountProduct();
            break;
          case MyaProductTypeId.DedicatedHosting:
            mp = new DedicatedHostingProduct();
            break;
          default:
            mp = new MyaProduct();
            break;
        }
        return mp.PopulateObjectFromDB(dr, myaProductRequestData, hasIsFreeColumn);
      }
      catch (IndexOutOfRangeException ex)
      {
        string className = mp.GetType().ToString();
        throw new AtlantisException(myaProductRequestData, "MYAGetProductRequest::PopulateObjectFromDB", "Database Column Does Not Exist", string.Format("ProductClass: {0} | Missing Column: {1}", className, ex.Message));
      }
      catch (Exception ex)
      {
        string className = mp.GetType().ToString();
        throw new AtlantisException(myaProductRequestData, "MYAGetProductRequest::PopulateObjectFromDB", "Error populating object from DataReader", string.Format("ProductClass: {0} | ErrorMsg: {1} | StackTrace: {2}", className, ex.Message, ex.StackTrace));
      }
    }

    private void HandlePreliminaryResultSets(PagingInfo pagingInfo, SqlDataReader dr)
    {
      if (dr.Read())
      {
        pagingInfo.NumberOfRecords = Convert.ToInt32(dr[0]);
      }

      if (dr.NextResult() && dr.Read())
      {
        pagingInfo.NumberOfPages = Convert.ToInt32(dr[0]);
      }
      dr.NextResult();
    }
    #endregion

    #region Stored Procedure Setup
    private void SetProcInfo(MYAGetProductRequestData myaProductRequestData, PagingInfo pagingInfo, out string procName, out SqlParameter[] parmArray)
    {
      procName = string.Empty;
      parmArray = new SqlParameter[0];

      switch (myaProductRequestData.ProductTypeId)
      {
        case MyaProductTypeId.BlogCast:
          SetBlogCastProcInfo(myaProductRequestData, pagingInfo, out procName, out parmArray);
          break;
        case MyaProductTypeId.WebsiteTonight:
        case MyaProductTypeId.CustomWebsiteDesign:
          SetWSTProcInfo(myaProductRequestData, out procName, out parmArray);
          break;
        case MyaProductTypeId.SSL:
          SetSSLProcInfo(myaProductRequestData, out procName, out parmArray);
          break;
        case MyaProductTypeId.ExpressEmailMarketing:
          SetEEMProcInfo(myaProductRequestData, pagingInfo, out procName, out parmArray);
          break;
        case MyaProductTypeId.TrafficBlazer:
          SetTrafficBlazerProcInfo(myaProductRequestData, pagingInfo, out procName, out parmArray);
          break;
        case MyaProductTypeId.Reseller:
          SetResellerProcInfo(myaProductRequestData, pagingInfo, out procName, out parmArray);
          break;
        case MyaProductTypeId.MerchantAccount:
          SetMerchantAccountProcInfo(myaProductRequestData, pagingInfo, out procName, out parmArray);
          break;
        case MyaProductTypeId.DedicatedHosting:
          SetDedicatedHostingProcInfo(myaProductRequestData, out procName, out parmArray);
          break;
        default:
          SetDefaultProcInfo(myaProductRequestData, MyaProductTypeId.DomainRep, out procName, out parmArray);
          break;
      }
    }

    private static void SetDedicatedHostingProcInfo(MYAGetProductRequestData myaProductRequestData, out string procName, out SqlParameter[] parmArray)
    {
      procName = "mya_GetActiveDedHostingList_sp";
      parmArray = new SqlParameter[1];
      parmArray[0] = new SqlParameter("@shopper_id", myaProductRequestData.ShopperID);
    }

    private static void SetMerchantAccountProcInfo(MYAGetProductRequestData myaProductRequestData, PagingInfo pagingInfo, out string procName, out SqlParameter[] parmArray)
    {
      pagingInfo.HasOutputParameters = false;
      pagingInfo.HasReturnAllFlag = false;
      procName = "mya_merchantAccountsByShopper_sp";
      parmArray = new SqlParameter[1];
      parmArray[0] = new SqlParameter("@shopper_id", myaProductRequestData.ShopperID);
    }

    private static void SetResellerProcInfo(MYAGetProductRequestData myaProducteRequestData, PagingInfo pagingInfo, out string procName, out SqlParameter[] parmArray)
    {
      //Set paging info attribute defaults that are specific to this proc call
      if (string.IsNullOrEmpty(pagingInfo.SortField) || pagingInfo.SortField.Equals("commonName"))
      {
        pagingInfo.SortField = "entityName";
      }

      procName = "mya_getActiveResellerList_sp";
      parmArray = new SqlParameter[1];
      parmArray[0] = new SqlParameter("@shopper_id", myaProducteRequestData.ShopperID);
    }

    private static void SetTrafficBlazerProcInfo(MYAGetProductRequestData myaProductRequestData, PagingInfo pagingInfo, out string procName, out SqlParameter[] parmArray)
    {
      //Turn off paging info for this proc call
      pagingInfo.HasPagingInfo = false;
      pagingInfo.HasOutputParameters = false;
      procName = "tba_userwebsiteStatusGetForMYA_sp";
      parmArray = new SqlParameter[1];
      parmArray[0] = new SqlParameter("@shopper_id", myaProductRequestData.ShopperID);
    }

    private static void SetEEMProcInfo(MYAGetProductRequestData myaProductRequestData, PagingInfo pagingInfo, out string procName, out SqlParameter[] parmArray)
    {
      //Set paging info attribute defaults that are specific to this proc call
      pagingInfo.HasOutputParameters = false;
      if (string.IsNullOrEmpty(pagingInfo.SortField) || pagingInfo.SortField.Equals("commonName"))
      {
        pagingInfo.SortField = "CampaignBlazerID";
      }

      procName = "mya_GetCampaignBlazerListByShopperID_sp";
      parmArray = new SqlParameter[1];
      parmArray[0] = new SqlParameter("@shopperId", myaProductRequestData.ShopperID);
    }

    private static void SetSSLProcInfo(MYAGetProductRequestData myaProductRequestData, out string procName, out SqlParameter[] parmArray)
    {
      procName = "mya_GetActiveSSLList_sp";
      parmArray = new SqlParameter[1];
      parmArray[0] = new SqlParameter("@shopper_id", myaProductRequestData.ShopperID);
    }

    private static void SetWSTProcInfo(MYAGetProductRequestData myaProductRequestData, out string procName, out SqlParameter[] parmArray)
    {
      if (myaProductRequestData.ProductTypeId == MyaProductTypeId.WebsiteTonight)
      {
        procName = "mya_getActiveWSTList_sp";
      }
      else
      {
        procName = "mya_getActiveCustomDesignList_sp";
      }
      parmArray = new SqlParameter[2];
      parmArray[0] = new SqlParameter("@shopper_id", myaProductRequestData.ShopperID);
      parmArray[1] = new SqlParameter("@product_typeID", myaProductRequestData.ProductTypeId);
    }

    private static void SetDefaultProcInfo(MYAGetProductRequestData myaProductRequestData, int DomainRep, out string procName, out SqlParameter[] parmArray)
    {
      if (myaProductRequestData.ProductTypeId == DomainRep)
      {
        myaProductRequestData.PagingInfo.SortField = "CommonName";
      }
      procName = "dbo.mya_GetActiveUnifiedList_sp";
      parmArray = new SqlParameter[2];
      parmArray[0] = new SqlParameter("@shopper_id", myaProductRequestData.ShopperID);
      parmArray[1] = new SqlParameter("@product_typeID", myaProductRequestData.ProductTypeId);
    }

    private static void SetBlogCastProcInfo(MYAGetProductRequestData myaProductRequestData, PagingInfo pagingInfo, out string procName, out SqlParameter[] parmArray)
    {
      procName = "dbo.mya_getActiveUnifiedListbyProductTypeList_sp";
      pagingInfo.SortField = "CommonName";
      string ids = string.Format("{0},{1}", MyaProductTypeId.BlogCast, MyaProductTypeId.PodCast);
      parmArray = new SqlParameter[2];
      parmArray[0] = new SqlParameter("@shopper_id", myaProductRequestData.ShopperID);
      parmArray[1] = new SqlParameter("@product_typeIDList", ids);
    }
    #endregion

    #region Nimitz
    private string LookupConnectionString(MYAGetProductRequestData myaProductRequestData, ConfigElement config)
    {
      string result;

      switch (myaProductRequestData.ProductTypeId)
      {
        case MyaProductTypeId.TrafficBlazer:
          LookupConnectionString(config, config.GetConfigValue("TB_DataSourceName"), out result);
          break;
        case MyaProductTypeId.MerchantAccount:
          LookupConnectionString(config, config.GetConfigValue("MA_DataSourceName"), out result);
          break;
        default:
          LookupConnectionString(config, config.GetConfigValue("GD_DataSourceName"), out result);
          break;
      }
      return result;
    }

    private static void LookupConnectionString(ConfigElement config, string dsn, out string result)
    {
      result = string.Empty;

      netConnect.Info nc = new netConnect.Info();
      result = nc.Get(dsn
        , config.GetConfigValue("ApplicationName")
        , config.GetConfigValue("CertificateName")
        , ConnectTypeEnum.CONNECT_TYPE_NET);

      if (string.IsNullOrEmpty(result) || result.Length <= 2)
      {
        throw new Exception("Invalid Connection String");
      }
    }
    #endregion
  }
}
