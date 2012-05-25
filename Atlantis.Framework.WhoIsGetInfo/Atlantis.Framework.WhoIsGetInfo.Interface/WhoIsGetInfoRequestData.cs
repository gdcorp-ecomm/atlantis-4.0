using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.WhoIsGetInfo.Interface
{
	public class WhoIsGetInfoRequestData: RequestData
	{
		private TimeSpan _requestTimeout = new TimeSpan(0, 0, 10);
		public TimeSpan RequestTimeout
		{
			get { return _requestTimeout; }
			set { _requestTimeout = value; }
		}
		public int PrivateLabelId { get; set; }
		public string DomainToLookup { get; set; }
    
		public WhoIsGetInfoRequestData(string shopperId,
		                                    string sourceUrl,
		                                    string orderId,
		                                    string pathway,
		                                    int pageCount, 
																				string domainName, 
																				int privateLabelId)
			: base(shopperId, sourceUrl, orderId, pathway, pageCount)
		{
			this.DomainToLookup = domainName;
			this.PrivateLabelId = privateLabelId;
		}

		public override string GetCacheMD5()
		{
			throw new NotImplementedException();
		}
	}
}
