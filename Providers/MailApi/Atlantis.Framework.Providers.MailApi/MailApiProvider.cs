using Atlantis.Framework.Interface;
using Atlantis.Framework.MailApi.Interface;

namespace Atlantis.Framework.Providers.MailApi
{
  public class MailApiProvider : ProviderBase, IMailApiProvider
  {
    public string RestrictedKey { get; set; }
    public string AppKey { get; set; }
    public string MailHash { get; set; }

    public MailApiProvider(IProviderContainer container)
      : base(container)
    {
    }

    public MailApiProvider(IProviderContainer container, string mailHash, string appKey, string restrictedKey) : base(container)
    {
      MailHash = mailHash;
      AppKey = appKey;
      RestrictedKey = restrictedKey;
    }

    public object Login(string username, string password, string appKey)
    {
      throw new System.NotImplementedException();
    }

    public object Login(string username, string password)
    {
      throw new System.NotImplementedException();
    }


    public MailFolder GetFolder(string folderNumber)
    {
      throw new System.NotImplementedException();
    }

    public object GetMessageList(string folderNumber, int offset, int count, string filter)
    {
      throw new System.NotImplementedException();
    }

    public object GetFolderList()
    {
      throw new System.NotImplementedException();
    }
  }
}
