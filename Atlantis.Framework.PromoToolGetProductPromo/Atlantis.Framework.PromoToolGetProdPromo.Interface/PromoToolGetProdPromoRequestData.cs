using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoToolGetProdPromo.Interface
{
	public class PromoToolGetProdPromoRequestData : RequestData
	{
		private const int  DEFAULT_TIMEOUT = 10;
		public string PromoCode { get; set; }

		public PromoToolGetProdPromoRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string promoCode)
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
