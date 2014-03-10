
namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface ILoginResult : MailApiResult
  {
    bool IsMailApiFault { get; set; }

    string BaseUrl { get; set; }
  }
}
