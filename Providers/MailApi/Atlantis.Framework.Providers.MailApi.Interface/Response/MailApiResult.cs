
namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  public interface MailApiResult
  {
    string Session { get; set; }

    bool IsMailApiFault { get; set; }
  }
}
