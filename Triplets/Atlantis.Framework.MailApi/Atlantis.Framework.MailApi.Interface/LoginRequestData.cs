using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MailApi.Interface
{
  public class LoginRequestData : RequestData
  {
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string Appkey { get; private set; }

    // Sample constructor
    // Do not use the base constructor any more, create a constructor like this that requires only what you need
    public LoginRequestData(string username, string password, string appKey)
    {
      Username = username;
      Password = password;
      Appkey = appKey;
    }


  }
}
