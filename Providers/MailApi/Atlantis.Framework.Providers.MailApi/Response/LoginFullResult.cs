using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Response
{

  // CompositeLoginResponse is just a temp name for now
  // holds login data, folders list, message list


  [DataContract]
  public class LoginFullResult : ILoginFullResult
  {
    private List<IMessageHeader> msgList = new List<IMessageHeader>();

    [DataMember]
    public List<IMessageHeader> MessageHeaderList { get { return msgList; } set { msgList = value; } }

    private List<IFolder> folderList = new List<IFolder>();

    [DataMember]
    public List<IFolder> FolderList { get { return folderList; } set { folderList = value; } }

    [DataMember]
    public string BaseUrl { get; set; }

    [DataMember]
    public int ResultCode { get; set; }
    
    [DataMember]
    public bool IsMailApiFault { get; set; }
    
    [DataMember]
    public string Session { get; set; }
  }
}
