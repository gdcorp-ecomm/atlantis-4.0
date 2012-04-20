namespace Atlantis.Framework.Google
{

  public class requestBody
  {
    public string verificationMethod { get; set; }
    public site site { get; set; }

  }

  public class site
  {
    public string identifier { get; set; }
    public string type { get; set; }
  }


  public class auth
  {
    public string access_token { get; set; }
    public string token_type { get; set; }
    public string expires_in { get; set; }
  }


  //{"method":"DNS","token":"google-site-verification=di-FOpgk9U3cEAcNWE74UnoEVcoU0fENYZNCV3eJjSA"}
  public class verification
  {
    public string method { get; set; }
    public string token { get; set; }
  }

  public class webresource
  {
    public string id { get; set; }
    public site site { get; set; }
    public string[] owners { get; set; }
  }

}