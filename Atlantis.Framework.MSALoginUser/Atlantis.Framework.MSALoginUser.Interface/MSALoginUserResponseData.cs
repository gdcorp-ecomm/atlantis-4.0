using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MSALoginUser.Interface
{
  public class MSALoginUserResponseData : IResponseData
  {
    AtlantisException _exception;
    bool _success = true;
    
    public string Hash { get; set; }
    
    public string BaseUrl { get; set; }

    public string ClientUrl { get; set; }

    public string Session { get; set; }

    public MSALoginUserResponseData()
    {

    }

    public MSALoginUserResponseData(LoginResponse loginResponse)
    {
      Hash = loginResponse.LoginData.Hash;
      BaseUrl = loginResponse.LoginData.BaseUrl;
      ClientUrl = loginResponse.LoginData.ClientUrl;      
    }

    public MSALoginUserResponseData(string hash, string baseUrl, string clientUrl, string session)
    {
      Hash = hash;
      BaseUrl = baseUrl;
      ClientUrl = clientUrl;
      Session = session;
    }

    public MSALoginUserResponseData(AtlantisException ex)
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
