using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendEnrollRequestData : RequestData
	{
		public ReferAFriendEnrollRequestData(string itemSourceCode,
			string shopperId,
			string sourceUrl,
			string orderId,
			string pathway,
			int pageCount)
			: base(shopperId, sourceUrl, orderId, pathway, pageCount)
		{
			this.ItemSourceCode = itemSourceCode;
		}

		public string ItemSourceCode { get; private set; }

		public override string GetCacheMD5()
		{
			return Util.GetCacheMD5(ItemSourceCode);
		}
	}
}