
using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  [DataContract]
  public class LoginResult : ILoginResult
  {
    [DataMember]
    public bool IsMailApiFault { get; set; }

    [DataMember]
    public string Session { get; set; }

    [DataMember]
    public string BaseUrl { get; set; }
  }
}
