
namespace Atlantis.Framework.Providers.Sso
{
  internal class SsoData
  {
    private string _loginUrl = string.Empty;
    private string _logoutUrl = string.Empty;
    private string _spkey = string.Empty;

    public string LoginUrl
    {
      get { return _loginUrl; }
      set { _loginUrl = value; }
    }

    public string LogoutUrl
    {
      get { return _logoutUrl; }
      set { _logoutUrl = value; }
    }

    public string SpKey
    {
      get { return _spkey; }
      set { _spkey = value; }
    }

  }
}
