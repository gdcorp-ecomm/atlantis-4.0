using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.ReferAFriend.Interface
{
	public class ReferAFriendMessageTemplateResponseData : IResponseData
	{
		private AtlantisException _exception = null;
		private Dictionary<string, ReferAFriendMessageTemplateData> dictResult;

		public ReferAFriendMessageTemplateResponseData()
		{
		}

		public ReferAFriendMessageTemplateResponseData(AtlantisException exception)
		{
			_exception = exception;
		}

		public ReferAFriendMessageTemplateResponseData(Dictionary<string, ReferAFriendMessageTemplateData> dictResult)
		{
			this.dictResult = dictResult;
			this.IsSuccess = true;
		}

		public bool IsSuccess { get; private set; }

		public AtlantisException GetException()
		{
			return _exception;
		}

		public string GetTemplateText(string templateName)
		{
			ReferAFriendMessageTemplateData d = null;
			if (dictResult.TryGetValue(templateName, out d))
			{
				return d.Text;
			}
			return null;
		}

		public string ToXML()
		{
			return Util.SerializeToXML(this);
		}
	}
}