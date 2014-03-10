
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Interface.Response
{
  [DataContract]
  public class LoginResult : MailApiResult
  {

    [DataMember]
    public string BaseUrl { get; set; }
  }
}
