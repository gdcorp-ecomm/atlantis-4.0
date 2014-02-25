﻿using Atlantis.Framework.MailApi.Interface;
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

    [TestMethod]
    public void GetMessageListRequestTest()
    {
      var loginRequest = new LoginRequestData("tester@qa-emailpod04.com", "Godaddy25", ANDROID_APP_KEY);
      var loginResponse = (LoginResponseData)Engine.Engine.ProcessRequest(loginRequest, 10350);
      string sessionHash = loginResponse.LoginData.Hash;

      var getMessageListRequest = new GetMessageListRequestData(FOLDER_NUMBER, OFFSET, COUNT, FILTER, sessionHash, loginResponse.LoginData.BaseUrl, ANDROID_APP_KEY);
      var getMessageListResponse = (GetMessageListResponseData)Engine.Engine.ProcessRequest(getMessageListRequest, 10352);

      Assert.AreEqual(50, getMessageListResponse.MessageListData.MailHeaderList.Count);
      Assert.AreEqual(sessionHash, getMessageListResponse.State.Session);
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
      Assert.Equals("inbox", getFolderResponse.MailFolder.DisplayName.ToLowerInvariant());

    }
  }
}