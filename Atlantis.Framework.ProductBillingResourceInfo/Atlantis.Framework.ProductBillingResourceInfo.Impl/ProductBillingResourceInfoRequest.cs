using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductBillingResourceInfo.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ProductBillingResourceInfo.Impl
{
  public class ProductBillingResourceInfoRequest : IRequest
  {
    private const string PROC_NAME = "mya_ProductBillingResourceInfo_sp";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      try
      {
        int numberOfRecords = 0, numberOfPages = 0;
        List<BillingResourceInfo> resourceList = new List<BillingResourceInfo>(10);

        ProductBillingResourceInfoRequestData request = oRequestData as ProductBillingResourceInfoRequestData;

        bool returnAll = request.ReturnAll == 1;

        int iResourceId = -1,
            iNameSpace = -1,
            iProfileId = -1,
            iProductDescription = -1,
            iInfo = -1,
            iBillingDate = -1,
            iOrderId = -1,
            iRenewalSku = -1,
            iIsLimited = -1,
            iPfid = -1,
            iAutoRenewFlag = -1,
            iAllowRenewals = -1,
            iRecurringPayment = -1,
            iNumberOfPeriods = -1,
            iRenewalPfid = -1,
            iProductTypeId = -1,
            iIsPastDue = -1,
            iUsageSDate = -1,
            iUsageEDate = -1,
            iExternalResourceId = -1,
            iPurchasedDuration = -1,
            iIsPrivacyPlusDomain = -1, iMostRecentRenewalOrderId = -1, iMostRecentRenewalOrderDate = -1;

        bool columnPositionChecked = false;

        string connectionString = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.NetConnectionString);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {

          IEnumerable<SqlParameter> _params = BuildSqlParametersForProc(request);
          using (SqlCommand command = new SqlCommand(PROC_NAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            foreach (SqlParameter param in _params)
            {
              command.Parameters.Add(param);
            }

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {

              if (request.ReturnAll == 0)
              {
                while (reader.Read())
                {
                  numberOfRecords = reader.GetInt32(0);
                }
                reader.NextResult();

                while (reader.Read())
                {
                  numberOfPages = reader.GetInt32(0);
                }
                reader.NextResult();
              }

              while (reader.Read())
              {

                if (!columnPositionChecked)
                {
                  iResourceId = reader.GetOrdinal(ColumnName.ResourceId);
                  iNameSpace = reader.GetOrdinal(ColumnName.Namespace);
                  iProfileId = reader.GetOrdinal(ColumnName.ProfileId);
                  iProductDescription = reader.GetOrdinal(ColumnName.ProductDescription);
                  iInfo = reader.GetOrdinal(ColumnName.Info);
                  iBillingDate = reader.GetOrdinal(ColumnName.BillingDate);
                  iOrderId = reader.GetOrdinal(ColumnName.OrderId);
                  iRenewalSku = reader.GetOrdinal(ColumnName.RenewalSKU);
                  iIsLimited = reader.GetOrdinal(ColumnName.IsLimited);

                  iAutoRenewFlag = reader.GetOrdinal(ColumnName.AutoRenewFlag);
                  iPfid = reader.GetOrdinal(ColumnName.PFID);
                  iAllowRenewals = reader.GetOrdinal(ColumnName.AllowRenewals);
                  iRecurringPayment = reader.GetOrdinal(ColumnName.RecurringPayment);
                  iNumberOfPeriods = reader.GetOrdinal(ColumnName.NumberOfPeriods);
                  iRenewalPfid = reader.GetOrdinal(ColumnName.RenewalPFID);
                  iIsPastDue = reader.GetOrdinal(ColumnName.IsPastDue);
                  iUsageSDate = reader.GetOrdinal(ColumnName.UsageStartDate);
                  iUsageEDate = reader.GetOrdinal(ColumnName.UsageEndDate);
                  iExternalResourceId = reader.GetOrdinal(ColumnName.ExternalResourceId);
                  iPurchasedDuration = reader.GetOrdinal(ColumnName.PurchasedDuration);
                  iProductTypeId = reader.GetOrdinal(ColumnName.ProductTypeId);
                  iIsPrivacyPlusDomain = reader.GetOrdinal(ColumnName.IsPrivacyPlusDomain);
                  iMostRecentRenewalOrderId = reader.GetOrdinal(ColumnName.MostRecentRenewalOrderId);
                  iMostRecentRenewalOrderDate = reader.GetOrdinal(ColumnName.MostRecentRenewalOrderDate);
                }
                columnPositionChecked = true;


                int resourceId = !reader.IsDBNull(iResourceId) ? reader.GetInt32(iResourceId) : -1;
                string nameSpace = !reader.IsDBNull(iNameSpace) ? reader.GetString(iNameSpace) : string.Empty;
                int profileId = !reader.IsDBNull(iProfileId) ? reader.GetInt32(iProfileId) : -1;
                string productDescription = !reader.IsDBNull(iProductDescription)
                                              ? reader.GetString(iProductDescription)
                                              : string.Empty;

                string info = !reader.IsDBNull(iInfo) ? reader.GetString(iInfo) : string.Empty;
                DateTime billingDate = !reader.IsDBNull(iBillingDate)
                                         ? reader.GetDateTime(iBillingDate)
                                         : DateTime.MinValue;
                string orderId = !reader.IsDBNull(iOrderId) ? reader.GetString(iOrderId) : string.Empty;
                string renewalSku = !reader.IsDBNull(iRenewalSku) ? reader.GetString(iRenewalSku) : string.Empty;

                int isLimited = !reader.IsDBNull(iIsLimited) ? reader.GetInt32(iIsLimited) : -1;

                int autoRenewFlag = reader.GetInt32(iAutoRenewFlag);
                int pfid = !reader.IsDBNull(iPfid) ? Convert.ToInt32(reader.GetDecimal(iPfid)) : -1;
                int allowRenewals = reader.GetInt32(iAllowRenewals);

                string recurringPayment = !reader.IsDBNull(iRecurringPayment)
                                            ? reader.GetString(iRecurringPayment)
                                            : string.Empty;
                int numberOfPeriods = !reader.IsDBNull(iNumberOfPeriods) ? reader.GetInt32(iNumberOfPeriods) : 0;
                int renewalPfid = !reader.IsDBNull(iRenewalPfid)
                                    ? Convert.ToInt32(reader.GetDecimal(iRenewalPfid))
                                    : -1;
                int isPastDue = reader.GetInt32(iIsPastDue);

                DateTime usageSDate = !reader.IsDBNull(iUsageSDate)
                                        ? reader.GetDateTime(iUsageSDate)
                                        : DateTime.MinValue;
                DateTime usageEDate = !reader.IsDBNull(iUsageEDate)
                                        ? reader.GetDateTime(iUsageEDate)
                                        : DateTime.MinValue;
                string externalResourceId = !reader.IsDBNull(iExternalResourceId)
                                              ? reader.GetString(iExternalResourceId)
                                              : string.Empty;
                int purchasedDuration = reader.GetInt32(iPurchasedDuration);
                int productTypeId = !reader.IsDBNull(iProductTypeId) ? reader.GetInt32(iProductTypeId) : -1;
                int isPrivacyPlusDomain = reader.GetInt32(iIsPrivacyPlusDomain);
                string mostRecentRenewalOrderId = !reader.IsDBNull(iMostRecentRenewalOrderId) ? reader.GetString(iMostRecentRenewalOrderId) : string.Empty;
                DateTime mostRecentRenewalOrderDate = !reader.IsDBNull(iMostRecentRenewalOrderDate) ? reader.GetDateTime(iMostRecentRenewalOrderDate) : DateTime.MinValue;

                BillingResourceInfo resourceInfo = new BillingResourceInfo(resourceId, nameSpace, profileId,
                                                             productDescription, info, billingDate, orderId,
                                                             renewalSku, isLimited, autoRenewFlag, pfid, allowRenewals,
                                                             recurringPayment, numberOfPeriods, renewalPfid,
                                                             isPastDue, usageSDate, usageEDate,
                                                             externalResourceId, purchasedDuration, productTypeId,
                                                             isPrivacyPlusDomain, mostRecentRenewalOrderId, mostRecentRenewalOrderDate);

                resourceList.Add(resourceInfo);

              }
            }
          }
        }

        if (request.ReturnAll == 1)
        {
          numberOfPages = 1;
          numberOfRecords = resourceList.Count;
        }

        oResponseData = new ProductBillingResourceInfoResponseData(numberOfRecords, numberOfPages, resourceList);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ProductBillingResourceInfoResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ProductBillingResourceInfoResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    private IEnumerable<SqlParameter> BuildSqlParametersForProc(ProductBillingResourceInfoRequestData request)
    {

      /*
      List<string> profileList = null,int pageNumber = 1, int rowsPerPage = 5, string sortColumn = ColumnName.Info, string sortDirection = "asc", int onlyNulls = 0, string infoFilter = null, 
      int billingDateFilter = -1, int deptFilter = -1, List<string> namespaceFilterList = null, int returnAll = 0)
      
      CREATE PROCEDURE [dbo].[mya_ProductBillingResourceInfo_sp]  
      ( @shopper_id  varchar(10),  
       @profileidlist  varchar(2000) = NULL,  
       @pageno   int = 1,  
       @rowsperpage  int = 25,  
       @sortcol  varchar(256) = 'info',  
       @sortdir  varchar(5) = 'ASC',  
       @returnAll  int = 0,  
       @onlyNulls  int = 0,  
       @infoFilter  varchar(255) = NULL,  
       @billingDateFilter int = NULL,  
       @deptFilter  int = NULL,  
       @namespaceFilterList varchar(4000) = NULL

       */

      List<SqlParameter> _paramList = new List<SqlParameter>(8);

      _paramList.Add(new SqlParameter("@shopper_id", request.ShopperID));

      if (request.ProfileList == null || request.ProfileList.Count == 0)
      {
        _paramList.Add(new SqlParameter("@profileidlist", DBNull.Value));
      }
      else
      {
        _paramList.Add(new SqlParameter("@profileidlist", String.Join(",", request.ProfileList.FindAll(x => !string.IsNullOrEmpty(x)).ToArray())));
      }

      _paramList.Add(new SqlParameter("@pageno", request.PageNumber));
      _paramList.Add(new SqlParameter("@rowsperpage", request.RowsPerPage));
      _paramList.Add(new SqlParameter("@sortcol", request.SortByColumn));
      _paramList.Add(new SqlParameter("@sortdir", request.SortDirection));
      _paramList.Add(new SqlParameter("@returnAll", request.ReturnAll));
      _paramList.Add(new SqlParameter("@onlyNulls", request.OnlyNulls));

      _paramList.Add(string.IsNullOrEmpty(request.InfoFilter)
                       ? new SqlParameter("@infoFilter", DBNull.Value)
                       : new SqlParameter("@infoFilter", request.InfoFilter));

      _paramList.Add(request.BillingDateFilter == -1
                       ? new SqlParameter("@billingDateFilter", DBNull.Value)
                       : new SqlParameter("@billingDateFilter", request.BillingDateFilter));

      _paramList.Add(request.DeptFilter == -1
                       ? new SqlParameter("@deptFilter", DBNull.Value)
                       : new SqlParameter("@deptFilter", request.DeptFilter));

      if (request.NameSpaceFilterList == null || request.NameSpaceFilterList.Count == 0)
      {
        _paramList.Add(new SqlParameter("@namespaceFilterList", DBNull.Value));
      }
      else
      {
        _paramList.Add(new SqlParameter("@namespaceFilterList", String.Join(",", request.NameSpaceFilterList.FindAll(x => !string.IsNullOrEmpty(x)).ToArray())));
      }

      return _paramList;
    }

  }
}
