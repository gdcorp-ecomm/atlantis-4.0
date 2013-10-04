using Atlantis.Framework.ClassLibrary.CRM.Tests.Properties;
using Atlantis.Framework.CRM.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.ClassLibrary.CRM.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CRM.Impl.dll")]
  public class InsertPrivacyDataAndScheduleCallTests
  {

    int _clientId = 44;
    int _REQUESTTYPE = 743;
    [TestMethod]
    public void InsertPrivacyDataAndScheduleCallRequestProperties()
    {
      string privacyXML = Resources.privacyXML;
      string scheduleXML = Resources.scheduleXML;
      InsertPrivacyDataAndScheduleCallRequestData request = new InsertPrivacyDataAndScheduleCallRequestData(privacyXML, scheduleXML, _clientId);
      Assert.IsTrue(!string.IsNullOrEmpty(request.PrivacyDataXML) && !string.IsNullOrEmpty(request.ScheduleXML) && (request.ClientId == _clientId));
    }

    [TestMethod]
    public void InsertPrivacyDataAndScheduleCallRequestPropertiesNull()
    {
      string privacyXML = null;
      string scheduleXML = null;
      InsertPrivacyDataAndScheduleCallRequestData request = new InsertPrivacyDataAndScheduleCallRequestData(privacyXML, scheduleXML, _clientId);
      Assert.IsTrue(string.IsNullOrEmpty(request.PrivacyDataXML) && string.IsNullOrEmpty(request.ScheduleXML) && (request.ClientId == _clientId));
    }

    [TestMethod]
    public void InsertPrivacyDataAndScheduleCallResponse()
    {
      InsertPrivacyDataAndScheduleCallResponseData response = InsertPrivacyDataAndScheduleCallResponseData.FromXml(string.Empty);
      Assert.AreEqual(response.Status, ResultStatus.OtherError);
    }

    [TestMethod]
    public void CheckForInvalidPhone()
    {
      string privacyXML = Resources.privacyXMLInvalidPhone;
      string scheduleXML = Resources.scheduleXMLInvalidPhone;
      var request = new InsertPrivacyDataAndScheduleCallRequestData(privacyXML, scheduleXML, _clientId);
      var response = (InsertPrivacyDataAndScheduleCallResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(response.Status, ResultStatus.InvalidPhone);
    }

    [TestMethod]
    public void StatusIsSuccess()
    {
      string privacyXML = Resources.privacyXML;
      string scheduleXML = Resources.scheduleXML;
      var request = new InsertPrivacyDataAndScheduleCallRequestData(privacyXML, scheduleXML, _clientId);
      var response = (InsertPrivacyDataAndScheduleCallResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(response.Status, ResultStatus.Success);
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void ToXMLNotImplemented()
    {
      InsertPrivacyDataAndScheduleCallResponseData response = InsertPrivacyDataAndScheduleCallResponseData.FromXml(string.Empty);
      var test = response.ToXML();
    }
  }
}
