using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendEnrollResponseData : IResponseData
	{
		private AtlantisException _exception = null;

		public ReferAFriendEnrollResponseData(string itemSourceCode, string shopperID, bool isValid,
			bool isTaken, bool isBlacklisted, long referShopperID)
		{
			this.ItemSourceCode = itemSourceCode;
			this.ShopperID = shopperID;
			this.IsValid = isValid;
			this.IsTaken = isTaken;
			this.IsBlacklisted = isBlacklisted;
			this.ReferShopperID = referShopperID;
			this.IsSuccess = true;
		}

		public ReferAFriendEnrollResponseData(AtlantisException exception)
		{
			this._exception = exception;
		}

		public bool IsBlacklisted { get; private set; }

		public bool IsSuccess { get; private set; }

		public bool IsTaken { get; private set; }

		public bool IsValid { get; set; }

		public string ItemSourceCode { get; set; }

		public long ReferShopperID { get; private set; }

		public string ShopperID { get; set; }

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