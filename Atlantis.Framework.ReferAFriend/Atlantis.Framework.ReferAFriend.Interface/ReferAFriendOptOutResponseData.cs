using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendOptOutResponseData : IResponseData
	{
		private AtlantisException _exception = null;

		public ReferAFriendOptOutResponseData(List<string> optOutList)
		{
			this._optOutList = optOutList;
			this.IsSuccess = true;
		}

		public ReferAFriendOptOutResponseData(AtlantisException exception)
		{
			_exception = exception;
		}

		public List<string> _optOutList { get; private set; }

		public bool IsSuccess { get; private set; }

		public List<string> OptOutList
		{
			get { return _optOutList; }
		}

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