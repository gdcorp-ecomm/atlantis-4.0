using Atlantis.Framework.MailApi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MailApi.Tests
{
  [TestClass]
  public class IntegrationTests
  {
    private const string ANDROID_APP_KEY = "tv2YfSzBx6zdjHAjIhW9mNe5";
    private const string IOS_APP_KEY = "PAiycbPel3SL1H6tj7UxNwUU";
    private const string FOLDER_NUMBER = "821720";
    private const string OFFSET = "0";
    private const string COUNT = "50";
    private const string FILTER = "none";
    private const string BAD_SESSION_HASH = "3ec646ddd52660180db869372bf86c36";
    private const string BAD_BASE_URL = "mailapi.secureserver.net";

    [TestMethod]
    public void GetMessageListRequestTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string sessionHash = loginResponse.LoginData.Hash;
      string baseUrl = loginResponse.LoginData.BaseUrl;

      var getMessageListRequest = new GetMessageListRequestData(FOLDER_NUMBER, OFFSET, COUNT, FILTER, sessionHash, baseUrl, ANDROID_APP_KEY);
      var getMessageListResponse = (GetMessageListResponseData)Engine.Engine.ProcessRequest(getMessageListRequest, 10352);

      Assert.AreEqual(50, getMessageListResponse.MessageListData.MailHeaderList.Count);
      Assert.AreEqual(sessionHash, getMessageListResponse.State.Session);
    }

    [TestMethod]
    public void GetMessageListRequestJsoapFaultBadSessionHashTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string baseUrl = loginResponse.LoginData.BaseUrl;

      var getMessageListRequest = new GetMessageListRequestData(FOLDER_NUMBER, OFFSET, COUNT, FILTER, BAD_SESSION_HASH, baseUrl, ANDROID_APP_KEY);
      var getMessageListResponse = (GetMessageListResponseData)Engine.Engine.ProcessRequest(getMessageListRequest, 10352);

      Assert.IsNull(getMessageListResponse.MessageListData);
      Assert.IsTrue(getMessageListResponse.IsJsoapFault);
      Assert.AreEqual("Error validating session", getMessageListResponse.JsoapDetail);
      Assert.AreEqual("INVALID_SESSION", getMessageListResponse.JsoapMessage);
      Assert.AreEqual(0, getMessageListResponse.ResultCode);
    }

    [TestMethod]
    public void GetMessageListRequestJsoapFaultEmptySessionHashTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string baseUrl = loginResponse.LoginData.BaseUrl;

      var getMessageListRequest = new GetMessageListRequestData(FOLDER_NUMBER, OFFSET, COUNT, FILTER, string.Empty, baseUrl, ANDROID_APP_KEY);
      var getMessageListResponse = (GetMessageListResponseData)Engine.Engine.ProcessRequest(getMessageListRequest, 10352);

      Assert.IsNull(getMessageListResponse.MessageListData);
      Assert.IsTrue(getMessageListResponse.IsJsoapFault);
      Assert.AreEqual("Error validating session", getMessageListResponse.JsoapDetail);
      Assert.AreEqual("INVALID_SESSION", getMessageListResponse.JsoapMessage);
      Assert.AreEqual(0, getMessageListResponse.ResultCode);
    }

    [TestMethod]
    public void GetMessageListRequestJsoapBadBaseUrlTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string sessionHash = loginResponse.LoginData.Hash;

      var getMessageListRequest = new GetMessageListRequestData(FOLDER_NUMBER, OFFSET, COUNT, FILTER, sessionHash, BAD_BASE_URL, ANDROID_APP_KEY);
      var getMessageListResponse = (GetMessageListResponseData)Engine.Engine.ProcessRequest(getMessageListRequest, 10352);

      Assert.IsNull(getMessageListResponse.MessageListData);
      Assert.IsTrue(getMessageListResponse.IsJsoapFault);
      Assert.AreEqual("Error validating session", getMessageListResponse.JsoapDetail);
      Assert.AreEqual("INVALID_SESSION", getMessageListResponse.JsoapMessage);
      Assert.AreEqual(0, getMessageListResponse.ResultCode);
    }

    [TestMethod]
    public void GetMessageListRequestJsoapEmptyBaseUrlTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string sessionHash = loginResponse.LoginData.Hash;

      var getMessageListRequest = new GetMessageListRequestData(FOLDER_NUMBER, OFFSET, COUNT, FILTER, sessionHash, string.Empty, ANDROID_APP_KEY);
      var getMessageListResponse = (GetMessageListResponseData)Engine.Engine.ProcessRequest(getMessageListRequest, 10352);

      Assert.IsNull(getMessageListResponse.MessageListData);
      Assert.IsTrue(getMessageListResponse.IsJsoapFault);
      Assert.AreEqual("Error validating session", getMessageListResponse.JsoapDetail);
      Assert.AreEqual("INVALID_SESSION", getMessageListResponse.JsoapMessage);
      Assert.AreEqual(0, getMessageListResponse.ResultCode);
    }

    [TestMethod]
    public void GetFolderRequestTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string sessionHash = loginResponse.LoginData.Hash;
      var key = string.Empty;

      var getFolderRequest = new GetFolderRequestData(FOLDER_NUMBER, sessionHash, ANDROID_APP_KEY, key, loginResponse.LoginData.BaseUrl);
      var getFolderResponse = (GetFolderResponseData)Engine.Engine.ProcessRequest(getFolderRequest, 10353);

      Assert.IsNotNull(getFolderResponse);
      Assert.IsNotNull(getFolderResponse.MailFolder);
      Assert.AreEqual("inbox", getFolderResponse.MailFolder.DisplayName.ToLowerInvariant());

    }

    [TestMethod]
    public void GetFolderRequest_InvalidFolderIdTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string sessionHash = loginResponse.LoginData.Hash;
      var key = string.Empty;

      var getFolderRequest = new GetFolderRequestData("-9999999x2", sessionHash, ANDROID_APP_KEY, key, loginResponse.LoginData.BaseUrl);
      var getFolderResponse = (GetFolderResponseData)Engine.Engine.ProcessRequest(getFolderRequest, 10353);

      Assert.IsNotNull(getFolderResponse);
      Assert.IsTrue(getFolderResponse.IsJsoapFault);
      Assert.IsNull(getFolderResponse.MailFolder);
    }
	
	[TestMethod]
    public void GetFolderListRequestTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string sessionHash = loginResponse.LoginData.Hash;
      var key = string.Empty;

      var getFolderListRequest = new GetFolderListRequestData(sessionHash, ANDROID_APP_KEY, key, loginResponse.LoginData.BaseUrl);
      var getFolderListResponse = (GetFolderListResponseData)Engine.Engine.ProcessRequest(getFolderListRequest, 10351);

      Assert.IsNotNull(getFolderListResponse);
      Assert.IsNotNull(getFolderListResponse.MailFolders);
    }
  }
}
