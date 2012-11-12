using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendActivitySummaryRequestData : RequestData
	{
		public ReferAFriendActivitySummaryRequestData(string sourceCode,
			DateTime startDate,
			DateTime endDate,
			string shopperId,
			string sourceUrl,
			string orderId,
			string pathway,
			int pageCount)
			: base(shopperId, sourceUrl, orderId, pathway, pageCount)
		{
			this.SourceCode = sourceCode;
			this.StartDate = startDate;
			this.EndDate = endDate;
		}

		public DateTime EndDate { get; set; }

		public string SourceCode { get; set; }

		public DateTime StartDate { get; set; }

		public override string GetCacheMD5()
		{
			return Util.GetCacheMD5(SourceCode, ShopperID, StartDate, EndDate);
		}
	}
}