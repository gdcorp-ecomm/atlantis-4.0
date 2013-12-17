using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public interface IRequestItem
  {
    IList<IItem> Items { get; }
  }
}
