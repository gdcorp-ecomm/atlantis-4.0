using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReferAFriend.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	public class ReferAFriendMessageTemplateRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			ReferAFriendMessageTemplateResponseData resp = null;

			try
			{
				var req = (ReferAFriendMessageTemplateRequestData)requestData;

				resp = PullMessageTemplatesFromDB(req, config);
			}
			catch (AtlantisException atlantisEx)
			{
				resp = new ReferAFriendMessageTemplateResponseData(atlantisEx);
			}
			catch (Exception ex)
			{
				resp = new ReferAFriendMessageTemplateResponseData(new AtlantisException(requestData,
					"ReferAFriendMessageTemplateRequest::RequestHandler", "Unable to pull referral message templates.", string.Empty, ex));
			}
			return resp;
		}

		private ReferAFriendMessageTemplateResponseData PullMessageTemplatesFromDB(ReferAFriendMessageTemplateRequestData req, ConfigElement config)
		{
			var dictResult = new Dictionary<string, ReferAFriendMessageTemplateData>();
			string procName = "dbo.refer_MessageTemplate_sp";

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
							var data = new ReferAFriendMessageTemplateData()
							{
								TemplateName = (string)dr["MessageType"],
								Text = (string)dr["MessageValue"]
							};

							dictResult[data.TemplateName] = data;
						}
					}
				}
			}

			return new ReferAFriendMessageTemplateResponseData(dictResult);
		}
	}
}