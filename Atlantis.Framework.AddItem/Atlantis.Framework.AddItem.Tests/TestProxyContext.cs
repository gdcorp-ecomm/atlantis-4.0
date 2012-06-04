using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddItem.Tests
{
  public class TestProxyContext : ProviderBase, IProxyContext
  {
    public TestProxyContext(IProviderContainer container)
      : base(container)
    { }

    public ProxyStatusType Status
    {
      get { return ProxyStatusType.Valid; }
    }

    public bool IsLocalARR
    {
      get { return false; }
    }

    public bool IsResellerDomain
    {
      get { return false; }
    }

    public bool IsTransalationDomain
    {
      get { throw new NotImplementedException(); }
    }

    public string ResellerDomainUrl
    {
      get { throw new NotImplementedException(); }
    }

    public string ResellerDomainHost
    {
      get { throw new NotImplementedException(); }
    }

    public string ResellerDomainIP
    {
      get { throw new NotImplementedException(); }
    }

    public string ARRUrl
    {
      get { throw new NotImplementedException(); }
    }

    public string ARRHost
    {
      get { throw new NotImplementedException(); }
    }

    public string ARRIP
    {
      get { throw new NotImplementedException(); }
    }

    public string TranslationHost
    {
      get { throw new NotImplementedException(); }
    }

    public string TranslationPort
    {
      get { throw new NotImplementedException(); }
    }

    public string TranslationIP
    {
      get { throw new NotImplementedException(); }
    }

    public string TranslationLanguage
    {
      get { throw new NotImplementedException(); }
    }

    public string OriginIP
    {
      get 
      {
        string result = null;
        IPAddress[] addresses = Dns.GetHostAddresses(Environment.MachineName);
        if (addresses.Length > 0)
        {
          result = addresses[0].ToString();
        }
        return result;
      }
    }
  }
}
