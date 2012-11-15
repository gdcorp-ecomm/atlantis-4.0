using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.ReferAFriend.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Atlantis.Framework.ReferAFriend.Impl
{
	public class ReferAFriendOptOutRequest : IRequest
	{
		public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
		{
			ReferAFriendOptOutResponseData resp = null;

			try
			{
				var req = (ReferAFriendOptOutRequestData)requestData;

				resp = GetOptOutListFromDB(req, config);
			}
			catch (AtlantisException atlantisEx)
			{
				resp = new ReferAFriendOptOutResponseData(atlantisEx);
			}
			catch (Exception ex)
			{
				resp = new ReferAFriendOptOutResponseData(new AtlantisException(requestData,
					"ReferAFriendOptOutRequest::RequestHandler", "Unable to check referral optout lists.", string.Empty, ex));
			}

			return resp;
		}

		private ReferAFriendOptOutResponseData GetOptOutListFromDB(ReferAFriendOptOutRequestData req, ConfigElement config)
		{
			var hashDict = GetHashString(req.EmailAddresses, req);

			var list = new List<string>();
			string procName = "dbo.refer_shopperEmailOptOuts_sp";

			string connectionString = NetConnect.LookupConnectInfo(config);
			using (var cn = new SqlConnection(connectionString))
			{
				using (var cmd = new SqlCommand(procName, cn))
				{
					cmd.CommandTimeout = Util.GetRequestTimeout(req, config);
					cmd.CommandType = CommandType.StoredProcedure;

					var hashList = hashDict.Keys.ToArray();
					cmd.Parameters.Add(new SqlParameter("@emailHashList", string.Join(",", hashList)));

					cn.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							var hash = (string)dr["emailHash"];
							string email = null;

							if (hashDict.TryGetValue(hash, out email))
							{
								list.Add(email);
							}
						}
					}
				}
			}

			return new ReferAFriendOptOutResponseData(list);
		}

		public Dictionary<string, string> GetHashString(string[] emailAddresses, ReferAFriendOptOutRequestData req)
		{
			var returnDict = new Dictionary<string, string>();

			gdHashLib.IHashSHA oHash = null;

			try
			{
				oHash = new gdHashLib.HashSHAClass();

				foreach (var emailAddress in emailAddresses.Select(s => s.ToLower()).Distinct())
				{
					returnDict.Add(oHash.GetHash(emailAddress), emailAddress);
				}

				return returnDict;
			}

			catch (System.Exception ex)
			{
				throw new AtlantisException(req, "ReferAFriendOptOutRequest::GetHashString", "Unable to hash email addresses.", string.Empty, ex);
			}

			finally
			{
				if (oHash != null)
				{
					System.Runtime.InteropServices.Marshal.ReleaseComObject(oHash);
				}
			}
		}
	}
}