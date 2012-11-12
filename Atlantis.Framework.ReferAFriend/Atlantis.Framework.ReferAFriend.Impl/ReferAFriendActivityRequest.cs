using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReferAFriend.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	public class ReferAFriendActivityRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			IResponseData resp = null;
			try
			{
				var req = (ReferAFriendActivityRequestData)requestData;
				resp = PullActivityFromDB(req, config);
			}
			catch (AtlantisException atlantisEx)
			{
				resp = new ReferAFriendActivityResponseData(atlantisEx);
			}
			catch (Exception ex)
			{
				resp = new ReferAFriendActivityResponseData(new AtlantisException(requestData,
					"ReferAFriendActivityRequest::RequestHandler", "Unable to pull referral activity.", string.Empty, ex));
			}

			return resp;
		}

		private IResponseData PullActivityFromDB(ReferAFriendActivityRequestData req, ConfigElement config)
		{
			var list = new List<ReferAFriendActivityData>();
			string procName = "dbo.refer_GetShopperActivity_sp";

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

					cn.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							var data = new ReferAFriendActivityData()
							{
								OrderID = (string)dr["order_id"],
								ActivityDate = (DateTime)dr["orderDate"],
								Amount = (int)dr["total"],
								IsNewCustomer = ((int)dr["IsNewCustomer"]) == 1,
								IsPendingCredit = ((int)dr["IsPending"]) == 1,
								EffectiveCreditDate = (DateTime)dr["EffectiveDate"]
							};

							list.Add(data);
						}
					}
				}
			}

			return new ReferAFriendActivityResponseData(list);
		}
	}
}