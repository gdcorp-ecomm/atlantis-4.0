using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommClientCertCheck.Interface
{
  public class EcommClientCertCheckRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(8);

    public string CertificateSubject { get; private set; }

    public string ApplicationName { get; private set; }

    public string MethodName { get; private set; }

    public EcommClientCertCheckRequestData(string certificateSubject, string applicationName, string methodName, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CertificateSubject = certificateSubject;
      ApplicationName = applicationName;
      MethodName = methodName;
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      byte[] data = Encoding.UTF8.GetBytes(string.Format("EcommClientCertCheckRequestData|{0}|{1}|{2}", CertificateSubject, ApplicationName, MethodName));

      MD5 md5 = new MD5CryptoServiceProvider();
      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}
