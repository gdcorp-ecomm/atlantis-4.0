using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{

  // CompositeLoginResponse is just a temp name for now
  // holds login data, folders list, message list


  [DataContract]
  public class LoginFullResult : MailApiResult
  {
    private List<MessageHeader> msgList = new List<MessageHeader>();

    [DataMember]
    public List<MessageHeader> MessageHeaderList { get { return msgList; } set { msgList = value; } }

    private List<Folder> folderList = new List<Folder>();

    [DataMember]
    public List<Folder> FolderList { get { return folderList; } set { folderList = value; } }

    [DataMember]
    public string BaseUrl { get; set; }

    [DataMember]
    public int ResultCode { get; set; }
    
  }
}
