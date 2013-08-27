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

      using (var password = PasswordEncryption.CreateDisposable())
      {
        pwEncrypted = password.EncryptPassword(pwToEncrypt);
      }

      Assert.IsNotNull(pwEncrypted);
      Assert.AreNotEqual(pwToEncrypt, pwEncrypted);
    }

    [TestMethod]
    public void TestMstkSingleNullAndEmptyInputs()
    {
      using (var auth = MstkAuthentication.CreateDisposable())
      {
        var mstk = auth.CreateMstk(null, "mmicco");
        Assert.AreNotEqual(string.Empty, mstk);

        mstk = auth.CreateMstk("1000", null);
        Assert.AreNotEqual(string.Empty, mstk);

        mstk = auth.CreateMstk(string.Empty, "mmicco");
        Assert.AreNotEqual(string.Empty, mstk);

        mstk = auth.CreateMstk("1000", string.Empty);
        Assert.AreNotEqual(string.Empty, mstk);
      }
    }

    [TestMethod]
    public void TestMstkBothNullAndEmptyInputs()
    {
      using (var auth = MstkAuthentication.CreateDisposable())
      {
        var mstk = auth.CreateMstk(null, null);
        Assert.AreEqual(string.Empty, mstk);

        mstk = auth.CreateMstk(string.Empty, null);
        Assert.AreEqual(string.Empty, mstk);

        mstk = auth.CreateMstk(string.Empty, null);
        Assert.AreEqual(string.Empty, mstk);

        mstk = auth.CreateMstk(string.Empty, string.Empty);
        Assert.AreEqual(string.Empty, mstk);
      }
    }


    [TestMethod]
    public void TestDecryptEncryptManagerValues()
    {
      const string userName = "syukna";
      const string userId = "10231";
      string decryptedUserName;
      string decryptedUserId;

      using (var auth = MstkAuthentication.CreateDisposable())
      {
        string encryptedMstk = auth.CreateMstk(userId, userName);
        auth.ParseMstk(encryptedMstk, out decryptedUserId, out decryptedUserName);
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

      using (var cookie = CookieEncryption.CreateDisposable())
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
