using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public interface IVerificationItems 
  {
    string RegistrationType { get; }

    string DomainId { get; }

    IList<IItem> Items { get;  }

    string VerfiedIp { get; }
  }
}
