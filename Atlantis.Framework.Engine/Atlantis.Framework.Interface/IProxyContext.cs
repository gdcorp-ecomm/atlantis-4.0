
namespace Atlantis.Framework.Interface
{
  public interface IProxyContext
  {
    ProxyStatusType Status { get; }

    bool IsLocalARR { get; }
    bool IsResellerDomain { get; }
    bool IsTransalationDomain { get; }

    string ResellerDomainUrl { get; }
    string ResellerDomainHost { get; }
    string ResellerDomainIP { get; }

    string ARRUrl { get; }
    string ARRHost { get; }
    string ARRIP { get; }

    string TranslationHost { get; }
    string TranslationPort { get; }
    string TranslationIP { get; }
    string TranslationLanguage { get; }

    string OriginIP { get; }
  }
}
