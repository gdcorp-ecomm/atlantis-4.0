using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAvailableProductNamespaces.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MyaAvailableProductNamespaces.Impl
{
 public class MyaAvailableProductNamespacesRequest: IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MyaAvailableProductNamespacesResponseData response = null;

      try
      {
        string connectionString = NetConnect.LookupConnectInfo(config);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand("mya_GetAvailableProductNamespaces_sp", connection))
          {
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);

            response = new MyaAvailableProductNamespacesResponseData(dt);
          }
        }
      }
      catch (AtlantisException ex)
      {
        response = new MyaAvailableProductNamespacesResponseData(ex);
      }
      catch (Exception ex)
      {
        response = new MyaAvailableProductNamespacesResponseData(ex, requestData);
      }

      return response;
    }
  }
}
