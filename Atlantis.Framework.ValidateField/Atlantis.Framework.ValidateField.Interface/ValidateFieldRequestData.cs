using System;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.ValidateField.Interface
{
  public class ValidateFieldRequestData : RequestData
  {
    public string FieldNameKey { get; private set; }
    public string Culture { get; private set; }


    public ValidateFieldRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string fieldNameKey)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Culture = string.Empty;
      FieldNameKey = fieldNameKey;
    }

    public ValidateFieldRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string fieldNameKey, string culture)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Culture = culture;
      FieldNameKey = fieldNameKey;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(FieldNameKey.ToLowerInvariant() + Culture);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
  }
}
