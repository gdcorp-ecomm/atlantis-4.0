using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendOptOutRequestData : RequestData
	{
		public ReferAFriendOptOutRequestData(
			string[] emailAddresses,
			string shopperId,
			string sourceUrl,
			string orderId,
			string pathway,
			int pageCount)
			: base(shopperId, sourceUrl, orderId, pathway, pageCount)
		{
			this.EmailAddresses = emailAddresses;
		}

		public string[] EmailAddresses { get; private set; }

		public override string GetCacheMD5()
		{
			return Util.GetCacheMD5(ShopperID);
		}
	}
}