using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyPlusBundleXML.Interface
{
  public class PrivacyPlusBundleXMLRequestData : RequestData
  {
    #region Properities
    public int DomainId { get; private set; }
    public double Duration { get; private set; }
    #endregion

    public PrivacyPlusBundleXMLRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int domainId
      , double duration)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      DomainId = domainId;
      Duration = duration;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      byte[] data = Encoding.UTF8.GetBytes("PrivacyPlusBundleXML::" + DomainId + ":" + Duration.ToString());

      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;

      throw new NotImplementedException("GetCacheMD5 not implemented in PrivacyPlusBundleXMLRequestData");     
    }
  }
}
