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
      string _shopperId = "859775";
      int requestId = 545;

      QSCGetOrderSearchParametersRequestData request = new QSCGetOrderSearchParametersRequestData(_shopperId, "", string.Empty, string.Empty, 1);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCGetOrderSearchParametersResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetOrderSearchParametersResponseData;

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.Response.searchableFields.Count() > 0);

      Debug.WriteLine(response.ToXML());
    }
  }
}
