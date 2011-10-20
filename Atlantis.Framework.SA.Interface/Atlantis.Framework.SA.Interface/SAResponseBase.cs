namespace Atlantis.Framework.SA.Interface
{
  public abstract class SAResponseBase
  {
    public string ShopperId { get; set; }
    public string ReturnCode { get; set; }
    public string ReturnMessage { get; set; }
  }
}
