
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  [DataContract]
  public class FolderResult : MailApiResult
  {
    [DataMember]
    public Folder Folder { get; set; }

    [DataMember]
    public int ResultCode { get; set; }


  }
}
