using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MailApi.Interface
{
  public class GetFolderRequestData : RequestData
  {
    public GetFolderRequestData(string folderNumber, string session, string appKey, string key, string mailBaseUrl)
    {
      FolderNum = folderNumber;
      Session = session;
      AppKey = appKey;
      Key = key;
      MailBaseUrl = mailBaseUrl;
    }

    public string FolderNum { get; set; }
    public string Session { get; set; }
    public string AppKey { get; set; }
    public string Key { get; set; }
    public string MailBaseUrl  { get; set; }
  }
}
