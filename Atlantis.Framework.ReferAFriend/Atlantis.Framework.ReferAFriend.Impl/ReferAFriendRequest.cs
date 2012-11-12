using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReferAFriend.Interface;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	public class ReferAFriendRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			ReferAFriendResponseData resp = null;

			try
			{
				var req = (ReferAFriendRequestData)requestData;

				resp = LookupReferral(req, config);
			}
			catch (AtlantisException atlantisEx)
			{
				resp = new ReferAFriendResponseData(atlantisEx);
			}
			catch (Exception ex)
			{
				resp = new ReferAFriendResponseData(new AtlantisException(requestData,
					"ReferAFriendRequest::RequestHandler", "Unable to pull referral shopper.", string.Empty, ex));
			}

			return resp;
		}

		private ReferAFriendResponseData LookupReferral(ReferAFriendRequestData req, ConfigElement config)
		{
			var shopper = LookupReferralShopper(req, config);

			if (shopper.Exists)
			{
				return LookupReferralISC(req, shopper, config);
			}
			else
			{
				return new ReferAFriendResponseData(req.ShopperID, 0, 0, false, false, string.Empty, DateTime.MinValue);
			}
		}

		private InternalReferAFriendShopper LookupReferralShopper(ReferAFriendRequestData req, ConfigElement config)
		{
			string procName = "dbo.refer_shopper_sp";

			var referControl = (ReferAFriendControlResponseData)DataCache.DataCache.GetProcessRequest(
				new ReferAFriendControlRequestData(req.ShopperID, req.SourceURL, req.OrderID, req.Pathway, req.PageCount),
				AtlantisConfig.ReferAFriendControlRequestTypeID);

			if (!referControl.IsSuccess)
			{
				throw new AtlantisException(req, "ReferAFriendRequest::LookupReferralShopper", "Unable to pull control data.", string.Empty);
			}

			var activeStatus = referControl.Get<int>("activeStatusID", 1);
			var blockedStatus = referControl.Get<int>("blockedStatusID", 3);

			string connectionString = NetConnect.LookupConnectInfo(config);
			using (var cn = new SqlConnection(connectionString))
			{
				using (var cmd = new SqlCommand(procName, cn))
				{
					cmd.CommandTimeout = Util.GetRequestTimeout(req, config);
					cmd.CommandType = CommandType.StoredProcedure;

					const int ACTION_SELECT_FULL = 8;
					const int APPUSERID_SYSTEM = -1;
					cmd.Parameters.Add(new SqlParameter("@n_AppUserID", APPUSERID_SYSTEM));
					cmd.Parameters.Add(new SqlParameter("@n_Action", ACTION_SELECT_FULL));
					cmd.Parameters.Add(new SqlParameter("@vc_Shopper_ID", req.ShopperID));

					cn.Open();

					var shopper = new InternalReferAFriendShopper();

					using (var dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							shopper.Exists = true;
							shopper.ReferShopperStatusID = Convert.ToInt32(dr["refer_shopperStatusID"]);
							shopper.ReferShopperID = Convert.ToInt64(dr["refer_shopperID"]);

							// if the shopper is currently active or blocked, we want that one over any inactive records
							if (shopper.ReferShopperStatusID == activeStatus || shopper.ReferShopperStatusID == blockedStatus)
							{
								shopper.IsQualified = (shopper.ReferShopperStatusID != blockedStatus);
								break;
							}
						}
					}

					return shopper;
				}
			}
		}

		private ReferAFriendResponseData LookupReferralISC(ReferAFriendRequestData req, InternalReferAFriendShopper shopper, ConfigElement config)
		{
			string procName = "dbo.refer_ShopperMTMISC_sp";

			string connectionString = NetConnect.LookupConnectInfo(config);
			using (var cn = new SqlConnection(connectionString))
			{
				using (var cmd = new SqlCommand(procName, cn))
				{
					cmd.CommandTimeout = (int)req.RequestTimeout.TotalSeconds;
					cmd.CommandType = CommandType.StoredProcedure;

					const int ACTION_SELECT_FULL = 8;
					const int APPUSERID_SYSTEM = -1;
					cmd.Parameters.Add(new SqlParameter("@n_AppUserID", APPUSERID_SYSTEM));
					cmd.Parameters.Add(new SqlParameter("@n_Action", ACTION_SELECT_FULL));
					cmd.Parameters.Add(new SqlParameter("@n_refer_shopperID", shopper.ReferShopperID));

					cn.Open();

					var isAwardable = false;
					string itemSourceCode = null;
					DateTime dateEnrolled = DateTime.MinValue;

					using (var dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							var isActive = (bool)dr["IsActive"];
							if (isActive)
							{
								isAwardable = (bool)dr["IsInStoreCreditAwardable"];
								itemSourceCode = (string)dr["ItemSourceCode"];
								dateEnrolled = (DateTime)dr["CreateDate"];

								// break on first awardable referral code
								if (isAwardable) break;
							}
						}
					}

					return new ReferAFriendResponseData(req.ShopperID, shopper.ReferShopperID, shopper.ReferShopperStatusID, shopper.IsQualified,
						isAwardable, itemSourceCode, dateEnrolled);
				}
			}
		}

		private class InternalReferAFriendShopper
		{
			public long ReferShopperID { get; set; }

			public int ReferShopperStatusID { get; set; }

			public bool IsQualified { get; set; }

			public bool Exists { get; set; }
		}
	}
}