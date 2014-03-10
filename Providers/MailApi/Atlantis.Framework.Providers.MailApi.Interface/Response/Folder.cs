
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  [DataContract]
  public class Folder
  {
    [DataMember]
    public string DisplayName { get; set; }

    [DataMember]
    public string FolderName { get; set; }

    [DataMember]
    public int NumMessages { get; set; }

    [DataMember]
    public int FolderNum { get; set; }

    [DataMember]
    public int NumRead { get; set; }

    [DataMember]
    public int UserNum { get; set; }
  }
}
