using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Collections.Generic;
namespace Atlantis.Framework.Providers.MailApi.Response
{

  // CompositeLoginResponse is just a temp name for now
  // holds login data, folders list, message list


  public class LoginFullResult : ILoginFullResult
  {
    private List<IMessageHeader> msgList = new List<IMessageHeader>();

    public List<IMessageHeader> MessageHeaderList { get { return msgList; } }

    private List<IFolder> folderList = new List<IFolder>();

    public List<IFolder> FolderList { get { return folderList; } }

    public string BaseUrl { get; set; }

    public int ResultCode { get; set; }
    public bool IsMailApiFault { get; set; }
    public string Session { get; set; }
  }
}
