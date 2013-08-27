using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MiniEncrypt.Tests
{
  [TestClass]
  public class MiniEncryptTests
  {
    [TestMethod]
    public void TestEncryptPassword()
    {
      string pwToEncrypt = "Firefly";
      string pwEncrypted;

      using (var password = MiniEncryptPassword.CreateDisposable())
      {
        pwEncrypted = password.EncryptPassword(pwToEncrypt);
      }

      Assert.IsNotNull(pwEncrypted);
      Assert.AreNotEqual(pwToEncrypt, pwEncrypted);
    }

    [TestMethod]
    public void TestDecryptEncryptManagerValues()
    {
      const string userName = "syukna";
      const string userId = "10231";
      string decryptedUserName;
      string decryptedUserId;

      using (var auth = MiniEncryptAuthentication.CreateDisposable())
      {
        string encryptedMstk = auth.GetMgrEncryptedValue(userId, userName);
        auth.GetMgrDecryptedValues(encryptedMstk, out decryptedUserId, out decryptedUserName);
      }

      Assert.AreEqual(decryptedUserId, userId);
      Assert.AreEqual(decryptedUserName, userName);
    }

    [TestMethod]
    public void TestDecryptEncryptCookieValues()
    {
      string valueToEncrypt = "Serenity";
      string encryptedValue;
      string decryptedValue;

      using (var cookie = MiniEncryptCookie.CreateDisposable())
      {
        encryptedValue = cookie.EncryptCookieValue(valueToEncrypt);
        decryptedValue = cookie.DecryptCookieValue(encryptedValue);
      }

      Assert.IsNotNull(encryptedValue);
      Assert.IsNotNull(decryptedValue);
      Assert.AreEqual(decryptedValue, valueToEncrypt);
    }
  }
}
