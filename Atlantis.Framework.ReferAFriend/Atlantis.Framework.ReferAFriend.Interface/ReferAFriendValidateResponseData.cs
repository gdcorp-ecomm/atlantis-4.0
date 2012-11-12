using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendValidateResponseData : IResponseData
	{
		private AtlantisException _exception = null;

		public ReferAFriendValidateResponseData(string itemSourceCode, bool isValid)
		{
			this.ItemSourceCode = itemSourceCode;
			this.IsValid = isValid;
			this.IsSuccess = true;
		}

		public ReferAFriendValidateResponseData(AtlantisException exception)
		{
			_exception = exception;
			this.IsSuccess = false;
		}

		public bool IsSuccess { get; private set; }

		public bool IsValid { get; private set; }

		public string ItemSourceCode { get; private set; }

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