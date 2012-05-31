using Atlantis.Framework.SsoIdentityProviderEdit.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SsoIdentityProviderEdit.Tests
{
  [TestClass]
  public class SsoIdentityProviderEditTests
  {
    private const int _requestType = 534;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void SsoServiceProviderEditTest()
    {
      string identityProviderName = "SSOEDITTEST";
      string loginUrl = "https://idp.dev.godaddy-com.ide/login.aspx";
      string logoutUrl = "https://idp.dev.godaddy-com.ide/logout.aspx";
      string publicKey = "MIIFFjCCA/6gAwIBAgIDQSbOMA0GCSqGSIb3DQEBBQUAMIHKMQswCQYDVQQGEwJV  UzEQMA4GA1UECBMHQXJpem9uYTETMBEGA1UEBxMKU2NvdHRzZGFsZTEaMBgGA1UE  ChMRR29EYWRkeS5jb20sIEluYy4xMzAxBgNVBAsTKmh0dHA6Ly9jZXJ0aWZpY2F0  ZXMuZ29kYWRkeS5jb20vcmVwb3NpdG9yeTEwMC4GA1UEAxMnR28gRGFkZHkgU2Vj  dXJlIENlcnRpZmljYXRpb24gQXV0aG9yaXR5MREwDwYDVQQFEwgwNzk2OTI4NzAe  Fw0wNzA5MTkxNjE4MjhaFw0xNzA5MTkxNjE4MjhaMGcxIDAeBgNVBAoTF3Nzby5k  ZXYuZ29kYWRkeS1jb20uaWRlMSEwHwYDVQQLExhEb21haW4gQ29udHJvbCBWYWxp  ZGF0ZWQxIDAeBgNVBAMTF3Nzby5kZXYuZ29kYWRkeS1jb20uaWRlMIGfMA0GCSqG  SIb3DQEBAQUAA4GNADCBiQKBgQC0715UQqhhRDWUKNiF1OLwdzdQO7UviEv1aDNA  T33hYOQb96cHyAn7zVbsmL9Y4j+yYJH2ysbltMTzMVvl6+pnsAwq62wWfGZNQLwm  OjRU4R4Ew9ncrGXgxzubTxNTU8EtDbZdhz1ENASkhE9UAGIaBuT2KxauSKJgFurj  lEq9AwIDAQABo4IB6TCCAeUwCQYDVR0TBAIwADALBgNVHQ8EBAMCBaAwHQYDVR0l  BBYwFAYIKwYBBQUHAwEGCCsGAQUFBwMCMFYGA1UdHwRPME0wS6BJoEeGRWh0dHA6  Ly9jZXJ0aWZpY2F0ZXMuZ29kYWRkeS5jb20vcmVwb3NpdG9yeS9nb2RhZGR5ZXh0  ZW5kZWRpc3N1aW5nLmNybDBSBgNVHSAESzBJMEcGC2CGSAGG/W0BBxcBMDgwNgYI  KwYBBQUHAgEWKmh0dHA6Ly9jZXJ0aWZpY2F0ZXMuZ29kYWRkeS5jb20vcmVwb3Np  dG9yeTB/BggrBgEFBQcBAQRzMHEwIwYIKwYBBQUHMAGGF2h0dHA6Ly9vY3NwLmdv  ZGFkZHkuY29tMEoGCCsGAQUFBzAChj5odHRwOi8vY2VydGlmaWNhdGVzLmdvZGFk  ZHkuY29tL3JlcG9zaXRvcnkvZ2RfaW50ZXJtZWRpYXRlLmNydDAdBgNVHQ4EFgQU  Tkf9JL3aw/c5CVRhr3xnJJvQX5gwHwYDVR0jBBgwFoAU/axhMpNsRdbi7oVfmrrn  dplozOcwPwYDVR0RBDgwNoIXc3NvLmRldi5nb2RhZGR5LWNvbS5pZGWCG3d3dy5z  c28uZGV2LmdvZGFkZHktY29tLmlkZTANBgkqhkiG9w0BAQUFAAOCAQEAjE0N2/1s  szIT+c1pBc9ddMUXaGPJdcVy6Zip2ZxuBuoDm5Y8xV+KjQbYLXAQlbK3+7lkaY1X  Jnba1p23lY69gnVjlEq0UsgFFiJIYWMmTcVsCvyRlx7obZAAbgdtgIynTKHWpyDd  6eAcj4kxi2cSnu54QEn9wQuaMi9SFLGT4g30UB+QAk0dnOHY/W2fL5pE7NTvo1aI  Me3BMzGO1OmGxdY19dohLSZClWKWZRoo4B2wd5nBh+l2Unga/1U5bH81eygtuJLk  e7LMh772QQDLm/dKCQsLBZ/1BO76KaQ48wUyYLiqparJDM08/sG3l8JvnWrU1iP5  /dLdPgOIj+2jsA==  ";
      string certificateName = "sso.dev.godaddy-com.ide";
      string changedBy = "";
      string approvedBy = "";
      string actionDescription = "TESTING - This is test data for the SSO Edit Tool";

      var request = new SsoIdentityProviderEditRequestData(string.Empty
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , identityProviderName
        , loginUrl
        , logoutUrl
        , publicKey
        , certificateName
        , changedBy
        , approvedBy
        , actionDescription);

      var response = (SsoIdentityProviderEditResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
