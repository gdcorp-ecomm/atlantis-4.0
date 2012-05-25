using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.WhoIsSaveBlock.Interface;

namespace Atlantis.Framework.WhoIsSaveBlock.Impl
{
    public class WhoIsSaveBlockRequest : IRequest
    {
        public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
        {
            WhoIsSaveBlockResponseData responseData = null;

            try
            {
                var requestData = (WhoIsSaveBlockRequestData)oRequestData;
                var connectionString = NetConnect.LookupConnectInfo(oConfig);
                const string procName = "dbo.prx_IPMonitorStatsInsert_sp";

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand(procName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IP", requestData.IP);
                        command.Parameters.AddWithValue("@Page", requestData.Page);

                        connection.Open();
                        command.ExecuteNonQuery();

                        responseData = new WhoIsSaveBlockResponseData();
                    }
                    connection.Close();
                }
            }
            catch (AtlantisException exAtlantis)
            {
                responseData = new WhoIsSaveBlockResponseData(exAtlantis);
            }
            catch (Exception ex)
            {
                responseData = new WhoIsSaveBlockResponseData(oRequestData, ex);
            }

            return responseData;
        }
    }
}
