using System.Runtime.Serialization;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract]
  public class HCCAccount
  {
    public HCCAccount() { }

    [DataMember(Name = "exec")]
    public bool HasAccountExecutive { get; set; }

    [DataMember(Name = "uid")]
    public string AccountUid { get; set; }

    [DataMember(Name = "bw")]
    public int BandwidthInMb { get; set; }

    [DataMember(Name = "ds")]
    public int DiskspaceInMb { get; set; }

    [DataMember(Name = "dom")]
    public string Domain { get; set; }

    [DataMember(Name = "os")]
    public string OperatingSystem { get; set; }

    [DataMember(Name = "plan")]
    public string Plan { get; set; }

    [DataMember(Name = "stat")]
    public string Status { get; set; }
  }
}
