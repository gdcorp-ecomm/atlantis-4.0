using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCUnblockIP.Interface
{
	public class QSCUnblockIPRequestData : RequestData
	{
		public QSCUnblockIPRequestData(string shopperId,
			string sourceURL,
			string orderId,
			string pathway,
			int pageCount,
			string accountUid,
			string ipAddress)
			: base(shopperId, sourceURL, orderId, pathway, pageCount)
		{
			RequestTimeout = TimeSpan.FromSeconds(5);
			AccountUid = accountUid;
			IpAddress = ipAddress;
		}

		public string AccountUid { get; set; }
		public string IpAddress { get; set; }

		#region Overrides of RequestData

		public override string GetCacheMD5()
		{
			throw new Exception("QSCUnblockIP is not a cacheable request.");
		}

		#endregion
	}
}
