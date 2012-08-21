using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Atlantis.Framework.QSCGetCountries.Interface
{
	public class QSCGetCountriesRequestData : RequestData
	{
		public QSCGetCountriesRequestData(string shopperId,
			string sourceURL,
			string orderId,
			string pathway,
			int pageCount
			)
			: base(shopperId, sourceURL, orderId, pathway, pageCount)
		{
			IncludeRegions = false;
			IncludeRegionsSpecified = false;
			SortByCountryCode = false;
			SortByCountryCodeSpecified = false;
			OrderSearchFields = new List<orderSearchField>();
		}

		public QSCGetCountriesRequestData(string shopperId,
			string sourceURL,
			string orderId,
			string pathway,
			int pageCount,
			bool includeRegions,
			bool sortByCountryCode
			)
			: base(shopperId, sourceURL, orderId, pathway, pageCount)
		{
			IncludeRegions = includeRegions;
			IncludeRegionsSpecified = true;
			SortByCountryCode = sortByCountryCode;
			SortByCountryCodeSpecified = true;
			OrderSearchFields = new List<orderSearchField>();
		}

		public bool IncludeRegions { get; set; }
		public bool IncludeRegionsSpecified { get; set; }
		public bool SortByCountryCode { get; set; }
		public bool SortByCountryCodeSpecified { get; set; }
		public List<orderSearchField> OrderSearchFields { get; set; }

		private string CacheKey
		{
			get 
			{
				StringBuilder search = new StringBuilder();
				
				search.Append(IncludeRegions);
				search.Append("|");
				search.Append(SortByCountryCode);
				search.Append("|");

				foreach (var item in OrderSearchFields)
				{
					search.Append(item.property);
					search.Append("|");
					search.Append(item.value);
				}
				return "QSCGetCountries:" + search.ToString(); 
			}
		}

		#region Overrides of RequestData

		public override string GetCacheMD5()
		{
			MD5 oMd5 = new MD5CryptoServiceProvider();
			oMd5.Initialize();
			byte[] stringBytes = Encoding.ASCII.GetBytes(CacheKey);
			byte[] md5Bytes = oMd5.ComputeHash(stringBytes);
			string sValue = BitConverter.ToString(md5Bytes, 0);
			return sValue.Replace("-", string.Empty);
		}

		#endregion
	}
}
