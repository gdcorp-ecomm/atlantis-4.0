using System;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendActivityData
	{
		public DateTime ActivityDate { get; set; }

		public int Amount { get; set; }

		public DateTime EffectiveCreditDate { get; set; }

		public bool IsNewCustomer { get; set; }

		public bool IsPendingCredit { get; set; }

		public string OrderID { get; set; }

		public string Referral { get; set; }
	}
}