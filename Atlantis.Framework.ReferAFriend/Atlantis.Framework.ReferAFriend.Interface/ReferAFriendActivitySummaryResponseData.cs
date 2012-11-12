using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendActivitySummaryResponseData : IResponseData
	{
		private AtlantisException _exception = null;

		public ReferAFriendActivitySummaryResponseData(int referralCount, int referralTotalAmount)
		{
			this.ReferralCount = referralCount;
			this.ReferralTotalAmount = referralTotalAmount;
			this.IsSuccess = true;
		}

		public ReferAFriendActivitySummaryResponseData(AtlantisException atlantisEx)
		{
			this._exception = atlantisEx;
		}

		public bool IsSuccess { get; private set; }

		public int ReferralCount { get; private set; }

		public int ReferralTotalAmount { get; private set; }

		public AtlantisException GetException()
		{
			return _exception;
		}

		public string ToXML()
		{
			return Util.SerializeToXML(this);
		}
	}
}