using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoIdentityProviderEdit.Interface
{
  public class SsoIdentityProviderEditRequestData : RequestData
  {
    #region Properties
    
    public string IdentityProviderName { get; private set; }
    public string LoginUrl { get; private set; }
    public string LogoutUrl { get; private set; }
    public string PublicKey { get; private set; }
    public string CertificateName { get; private set; }
    public string ChangedBy { get; private set; }
    public string ApprovedBy { get; private set; }
    public string ActionDescription { get; private set; }

    #endregion Properties

    public SsoIdentityProviderEditRequestData(string shopperId
                                            , string sourceUrl
                                            , string orderId
                                            , string pathway
                                            , int pageCount
                                            , string identityProviderName
                                            , string loginUrl
                                            , string logoutUrl
                                            , string publicKey
                                            , string certificateName
                                            , string changedBy
                                            , string approvedBy
                                            , string actionDescription)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      IdentityProviderName = identityProviderName;
      LoginUrl = loginUrl;
      LogoutUrl = logoutUrl;
      PublicKey = publicKey;
      CertificateName = certificateName;
      ChangedBy = changedBy;
      ApprovedBy = approvedBy;
      ActionDescription = actionDescription;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in SsoIdentityProviderEditRequestData");
    }
  }
}