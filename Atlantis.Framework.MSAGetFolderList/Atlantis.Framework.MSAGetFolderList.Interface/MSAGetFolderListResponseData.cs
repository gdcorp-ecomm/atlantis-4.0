﻿using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MSA.Interface;

namespace Atlantis.Framework.MSAGetFolderList.Interface
{
  public class MSAGetFolderListResponseData : IResponseData
  {    
    AtlantisException _exception;
    bool _success = true;

    public List<MailFolder> MailFolders { get; set; }

    public MSAGetFolderListResponseData()
    {

    }

    public MSAGetFolderListResponseData(GetFolderListResponse response)
    {
      MailFolders = response.MailFolders;
    }
  

    public MSAGetFolderListResponseData(AtlantisException ex)
    {
      _exception = ex;
      _success = false;
    }

    #region IResponseData Members

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder("");
      return sb.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

  }
    #endregion
}
