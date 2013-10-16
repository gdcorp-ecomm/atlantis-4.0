namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public class KeyData
  {
    public string kty { get; set; }
    public string alg { get; set; }
    public string kid { get; set; }
    public string n { get; set; }
    public string e { get; set; }

    public KeyData()
    {
      kty = string.Empty;
      alg = string.Empty;
      kid = string.Empty;
      n = string.Empty;
      e = string.Empty;
    }
  }
}