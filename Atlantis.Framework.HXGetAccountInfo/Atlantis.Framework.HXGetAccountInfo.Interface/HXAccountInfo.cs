
namespace Atlantis.Framework.HXGetAccountInfo.Interface
{
  public class HXAccountInfo
  {
    public int MailboxesTotal { get; set; }
    public int MailboxesAllocated { get; set; }
    public bool IsActiveSyncEnabled { get; set; }
    public bool IsBlackBerryEnabled { get; set; }
    public int DiskSpaceTotal { get; set; }
    public int DiskSpaceAllocated { get; set; }
    public int SharePointDiskSpaceTotal { get; set; }
    public int SharePointDiskSpaceAllocate { get; set; }
    public int EmailForwardsTotal { get; set; }
    public int EmailForwardsAllocated { get; set; }
    public int BlackBerryDevicesTotal { get; set; }
    public int BlackBerryDevicesAllocated { get; set; }
    public int SharePointSitesTotal { get; set; }
    public int SharePointSitesAllocated { get; set; }
  }
}
