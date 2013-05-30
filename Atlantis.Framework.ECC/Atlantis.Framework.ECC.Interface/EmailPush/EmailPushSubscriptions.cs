using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Atlantis.Framework.Ecc.Interface
{
  [CollectionDataContract]
  public class EmailPushSubscriptions : Collection<EmailPushSubscriptionItem>
  {
  }
}
