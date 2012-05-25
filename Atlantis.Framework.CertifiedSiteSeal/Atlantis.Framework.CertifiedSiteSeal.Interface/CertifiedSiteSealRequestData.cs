using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.CertifiedSiteSeal.Interface
{
  public class CertifiedSiteSealRequestData : RequestData
  {
    private string _domain;
    private string _userID;
    private string _userPwd;
    private string _app;
    private int _timeout;

    public string Domain
    {
      get
      {
        return this._domain;
      }
    }

    public string UserID
    {
      get
      {
        return this._userID;
      }
    }

    public string UserPwd
    {
      get
      {
        return this._userPwd;
      }
    }

    public string App
    {
      get
      {
        return this._app;
      }
    }

    public int Timeout
    {
      get
      {
        return this._timeout;
      }
    }

    public CertifiedSiteSealRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string domain, string userID, string userPwd, string app, int timeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      this._domain = domain;
      this._userID = userID;
      this._userPwd = userPwd;
      this._app = app;
      this._timeout = timeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("CertifiedDomain is not a cacheable request.");
    }
  }
}
