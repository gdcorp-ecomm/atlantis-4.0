namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public class Header
  {
    public string alg { get; set; }
    public string kid { get; set; }

    public Header()
    {
      alg = string.Empty;
      kid = string.Empty;
    }
  }
}