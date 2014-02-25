using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class GetMessageListData
  {
    [DataMember(Name = "mail_headers")]
    public List<MailHeader> MailHeaderList { get; set; } 
  }
}
