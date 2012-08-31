using System.Runtime.Serialization;

namespace Atlantis.Framework.ECCGetEmailPodDetails.Impl
{
  [DataContract]
  public class EccJsonGetEmailPodDetailsRequst
  {
    [DataMember(Name = "emailaddress")]
    public string EmailAddress { get; set; }
  }
}
