﻿using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoToolGetOrderPromo.Interface
{
	public class PromoToolGetOrderPromoResponseData : IResponseData
	{
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public bool IsActive { get; set; }
		public string Currencies { get; set; }
		public string[] ExcludedPaymentTypes { get; set; }
		public int? UseLimit { get; set; }
		public RestrictionType Restriction { get; set; }

		private AtlantisException _exception = null;
    
    public PromoToolGetOrderPromoResponseData()
    {
    }

		public PromoToolGetOrderPromoResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(
				requestData, "Atlantis.Framework.PromoToolGetOrderPromo", ex.Message, ex.StackTrace, ex);
    }

		#region IResponseData Members

		public string ToXML()
		{
			throw new NotImplementedException();
		}

		public AtlantisException GetException()
		{
			return _exception;
		}

		#endregion
	}

	public enum RestrictionType
	{
		NoRestriction = 0,
		Restricted = 1,
		NewShopperOnly = 2
	}
}