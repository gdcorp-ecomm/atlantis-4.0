using System;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaAvailableProductNamespaces.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.MyaAvailableProductNamespaces.Impl
{
  public class MyaAvailableProductNamespacesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      MyaAvailableProductNamespacesResponseData response;

      try
      {
        var data = new DataTable();
        string connectionString = NetConnect.LookupConnectInfo(config);
        using (var connection = new SqlConnection(connectionString))
        {
          using (var command = new SqlCommand("mya_GetAvailableProductNamespaces_sp", connection))
          {
            var da = new SqlDataAdapter(command);
            da.Fill(data);
          }
        }

        response = new MyaAvailableProductNamespacesResponseData(data);
        data.Dispose();
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
