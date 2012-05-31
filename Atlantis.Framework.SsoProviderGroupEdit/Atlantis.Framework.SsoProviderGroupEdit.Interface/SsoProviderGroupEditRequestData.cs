using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoProviderGroupEdit.Interface
{
  public class SsoProviderGroupEditRequestData : RequestData
  {
    #region Properties

    public string ServiceProviderGroupName { get; private set; }
    public string RedirectLoginUrl { get; private set; }
    public string LogoutUrl { get; private set; }
    public string RedirectLogoutUrl { get; private set; }
    public string ChangedBy { get; private set; }
    public string ApprovedBy { get; private set; }
    public string ActionDescription { get; private set; }

    #endregion Properties

    public SsoProviderGroupEditRequestData(string shopperId
                                            , string sourceUrl
                                            , string orderId
                                            , string pathway
                                            , int pageCount
                                            , string serviceProviderGroupName
                                            , string redirectLoginUrl
                                            , string logoutUrl
                                            , string redirectLogoutUrl
                                            , string changedBy
                                            , string approvedBy
                                            , string actionDescription)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      ServiceProviderGroupName = serviceProviderGroupName;
      RedirectLoginUrl = redirectLoginUrl;
      LogoutUrl = logoutUrl;
      RedirectLogoutUrl = redirectLogoutUrl;      
      ChangedBy = changedBy;
      ApprovedBy = approvedBy;
      ActionDescription = actionDescription;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in SsoProviderGroupEditRequestData");
    }
  }
}
