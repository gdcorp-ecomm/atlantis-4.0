﻿using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MailApi.Interface
{
  public class GetMessageListRequestData : RequestData
  {
    public string FolderNum { get; private set; }
    public string Offset { get; private set; }
    public string Count { get; private set; }
    public string Filter { get; private set; }
    public string MailHash { get; private set; }
    public string MailBaseUrl { get; private set; }
    public string AppKey { get; private set; }

    public GetMessageListRequestData(string folderNum, string offset, string count, string filter, string mailHash, string mailBaseUrl, string appKey)
    {
      FolderNum = folderNum;
      Offset = offset;
      Count = count;
      Filter = filter;
      MailHash = mailHash;
      MailBaseUrl = mailBaseUrl;
      AppKey = appKey;
    }
  }
}
