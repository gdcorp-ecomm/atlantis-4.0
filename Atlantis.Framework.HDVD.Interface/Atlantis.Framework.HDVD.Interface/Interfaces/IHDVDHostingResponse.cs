namespace Atlantis.Framework.HDVD.Interface.Interfaces
{
  public interface IHDVDHostingResponse
  {
    int StatusCode { get; set; }
    string Message { get; set; }
    string Status { get; set; }
  }
}
