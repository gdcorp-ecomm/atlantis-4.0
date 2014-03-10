using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  [DataContract]
  public class FolderListResult : MailApiResult
  {
    private  List<Folder> folderList = new List<Folder>();

    [DataMember]
    public List<Folder> FolderList { get { return folderList; } }

    [DataMember]
    public int ResultCode { get; set; }


  }
}
