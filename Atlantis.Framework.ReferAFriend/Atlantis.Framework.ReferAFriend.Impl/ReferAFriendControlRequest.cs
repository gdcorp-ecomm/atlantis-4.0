using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReferAFriend.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	public class ReferAFriendControlRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			IResponseData resp = null;
			var dictResult = new Dictionary<string, ReferAFriendControlData>();

			try
			{
				var req = (ReferAFriendControlRequestData)requestData;
				resp = PullControlFromDB(req, config);
			}
			catch (AtlantisException atlantisEx)
			{
				resp = new ReferAFriendControlResponseData(atlantisEx);
			}
			catch (Exception ex)
			{
				resp = new ReferAFriendControlResponseData(new AtlantisException(requestData,
					"ReferAFriendControlRequest::RequestHandler", "Unable to pull referral control information.", string.Empty, ex));
			}

			return resp;
		}

		private IResponseData PullControlFromDB(ReferAFriendControlRequestData req, ConfigElement config)
		{
			var dictResult = new Dictionary<string, ReferAFriendControlData>();
			string procName = "dbo.refer_Control_sp";

			string connectionString = NetConnect.LookupConnectInfo(config);
			using (var cn = new SqlConnection(connectionString))
			{
				using (var cmd = new SqlCommand(procName, cn))
				{
					cmd.CommandTimeout = Util.GetRequestTimeout(req, config);
					cmd.CommandType = CommandType.StoredProcedure;

					const int APP_USER_ID = -1; const int ACTION_SELECT_ALL = 8;
					cmd.Parameters.Add(new SqlParameter("@n_AppUserID", APP_USER_ID));
					cmd.Parameters.Add(new SqlParameter("@n_Action", ACTION_SELECT_ALL));

					cn.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							var data = new ReferAFriendControlData()
							{
								Key = Convert.ToString(dr["ControlType"]),
								Value = Convert.ToString(dr["ControlValue"])
							};

							dictResult[data.Key] = data;
						}
					}
				}
			}

			return new ReferAFriendControlResponseData(dictResult);
		}
	}
}