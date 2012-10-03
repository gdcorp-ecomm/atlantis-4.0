using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoToolGetProdPromo.Interface
{
	public class PromoToolGetProdPromoResponseData : IResponseData
	{
		public ProdPromo[] ProductPromos { get; private set; }
		private AtlantisException _exception = null;

		public PromoToolGetProdPromoResponseData()
		{
		}

    public PromoToolGetProdPromoResponseData(ProdPromo[] Promos)
    {
      ProductPromos = Promos;
    }

		public PromoToolGetProdPromoResponseData(RequestData requestData, Exception ex)
		{
			_exception = new AtlantisException(
				requestData, "Atlantis.Framework.PromoToolGetProdPromo", ex.Message, ex.StackTrace, ex);
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

	public class ProdPromo
	{
		public int RankValue { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public bool IsActive { get; set; }
		public string Currencies { get; set; }
		public int? UseLimit { get; set; }
		public int? UsePerPurchase { get; set; }
		public RestrictionType Restriction { get; set; }
		public bool IsRestrictedByShopperId { get; set; }
		public int ShopperPriceTypeExclusion { get; set; }
	}

	public enum RestrictionType
	{
		NoRestriction = 0,
		Restricted = 1,
		NewShopperOnly = 2
	}
}
