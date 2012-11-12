using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendRequestData : RequestData
	{
		public ReferAFriendRequestData(
			string shopperId,
			string sourceUrl,
			string orderId,
			string pathway,
			int pageCount)
			: base(shopperId, sourceUrl, orderId, pathway, pageCount)
		{
		}

		public override string GetCacheMD5()
		{
			return Util.GetCacheMD5(ShopperID);
		}

		public override string ToXML()
		{
			return Util.SerializeToXML(this);
		}
	}
}