
namespace Atlantis.Framework.Sso.Interface.JsonHelperClasses
{
  public abstract class AuthApiResponseBase
  {
    public string type { get; set; }
    public string id { get; set; }
    public string code { get; set; }
    public string message { get; set; }
    public string status { get; set; }

    protected AuthApiResponseBase()
    {
      type = string.Empty;
      id = string.Empty;
      code = string.Empty;
      message = string.Empty;
      status = string.Empty;
    }
  }
}
