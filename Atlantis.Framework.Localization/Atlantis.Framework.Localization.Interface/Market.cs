using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class Market : IMarket
  {
    internal Market(string id, string description, string msCulture, bool internalyOnly)
    {
      Id = id;
      Description = description;
      MsCulture = msCulture;
      IsInternalOnly = internalyOnly;
    }

    #region IMarket Members

    public string Id { get; private set; }

    public string Description { get; private set; }

    public string MsCulture { get; private set; }

    public bool IsInternalOnly { get; private set; }

    #endregion
  }
}
