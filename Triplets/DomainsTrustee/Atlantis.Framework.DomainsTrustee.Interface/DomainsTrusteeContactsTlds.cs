using System.Collections.Generic;

namespace Atlantis.Framework.DomainsTrustee.Interface
{
  public class DomainsTrusteeContactsTlds
  {
    public IList<DomainsTrusteeContact> Contacts { get; set; }
    public IList<string> Tlds { get; set; }

    public DomainsTrusteeContactsTlds(IList<DomainsTrusteeContact> contacts, IList<string> tlds)
    {
      Contacts = contacts;
      Tlds = tlds;
    }
  }
}
