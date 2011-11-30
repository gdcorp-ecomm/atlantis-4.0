using System;
using Atlantis.Framework.FaxEmailAddonPacks.Interface.Types;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FaxEmailApplyAddonPack.Interface
{
  public class FaxEmailApplyAddonPackRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public string RequestedBy { get; private set; }
    public string Application { get; private set; }
    public string FteAccountUid { get; private set; }
    public int FteResourceId { get; private set; }
    public FaxEmailAddonPack FaxEmailAddonPack { get; private set; }

    public FaxEmailApplyAddonPackRequestData(string shopperId,
                                             string sourceUrl,
                                             string orderId,
                                             string pathway,
                                             int pageCount,
                                             int privateLabelId,
                                             string requestedBy,
                                             string application,
                                             string fteAccountUid,
                                             int fteResourceId,
                                             FaxEmailAddonPack faxEmailAddonPack)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Guid testValue;
      if (string.IsNullOrWhiteSpace(fteAccountUid) || !Guid.TryParse(fteAccountUid, out testValue))
        throw new ArgumentOutOfRangeException("fteAccountUid", "fteAccountUid is not a valid GUID string");
      if (fteResourceId <= 0)
        throw new ArgumentOutOfRangeException("fteResourceId");
      if (faxEmailAddonPack == null ||
          faxEmailAddonPack.ResourceId <= 0 ||
          faxEmailAddonPack.ExpireDate < DateTime.Today ||
          faxEmailAddonPack.PackDetails == null ||
          faxEmailAddonPack.PackDetails.Count == 0 ||
          !(faxEmailAddonPack.PackDetails.ContainsKey(FaxEmailAddonPack.Minutes) || faxEmailAddonPack.PackDetails.ContainsKey(FaxEmailAddonPack.Pages)))
        throw new ArgumentException("faxEmailAddonPack does not contain valid values", "faxEmailAddonPack");
      
      PrivateLabelId = privateLabelId;
      RequestedBy = requestedBy;
      Application = string.IsNullOrEmpty(application) ? "MYA" : application;
      FteAccountUid = fteAccountUid;
      FteResourceId = fteResourceId;
      FaxEmailAddonPack = faxEmailAddonPack;
      RequestTimeout = TimeSpan.FromSeconds(10d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in FaxEmailApplyAddonPackRequestData");
    }
  }
}
