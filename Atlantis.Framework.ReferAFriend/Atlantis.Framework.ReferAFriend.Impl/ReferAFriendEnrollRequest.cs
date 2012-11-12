using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReferAFriend.Interface;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	public class ReferAFriendEnrollRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			ReferAFriendEnrollResponseData resp = null;

			try
			{
				var req = (ReferAFriendEnrollRequestData)requestData;

				resp = EnrollInProgram(req, config);
			}
			catch (AtlantisException atlantisEx)
			{
				resp = new ReferAFriendEnrollResponseData(atlantisEx);
			}
			catch (Exception ex)
			{
				resp = new ReferAFriendEnrollResponseData(new AtlantisException(requestData,
					"ReferAFriendEnrollRequest::RequestHandler", "Unable to enroll shopper in referral program.", string.Empty, ex));
			}

			return resp;
		}

		private ReferAFriendEnrollResponseData EnrollInProgram(ReferAFriendEnrollRequestData req, ConfigElement config)
		{
			const string PROC_NAME = "dbo.refer_shopperEnroll_sp";

			string connectionString = NetConnect.LookupConnectInfo(config);
			using (var cn = new SqlConnection(connectionString))
			{
				using (var cmd = new SqlCommand(PROC_NAME, cn))
				{
					cmd.CommandTimeout = Util.GetRequestTimeout(req, config);
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.Add(new SqlParameter("@ISC", req.ItemSourceCode));
					cmd.Parameters.Add(new SqlParameter("@shopper_id", req.ShopperID));

					var pTaken = cmd.Parameters.Add(new SqlParameter("@taken", SqlDbType.Bit)); pTaken.Direction = ParameterDirection.Output;
					var pBlacklist = cmd.Parameters.Add(new SqlParameter("@blacklist", SqlDbType.Bit)); pBlacklist.Direction = ParameterDirection.Output;
					var pReferShopperID = cmd.Parameters.Add(new SqlParameter("@refer_shopperID", SqlDbType.BigInt)); pReferShopperID.Direction = ParameterDirection.Output;

					cn.Open();

					cmd.ExecuteNonQuery();

					var isTaken = (bool)pTaken.Value;
					var isBlacklisted = (bool)pBlacklist.Value;

					return new ReferAFriendEnrollResponseData(req.ItemSourceCode, req.ShopperID, (!isTaken && !isBlacklisted),
						isTaken, isBlacklisted, (long)pReferShopperID.Value);
				}
			}
		}
	}
}