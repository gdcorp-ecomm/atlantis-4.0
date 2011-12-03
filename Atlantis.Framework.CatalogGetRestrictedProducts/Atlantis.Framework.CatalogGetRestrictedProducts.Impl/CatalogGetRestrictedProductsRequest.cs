using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.CatalogGetRestrictedProducts.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.CatalogGetRestrictedProducts.Impl
{
  public class CatalogGetRestrictedProductsRequest : IRequest
  {

    private const string _PROCNAME = "crm_ProductCatalogGetRestrictedProducts_sp";
    private const string _PARAM_PRIVATE_LABEL_ID = "n_privatelabelID";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement oConfig)
    {
      IResponseData response = null;

      try
      {
        CatalogGetRestrictedProductsRequestData request = (CatalogGetRestrictedProductsRequestData)requestData;
        DataTable resultTable = null;
        string connectionString = NetConnect.LookupConnectInfo(oConfig);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_PARAM_PRIVATE_LABEL_ID, request.PrivateLabelId));
            command.CommandTimeout = (int)Math.Truncate(request.RequestTimeout.TotalSeconds);

            DataSet dataSet = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
              adapter.Fill(dataSet);
            }

            if (dataSet.Tables.Count > 0)
            {
              resultTable = dataSet.Tables[0];
            }
          }
        }

        response = new CatalogGetRestrictedProductsResponseData(requestData,resultTable);
      }
      catch (Exception ex)
      {
        response = new CatalogGetRestrictedProductsResponseData(requestData, ex);
      }

      return response;
    }

    #endregion IRequest Members
   
  }
}
