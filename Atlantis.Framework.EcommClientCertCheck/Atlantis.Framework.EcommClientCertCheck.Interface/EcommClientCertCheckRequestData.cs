using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommClientCertCheck.Interface
{
  public class EcommClientCertCheckRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(8);

    public string CertificateSubject { get; private set; }

    public string ApplicationTeam { get; private set; }

    public string ApplicationName { get; private set; }

    public string MethodName { get; private set; }

    public EcommClientCertCheckRequestData(string certificateSubject, string applicationTeam, string applicationName, string methodName, string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      CertificateSubject = certificateSubject;
      ApplicationTeam = applicationTeam;
      ApplicationName = applicationName;
      MethodName = methodName;
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("EcommClientCertCheck is already cached using DataCache");
    }
  }
}
