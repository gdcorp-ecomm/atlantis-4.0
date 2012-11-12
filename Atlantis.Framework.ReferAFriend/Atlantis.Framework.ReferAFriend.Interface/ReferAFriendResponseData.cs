using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;
using System;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendResponseData : IResponseData, ISessionSerializableResponse
	{
		private AtlantisException _exception = null;

		public ReferAFriendResponseData(string shopperID)
		{
			this.ShopperID = shopperID;
		}

		public ReferAFriendResponseData(AtlantisException exception)
		{
			_exception = exception;
		}

		public ReferAFriendResponseData(string shopperID, long referShopperID, int referShopperStatusID, bool isQualified, bool isInStoreCreditAwardable,
			string itemSourceCode, DateTime dateEnrolled)
		{
			this.ShopperID = shopperID;
			this.ReferShopperID = referShopperID;
			this.ReferShopperStatusID = referShopperStatusID;
			this.IsQualified = isQualified;
			this.IsInStoreCreditAwardable = isInStoreCreditAwardable;
			this.ItemSourceCode = itemSourceCode;
			this.DateEnrolled = dateEnrolled;
			this.IsSuccess = true;
		}

		public DateTime DateEnrolled { get; private set; }

		public bool IsInStoreCreditAwardable { get; private set; }

		public bool IsQualified { get; private set; }

		public bool IsSuccess { get; private set; }

		public string ItemSourceCode { get; private set; }

		public long ReferShopperID { get; private set; }

		public int ReferShopperStatusID { get; private set; }

		public string ShopperID { get; private set; }

		public void DeserializeSessionData(string sessionData)
		{
			if (!string.IsNullOrEmpty(sessionData))
			{
				Util.DeserializeFromXML(sessionData, this);
			}
		}

		public AtlantisException GetException()
		{
			return _exception;
		}

		public string SerializeSessionData()
		{
			return ToXML();
		}

		public string ToXML()
		{
			return Util.SerializeToXML(this);
		}
	}
}