using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.WhoIsGetBlock.Interface;



namespace Atlantis.Framework.WhoIsGetBlock.Impl 
{
    public class WhoIsGetBlockRequest : IRequest
    {
        public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
        {
            WhoIsGetBlockResponseData responseData = null;

            try
            {
                var requestData = (WhoIsGetBlockRequestData)oRequestData;
                var connectionString = NetConnect.LookupConnectInfo(oConfig);
                const string procName = "dbo.prx_IPMonitorCheck_sp";

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand(procName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                         // populate search criteria
                        command.Parameters.AddWithValue("@IP", requestData.IP);
                        command.Parameters.AddWithValue("@timeSpan", requestData.TimeSpan);

                        var blockCount = new SqlParameter("RETURN_VALUE", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue};
                        command.Parameters.Add(blockCount);
                        connection.Open();
                        command.ExecuteNonQuery();

                        responseData = new WhoIsGetBlockResponseData((int)blockCount.Value);
                    }
                    connection.Close();
                }
            }
            catch (AtlantisException exAtlantis)
            {
                responseData = new WhoIsGetBlockResponseData(exAtlantis); 
            }
            catch (Exception ex)
            {
                responseData = new WhoIsGetBlockResponseData(oRequestData, ex);
            }

            return responseData;
        }

    }
}
