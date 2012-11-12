using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReferAFriend.Interface;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	public class ReferAFriendActivitySummaryRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			IResponseData resp = null;
			try
			{
				var req = (ReferAFriendActivitySummaryRequestData)requestData;
				resp = PullActivitySummaryFromDB(req, config);
			}
			catch (AtlantisException atlantisEx)
			{
				resp = new ReferAFriendActivitySummaryResponseData(atlantisEx);
			}
			catch (Exception ex)
			{
				resp = new ReferAFriendActivitySummaryResponseData(new AtlantisException(requestData,
					"ReferAFriendActivitySummaryRequest::RequestHandler", "Unable to pull referral activity summary.", string.Empty, ex));
			}

			return resp;
		}

		private IResponseData PullActivitySummaryFromDB(ReferAFriendActivitySummaryRequestData req, ConfigElement config)
		{
			string procName = "dbo.refer_GetShopperActivitySummary_sp";

			string connectionString = NetConnect.LookupConnectInfo(config);
			using (var cn = new SqlConnection(connectionString))
			{
				using (var cmd = new SqlCommand(procName, cn))
				{
					cmd.CommandTimeout = Util.GetRequestTimeout(req, config);
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.Add(new SqlParameter("@shopper_id", req.ShopperID));
					cmd.Parameters.Add(new SqlParameter("@startdate", req.StartDate));
					cmd.Parameters.Add(new SqlParameter("@enddate", req.EndDate));
					cmd.Parameters.Add(new SqlParameter("@ItemSourceCode", req.SourceCode));

					var pReferralCount = cmd.Parameters.Add(new SqlParameter("@ReferralsThisWeek", DbType.Int32)); pReferralCount.Direction = ParameterDirection.Output;
					var pCreditsEarned = cmd.Parameters.Add(new SqlParameter("@CreditEarnThisWeek", DbType.Int32)); pCreditsEarned.Direction = ParameterDirection.Output;

					cn.Open();
					cmd.ExecuteNonQuery();

					return new ReferAFriendActivitySummaryResponseData((int)pReferralCount.Value, (int)pCreditsEarned.Value);
				}
			}
		}
	}
}