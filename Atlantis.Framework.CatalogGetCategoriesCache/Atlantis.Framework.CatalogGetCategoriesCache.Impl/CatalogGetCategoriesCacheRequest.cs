using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.CatalogGetCategoriesCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.CatalogGetCategoriesCache.Impl
{
  public class CatalogGetCategoriesCacheRequest : IRequest
  {
    private const string _PROCNAME = "crm_ProductCatalogGetCategoriesCache_sp";
    private const string _PARAM_PRIVATE_LABEL_ID = "n_privatelabelID";
    private const string _PARAM_VERSION = "n_versionID";

    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement oConfig)
    {
      IResponseData response = null;

      try
      {
        CatalogGetCategoriesCacheRequestData request = (CatalogGetCategoriesCacheRequestData)requestData;
        DataTable resultTable = null;
        string connectionString = NetConnect.LookupConnectInfo(oConfig);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand(_PROCNAME, connection))
          {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter(_PARAM_PRIVATE_LABEL_ID, request.PrivateLabelId));
            command.Parameters.Add(new SqlParameter(_PARAM_VERSION, request.VersionNumber));
            command.CommandTimeout = (int)Math.Truncate(request.RequestTimeout.TotalSeconds);

            DataSet dataSet = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
              adapter.Fill(dataSet);
            }

            if (dataSet.Tables.Count > 1)
            {
              resultTable = dataSet.Tables[1];
            }
          }
        }

        response = new CatalogGetCategoriesCacheResponseData(requestData, resultTable);
      }
      catch (Exception ex)
      {
        response = new CatalogGetCategoriesCacheResponseData(requestData, ex);
      }

      return response;
    }

    #endregion IRequest Members
  }
}
