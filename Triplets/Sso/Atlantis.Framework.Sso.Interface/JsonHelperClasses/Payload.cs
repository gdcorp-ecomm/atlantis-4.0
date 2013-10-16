
namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public class Payload
  {
    public string shopperId { get; set; }
    public string firstname { get; set; }
    public string plid { get; set; }
    public Factors factors { get; set; }
    public string exp { get; set; }
    public string iat { get; set; }
    public string jti { get; set; }
    public string typ { get; set; }

    public Payload()
    {
      shopperId = string.Empty;
      firstname = string.Empty;
      plid = string.Empty;
      factors = new Factors();
      exp = string.Empty;
      iat = string.Empty;
      jti = string.Empty;
      typ = string.Empty;
    }
  }

}


