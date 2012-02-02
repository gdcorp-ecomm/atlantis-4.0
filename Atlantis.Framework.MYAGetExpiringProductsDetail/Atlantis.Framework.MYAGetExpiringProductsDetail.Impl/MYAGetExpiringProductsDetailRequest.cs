using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetExpiringProductsDetail.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MYAGetExpiringProductsDetail.Impl
{
  public class MYAGetExpiringProductsDetailRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      DataSet ds = null;
      int totalRecords = 0, totalPages = 0;
      List<RenewingProductObject> productList = new List<RenewingProductObject>();
      try
      {
        MYAGetExpiringProductsDetailRequestData request = (MYAGetExpiringProductsDetailRequestData)oRequestData;

        string connectionString = LookupConnectionString(request, oConfig);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_SHOPPERIDPARAM, request.ShopperID));
            command.Parameters.Add(new SqlParameter(_DAYSPARAM, request.Days));
            command.Parameters.Add(new SqlParameter(_PAGENOPARAM, request.PageNumber));
            command.Parameters.Add(new SqlParameter(_ROWSPERPAGEPARAM, request.RowsPerPage));
            command.Parameters.Add(new SqlParameter(_SORTXMLPARAM, request.SortXML));
            command.Parameters.Add(new SqlParameter(_RETURNALLFLAGPARAM, request.ReturnAll));
            command.Parameters.Add(new SqlParameter(_SYNCABLEONLYPARAM, request.SyncableOnly));
            command.Parameters.Add(new SqlParameter(_ISCDATEPARAM, request.IscDate));
            command.Parameters.Add(new SqlParameter(_TOTALRECORDSPARAM, SqlDbType.Int)).Direction = ParameterDirection.Output;
            command.Parameters.Add(new SqlParameter(_TOTALPAGESPARAM, SqlDbType.Int)).Direction = ParameterDirection.Output;
            if (!String.IsNullOrEmpty(request.ProductTypeIdList))
            {
              command.Parameters.Add(new SqlParameter(_PRODUCTTYPEIDLIST, request.ProductTypeIdList));
            }
            command.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;

            connection.Open();
            ds = new DataSet(Guid.NewGuid().ToString());
            SqlDataAdapter adp = new SqlDataAdapter(command);
            adp.Fill(ds);
            totalRecords = (int)command.Parameters[_TOTALRECORDSPARAM].Value;
            totalPages = (int)command.Parameters[_TOTALPAGESPARAM].Value;

            productList = GetObjectListFromDataset(ds);
          }
        }

        oResponseData = new MYAGetExpiringProductsDetailResponseData(ds, productList, totalRecords, totalPages);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new MYAGetExpiringProductsDetailResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new MYAGetExpiringProductsDetailResponseData(ds, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

    #region Private Methods

    private const string _PROCNAME = "mya_getExpiringProductsDetailGet_sp";
    private const string _SHOPPERIDPARAM = "shopper_id";
    private const string _DAYSPARAM = "days";
    private const string _PAGENOPARAM = "pageno";
    private const string _ROWSPERPAGEPARAM = "rowsperpage";
    private const string _SORTXMLPARAM = "sortXML";
    private const string _RETURNALLFLAGPARAM = "returnAllFlag";
    private const string _ALLRECORDSPARAM = "allrecords";
    private const string _SYNCABLEONLYPARAM = "syncAbleOnly";
    private const string _ISCDATEPARAM = "iscDate";
    private const string _TOTALRECORDSPARAM = "totalrecords";
    private const string _TOTALPAGESPARAM = "totalpages";
    private const string _PRODUCTTYPEIDLIST = "product_typeIDList";

    private string LookupConnectionString(MYAGetExpiringProductsDetailRequestData request, ConfigElement config)
    {
      string dataSource = config.GetConfigValue("DataSourceName");
      string applicationName = config.GetConfigValue("ApplicationName");
      string certificateName = config.GetConfigValue("CertificateName");

      string result = string.Empty;
      if (!string.IsNullOrEmpty(dataSource) && !string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(certificateName))
      {
        result = NetConnect.LookupConnectInfo(dataSource, certificateName, applicationName, "MYAGetExpiringProductsDetailRequest.LookupConnectionString", ConnectLookupType.NetConnectionString);
      }

      //when an error occurs a ';' is returned not a valid connection string or empty
      if (result.Length <= 1)
      {
        throw new AtlantisException(request, "LookupConnectionString",
                "Database connection string lookup failed", "No ConnectionFound For:"
                + dataSource + ":"
                + applicationName
                + ":" + certificateName);
      }

      return result;
    }

    private List<RenewingProductObject> GetObjectListFromDataset(DataSet ds)
    {      
      List<RenewingProductObject> productList = new List<RenewingProductObject>();
      foreach (DataRow row in ds.Tables[0].Rows)
      {
        RenewingProductObject product = new RenewingProductObject();
        if ((row["id"] as DBNull) == null)
        {
          product.Id = (int)row["id"];
        }
        else
        {
          product.Id = null;
        }

        if ((row["autoRenewFlag"] as DBNull) == null)
        {
          product.AutoRenewFlag = (int)row["autoRenewFlag"] == 0 ? false : true;
        }
        else
        {
          product.AutoRenewFlag = false;
        }

        if ((row["billing_attempt"] as DBNull) == null)
        {
          product.BillingAttempt = (int)row["billing_attempt"];
        }
        else
        {
          product.BillingAttempt = null;
        }
              

        if ((row["description"] as DBNull) == null)
        {
          product.Description = (string)row["description"];
        }
        else
        {
          product.Description = null;
        }
        
        if ((row["displayimageflag"] as DBNull) == null)
        {
          product.DisplayImageFlag = (bool)row["displayimageflag"];
        }
        else
        {
          product.DisplayImageFlag = null;
        }

        if ((row["domainid"] as DBNull) == null)
        {
          product.DomainID = (int)row["domainid"];
        }
        else
        {
          product.DomainID = null;
        }

        if ((row["domainname"] as DBNull) == null)
        {
          product.CommonName = (string)row["domainname"];
        }
        else
        {
          product.CommonName = null;
        }        

        if ((row["dontsync"] as DBNull) == null)
        {
          product.DontSync = (byte)row["dontsync"] == 0 ? false : true;
        }
        else
        {
          product.DontSync = null;
        }

        if ((row["expiration_date"] as DBNull) == null)
        {
          product.AccountExpirationDate = (DateTime)row["expiration_date"];
        }
        else
        {
          product.AccountExpirationDate = null;
        }

        if ((row["gdshop_product_typeID"] as DBNull) == null)
        {
          product.ProductTypeID = (int)row["gdshop_product_typeID"];
        }
        else
        {
          product.ProductTypeID = null;
        }

        if ((row["hasaddon"] as DBNull) == null)
        {
          product.HasAddon = (bool)row["hasaddon"];
        }
        else
        {
          product.HasAddon = null;
        }

        if ((row["isHosting"] as DBNull) == null)
        {
          product.IsHostingProduct = (int)row["isHosting"] == 0 ? false : true;
        }
        else
        {
          product.IsHostingProduct = false;
        }

        if ((row["isPastDue"] as DBNull) == null)
        {
          product.IsPastDue = (int)row["isPastDue"] == 0 ? false : true;
        }
        else
        {
          product.IsPastDue = false;
        }

        if ((row["isrenewalPriceLocked"] as DBNull) == null)
        {
          product.IsRenewalPriceLocked = (byte)row["isrenewalPriceLocked"] == 0 ? false : true;
        }
        else
        {
          product.IsRenewalPriceLocked = false;
        }

        if ((row["namespace"] as DBNull) == null)
        {
          product.Namespace = (string)row["namespace"];
        }
        else
        {
          product.Namespace = null;
        }

        if ((row["originalListPrice"] as DBNull) == null)
        {
          product.OriginalListPrice = (int)row["originalListPrice"];
        }
        else
        {
          product.OriginalListPrice = null;
        }

        if ((row["pf_id"] as DBNull) == null)
        {
          int pf_id;
          if (Int32.TryParse(row["pf_id"].ToString(), out pf_id))
          {
            product.PFID = pf_id;
          }
          else
          {
            product.PFID = null;
          }
        }
        else
        {
          product.PFID = null;
        }

        if ((row["recurring_payment"] as DBNull) == null)
        {
          product.RecurringPayment = (string)row["recurring_payment"];
        }
        else
        {
          product.RecurringPayment = null;
        }

        if ((row["unified_renewal_pf_id"] as DBNull) == null)
        {
          product.UnifiedRenewalProductId = (int)row["unified_renewal_pf_id"];
        }
        else
        {
          product.UnifiedRenewalProductId = null;
        }     

        if ((row["resource_id"] as DBNull) == null)
        {
          product.BillingResourceId = (int)row["resource_id"];
        }
        else
        {
          product.BillingResourceId = null;
        }               

        if ((row["unified_productID"] as DBNull) == null)
        {
          product.UnifiedProductID = (int)row["unified_productID"];
        }
        else
        {
          product.UnifiedProductID = null;
        }

        if ((row["externalResourceID"] as DBNull) == null)
        {
          product.ExternalResourceId = (string)row["externalResourceID"];
        }
        else
        {
          product.ExternalResourceId = null;
        }

        
        

        productList.Add(product);
      }
      return productList;
    }

    #endregion
  }
}
