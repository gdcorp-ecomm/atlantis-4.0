using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendActivityResponseData : IResponseData
	{
		private AtlantisException _exception = null;

		public ReferAFriendActivityResponseData(List<ReferAFriendActivityData> items)
		{
			this.Items = items;
			this.IsSuccess = true;
		}

		public ReferAFriendActivityResponseData(AtlantisException atlantisEx)
		{
			this._exception = atlantisEx;
			this.IsSuccess = false;
		}

		public bool IsSuccess { get; private set; }

		public List<ReferAFriendActivityData> Items { get; private set; }

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