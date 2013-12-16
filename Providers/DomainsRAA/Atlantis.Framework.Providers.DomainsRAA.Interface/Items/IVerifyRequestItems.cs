using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface.Items
{
  public interface IVerifyRequestItems
  {
    string RegistrationType { get; }
    string DomainId { get; }
    IEnumerable<IVerifyRequestItem> Items { get; }
    DomainsRAAReasonCodes ReasonCode { get; }
  }
}
