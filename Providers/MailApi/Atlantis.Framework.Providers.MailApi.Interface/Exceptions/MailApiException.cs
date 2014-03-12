using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Interface.Exceptions
{

  public class MailApiException : Exception
  {
    public string EmailAddress { get; set; }
    public string MailPod { get; set; }
    public string RequestJson { get; set; }
    public string ResponseJson { get; set; }
    public string Session { get; set; }


    public MailApiException(string message, string session, string emailAddress, string mailPod, string requestJson, string responseJson) : base(message)
    {
      EmailAddress = emailAddress;
      MailPod = mailPod;
      RequestJson = requestJson;
      ResponseJson = responseJson;
      Session = session;
    }

    public MailApiException(string message, string session, string emailAddress, string mailPod, string requestJson, string responseJson, Exception innerException)
      : base(message,innerException)
    {
      EmailAddress = emailAddress;
      MailPod = mailPod;
      RequestJson = requestJson;
      ResponseJson = responseJson;
    }


  }
}
