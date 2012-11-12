using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendControlResponseData : IResponseData
	{
		private Dictionary<string, ReferAFriendControlData> _dictResult;
		private AtlantisException _exception = null;

		public ReferAFriendControlResponseData()
		{
		}

		public ReferAFriendControlResponseData(AtlantisException exception)
		{
			_exception = exception;
			this.IsSuccess = false;
		}

		public ReferAFriendControlResponseData(Dictionary<string, ReferAFriendControlData> dictResult)
		{
			this._dictResult = dictResult;
			this.IsSuccess = true;
		}

		public bool IsSuccess { get; private set; }

		public T Get<T>(string key, T defaultValue)
		{
			ReferAFriendControlData d = null;
			if (_dictResult.TryGetValue(key, out d))
			{
				return (T)Convert.ChangeType(d.Value, typeof(T));
			}
			return defaultValue;
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