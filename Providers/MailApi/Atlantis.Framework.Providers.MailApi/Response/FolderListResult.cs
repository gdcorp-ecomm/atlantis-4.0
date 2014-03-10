using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  [DataContract]
  public class FolderListResult : IFolderListResult
  {
    private  List<IFolder> folderList = new List<IFolder>();

    [DataMember]
    public List<IFolder> FolderList { get { return folderList; } }

    [DataMember]
    public int ResultCode { get; set; }

    [DataMember]
    public bool IsMailApiFault { get; set; }

    [DataMember]
    public string Session { get; set; }
  }
}
