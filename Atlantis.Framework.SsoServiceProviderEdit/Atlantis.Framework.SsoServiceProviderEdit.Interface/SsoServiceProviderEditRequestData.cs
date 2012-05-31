using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SsoServiceProviderEdit.Interface
{
  public class SsoServiceProviderEditRequestData : RequestData
  {
    #region Properties

    public string ServiceProviderName { get; private set; }
    public string IdentityProviderName { get; private set; }
    public string ServiceProviderGroupName { get; private set; }
    public string LoginReceive { get; private set; }
    public string LoginReceiveType { get; private set; }
    public string ServerName { get; private set; }
    public bool IsRetired { get; private set; }
    public DateTime? RetiredDate { get; private set; }
    public string ChangedBy { get; private set; }
    public string ApprovedBy { get; private set; }
    public string ActionDescription { get; private set; }

    #endregion Properties

    public SsoServiceProviderEditRequestData(string shopperId
                                            , string sourceUrl
                                            , string orderId
                                            , string pathway
                                            , int pageCount
                                            , string serviceProviderName
                                            , string identityProviderName
                                            , string serviceProviderGroupName
                                            , string loginReceive
                                            , string loginReceiveType
                                            , string serverName
                                            , bool isRetired
                                            , DateTime? retiredDate
                                            , string changedBy
                                            , string approvedBy
                                            , string actionDescription)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      ServiceProviderName = serviceProviderName;
      IdentityProviderName = identityProviderName;
      ServiceProviderGroupName = serviceProviderGroupName;
      LoginReceive = loginReceive;
      LoginReceiveType = loginReceiveType;
      ServerName = serverName;
      IsRetired = isRetired;
      RetiredDate = retiredDate;
      ChangedBy = changedBy;
      ApprovedBy = approvedBy;
      ActionDescription = actionDescription;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in SsoServiceProviderEditRequestData");
    }
  }
}