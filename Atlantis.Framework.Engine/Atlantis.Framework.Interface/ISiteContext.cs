using System;
using System.Web;

namespace Atlantis.Framework.Interface
{
  public interface ISiteContext
  {
    int ContextId { get; }
    string StyleId { get; }
    int PrivateLabelId { get; }
    string ProgId { get; }

    HttpCookie NewCrossDomainCookie(string cookieName, DateTime expiration);
    HttpCookie NewCrossDomainMemCookie(string cookieName);

    int PageCount { get; }
    string Pathway { get; }
    string CI { get; }
    string CommissionJunctionStartDate { get; set; }
    string ISC { get; }

    string CurrencyType { get; }
    void SetCurrencyType(string currencyType);

    bool IsRequestInternal { get; }
    ServerLocationType ServerLocation { get; }
    IManagerContext Manager { get; }
  }
}
