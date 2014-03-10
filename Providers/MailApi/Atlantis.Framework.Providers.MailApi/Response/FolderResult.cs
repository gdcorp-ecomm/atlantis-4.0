
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  [DataContract]
  public class FolderResult : IFolderResult
  {
    [DataMember]
    public IFolder Folder { get; set; }

    [DataMember]
    public int ResultCode { get; set; }

    [DataMember]
    public bool IsJsoapFault { get; set; }
  }
}
