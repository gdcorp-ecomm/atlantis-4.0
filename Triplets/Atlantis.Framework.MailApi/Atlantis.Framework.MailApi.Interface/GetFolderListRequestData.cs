using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MailApi.Interface
{
  public class GetFolderListRequestData : RequestData
  {

    public GetFolderListRequestData(string session, string appKey, string key, string mailBaseUrl)
    {
      Session = session;
      AppKey = appKey;
      Key = key;
      MailBaseUrl = mailBaseUrl;
    }

    public string MailBaseUrl { get; set; }
    public string Session { get; set; }
    public string AppKey { get; set; }
    public string Key { get; set; }

  }
}
