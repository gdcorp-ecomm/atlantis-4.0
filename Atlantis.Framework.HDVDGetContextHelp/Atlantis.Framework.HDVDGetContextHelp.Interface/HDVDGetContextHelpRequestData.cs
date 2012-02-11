using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetContextHelp.Interface
{
  public class HDVDGetContextHelpRequestData : RequestData
  {
    private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(15);

    public HDVDGetContextHelpRequestData(
      string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountGuid,
      string contextHelpPage,
      string contextHelpField) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountGuid = accountGuid;
      ContextHelpPage = contextHelpPage;
      ContextHelpField = contextHelpField;
      RequestTimeout = _requestTimeout;
    }

    public string AccountGuid { get; set; }
    
    public string ContextHelpPage { get; set; }
    
    public string ContextHelpField { get; set; }


    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("HDVDGetContextHelpRequest is not a cacheable request.");
    }

    #endregion
  }
}
