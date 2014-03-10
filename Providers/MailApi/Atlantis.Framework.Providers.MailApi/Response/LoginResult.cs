using Atlantis.Framework.Providers.MailApi.Interface.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.MailApi.Response
{
  class LoginResult : ILoginResult
  {
    public bool IsMailApiFault { get; set; }

    public string Session { get; set; }

    public string BaseUrl { get; set; }
  }
}
