using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OptInUpdateInfo.Interface
{
  public class OptInUpdateInfoRequestData : RequestData
  {
    public const int EmailTypeText = 1;
    public const int EmailTypeHtml = 2;

    public OptInUpdateInfoRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId, string emailAddress, List<OptIn.Interface.OptIn> optIns, string userHostAddress, string firstName, string lastName, bool isReseller)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      EmailAddress = emailAddress ?? string.Empty;
      UserHostAddress = userHostAddress ?? string.Empty;
      FirstName = firstName;
      LastName = lastName;

      if (optIns != null && optIns.Count > 0)
      {
        _optIns = optIns;
      }
      else
      {
        _optIns = new List<OptIn.Interface.OptIn>();
      }

      EmailTypeId = EmailTypeText;
      PrivateLabelId = privateLabelId;
      IsReseller = isReseller;
    }

    public bool IsReseller { get; set; }

    public string EmailAddress { get; set; }
    
    private readonly List<OptIn.Interface.OptIn> _optIns;
    public List<OptIn.Interface.OptIn> OptIns { get { return _optIns; } }

    public string UserHostAddress { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int EmailTypeId { get; set; }

    public int PrivateLabelId { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("OptInUpdateInfoRequest is not a cachable request.");
    }

    #endregion
  }
}
