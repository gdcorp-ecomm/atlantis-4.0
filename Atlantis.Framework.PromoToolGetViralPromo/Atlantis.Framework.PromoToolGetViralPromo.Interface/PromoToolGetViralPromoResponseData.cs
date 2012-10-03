using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoToolGetViralPromo.Interface
{
	public class PromoToolGetViralPromoResponseData : IResponseData
	{
		public OutputViralPromo[] ViralPromos { get; private set; }
		private AtlantisException _exception = null;

		public PromoToolGetViralPromoResponseData()
		{
		}

    public PromoToolGetViralPromoResponseData(OutputViralPromo[] promos)
    {
      ViralPromos = promos;
    }

		public PromoToolGetViralPromoResponseData(RequestData requestData, Exception ex)
		{
			_exception = new AtlantisException(
				requestData, "Atlantis.Framework.PromoToolGetViralPromo", ex.Message, ex.StackTrace, ex);
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

	public class OutputViralPromo
	{
		public string Description { get; set; }	
		public DateTime StartDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public bool IsActive { get; set; }
		public string Currencies { get; set; }
		public int? UseLimit { get; set; }
		public int RequiredYard { get; set; }
		public string[] ExcludedPaymentTypes { get; set; }
		public bool NewShopperOnly { get; set; }
	}
}
