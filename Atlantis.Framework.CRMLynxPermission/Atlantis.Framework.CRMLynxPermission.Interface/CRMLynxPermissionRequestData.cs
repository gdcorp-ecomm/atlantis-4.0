using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CRMLynxPermission.Interface
{
  public class CRMLynxPermissionRequestData : RequestData
  {
    #region Properties

    public int ManagerUserId { get; private set; }
    public string PermissionKey { get; private set; }

    #endregion

    public CRMLynxPermissionRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int managerUserId, string permissionKey)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ManagerUserId = managerUserId;
      PermissionKey = permissionKey;
      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Format("{0}-{1}", ManagerUserId, PermissionKey));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
