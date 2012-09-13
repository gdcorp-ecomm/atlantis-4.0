using System;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetCountries.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCGetCountries.Tests
{
	[TestClass]
	public class QSCGetCountriesTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCGetCountries.Impl.dll")]
		public void GetCountries()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			int requestId = 581;

			orderSearchField orderSearchField = new orderSearchField();
			orderSearchField.property = "countryCode";
			orderSearchField.value = string.Empty;

			QSCGetCountriesRequestData request = new QSCGetCountriesRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, true, false);

			// add the search criteria to the request
			request.OrderSearchFields.Add(orderSearchField);

			request.RequestTimeout = TimeSpan.FromSeconds(10);

			QSCGetCountriesResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetCountriesResponseData;

			Assert.IsTrue(response.IsSuccess);
			Console.WriteLine("Total Records Found: " + response.Response.resultSize);
			Console.WriteLine(response.ToXML());
		}
	}
}
