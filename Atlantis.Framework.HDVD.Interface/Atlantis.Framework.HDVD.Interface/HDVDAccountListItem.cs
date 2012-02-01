using System.Runtime.Serialization;

namespace Atlantis.Framework.HDVD.Interface
{
  [DataContract]
  public class HDVDAccountListItem
  {
    [DataMember]
    public string AccountUID { get; set; }

    [DataMember]
    public string ServerName { get; set; }

    [DataMember]
    public string HostingType { get; set; }

    [DataMember]
    public string Status { get; set; }

    [DataMember]
    public string OperatingSystem { get; set; }
  }
}