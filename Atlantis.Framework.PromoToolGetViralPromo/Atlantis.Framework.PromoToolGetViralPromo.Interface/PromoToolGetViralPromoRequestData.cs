using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoToolGetViralPromo.Interface
{
	public class PromoToolGetViralPromoRequestData : RequestData
	{
		private const int  DEFAULT_TIMEOUT = 10;
		public string PromoCode { get; set; }

		public PromoToolGetViralPromoRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string promoCode)
			: base(shopperId, sourceUrl, orderId, pathway, pageCount)
		{
			PromoCode = promoCode;
			RequestTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);
		}

		public override string GetCacheMD5()
		{
			throw new System.NotImplementedException();
		}
	}
}
