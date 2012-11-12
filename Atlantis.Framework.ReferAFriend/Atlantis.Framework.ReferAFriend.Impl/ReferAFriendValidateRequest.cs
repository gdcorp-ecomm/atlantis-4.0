using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReferAFriend.Interface;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	public class ReferAFriendValidateRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			ReferAFriendValidateResponseData resp = null;

			try
			{
				var req = (ReferAFriendValidateRequestData)requestData;

				resp = CheckIfSourceCodeIsValid(req, config);
			}
			catch (AtlantisException atlantisEx)
			{
				resp = new ReferAFriendValidateResponseData(atlantisEx);
			}
			catch (Exception ex)
			{
				resp = new ReferAFriendValidateResponseData(new AtlantisException(requestData,
					"ReferAFriendValidateRequest::RequestHandler", "Unable to validate referral code.", string.Empty, ex));
			}

			return resp;
		}

		private ReferAFriendValidateResponseData CheckIfSourceCodeIsValid(ReferAFriendValidateRequestData req, ConfigElement config)
		{
			string procName = "dbo.refer_GetIsISCAvailable_sp";

			string connectionString = NetConnect.LookupConnectInfo(config);
			using (var cn = new SqlConnection(connectionString))
			{
				using (var cmd = new SqlCommand(procName, cn))
				{
					cmd.CommandTimeout = Util.GetRequestTimeout(req, config);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(new SqlParameter("@ItemSourceCode", req.ItemSourceCode));
					cn.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
						{
							var isValid = (!Convert.ToBoolean(dr["Taken"]) && !Convert.ToBoolean(dr["Blacklist"]));
							return new ReferAFriendValidateResponseData(req.ItemSourceCode, isValid);
						}
					}
				}
			}

			throw new AtlantisException(req, "ReferAFriendValidateRequest::CheckIfSourceCodeIsValid", "Failed to validate referral code.", string.Empty);
		}
	}
}