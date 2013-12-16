﻿using System.Collections.Generic;
using Atlantis.Framework.Providers.DomainsRAA.Interface.Items;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface
{
  public interface IDomainsRAAStatus
  {
    bool HasVerifiedResponseItems { get; }
    bool HasErrorCodes { get; set; }

    IEnumerable<IVerifiedResponseItem> VerifiedItems { get; }

    IEnumerable<DomainsRAAErrorCodes> ErrorCodes { get; }
  }
}
