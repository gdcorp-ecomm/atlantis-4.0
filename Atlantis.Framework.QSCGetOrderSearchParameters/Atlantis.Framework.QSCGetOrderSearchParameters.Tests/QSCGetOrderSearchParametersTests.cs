using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.QSC.Interface.Enums;
using Atlantis.Framework.QSCGetOrderSearchParameters.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCGetOrderSearchParameters.Tests
{
  [TestClass]
  public class QSCGetOrderSearchParametersTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetOrderSearchParameterList()
    {
			string _shopperId = "837435";
      int requestId = 545;
			string accountUid = "265ddd62-2f88-11de-baa9-005056956427";

      QSCGetOrderSearchParametersRequestData request = new QSCGetOrderSearchParametersRequestData(_shopperId, "", string.Empty, string.Empty, 1, accountUid);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCGetOrderSearchParametersResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetOrderSearchParametersResponseData;

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.Response.searchableFields.Count() > 0);

      Console.WriteLine(response.ToXML());
    }
  }
}
