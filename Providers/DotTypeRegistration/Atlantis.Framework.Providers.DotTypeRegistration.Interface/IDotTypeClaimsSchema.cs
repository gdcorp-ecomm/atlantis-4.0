namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeClaimsSchema
  {
    bool TryGetNoticeXmlByDomain(string domain, out string noticeXml);
    bool TryGetClaimsXmlByDomain(string domain, out string claimsXml);
  }
}
