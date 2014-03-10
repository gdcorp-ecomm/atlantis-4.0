namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface IFolder
  {
    string DisplayName { get; set; } 
    string FolderName { get; set; }
    int FolderNum { get; set; }
    int NumMessages { get; set; }
    int NumRead { get; set; }
    int UserNum { get; set; }
  }
}
