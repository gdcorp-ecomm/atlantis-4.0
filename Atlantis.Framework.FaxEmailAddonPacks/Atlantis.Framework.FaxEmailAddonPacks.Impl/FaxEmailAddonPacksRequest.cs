using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.FaxEmailAddonPacks.Interface;
using Atlantis.Framework.FaxEmailAddonPacks.Interface.Types;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FaxEmailAddonPacks.Impl
{
  public class FaxEmailAddonPacksRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var request = (FaxEmailAddonPacksRequestData) requestData;
      FaxEmailAddonPacksResponseData response;

      try
      {
        var addonPacks =  new List<FaxEmailAddonPack>();
        using (var conn = new SqlConnection(Nimitz.NetConnect.LookupConnectInfo(config)))
        {
          using (var cmd = new SqlCommand("dbo.mya_getFaxEmailMinutePacks_sp", conn))
          {
            cmd.CommandTimeout = (int)request.RequestTimeout.TotalSeconds;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@resource_id", request.FteResourceId));

            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
              addonPacks.AddRange(BuildAddonPackList(reader));
            }
          }
        }

        response = new FaxEmailAddonPacksResponseData(addonPacks);
      }
      catch (AtlantisException atlEx)
      {
        response = new FaxEmailAddonPacksResponseData(atlEx);
      }
      catch (Exception ex)
      {
        string data = string.Format("FteResourceId={0}", request.FteResourceId);
        var atlEx = new AtlantisException(requestData, "FaxEmailAddonPackRequest.RequestHandler", ex.Message, data, ex);
        response = new FaxEmailAddonPacksResponseData(atlEx);
      }

      return response;
    }

    private static IEnumerable<FaxEmailAddonPack> BuildAddonPackList(SqlDataReader reader)
    {
      while (reader.Read())
      {
        yield return
          new FaxEmailAddonPack(Convert.ToInt32(reader["resource_id"]),
                                DateTime.Parse(reader["expiration_date"].ToString()),
                                new Dictionary<string, string>
                                  {
                                    {FaxEmailAddonPack.Minutes, reader["file_size"].ToString()},
                                    {FaxEmailAddonPack.Pages, reader["file_size"].ToString()}
                                  });
      }
    }
  }
}
