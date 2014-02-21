using System.Runtime.Serialization;

namespace Atlantis.Framework.MailApi.Interface
{
  [DataContract]
  public class MailApiResponseState // encaspulates the "state" portion of mailapi responses
  {
    [DataMember(Name = "app_key")]
    public string AppKey { get; set; }
  }
}
