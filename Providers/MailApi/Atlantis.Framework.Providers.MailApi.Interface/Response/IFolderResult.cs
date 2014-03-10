
namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface IFolderResult
  {
    IFolder Folder { get; set; }

    int ResultCode { get; set; }

    bool IsJsoapFault { get; set; }
  }
}
