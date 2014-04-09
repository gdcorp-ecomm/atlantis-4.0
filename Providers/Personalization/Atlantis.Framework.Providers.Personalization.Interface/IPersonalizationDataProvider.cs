using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Providers.Personalization.Interface
{
  public interface IPersonalizationDataProvider
  {
    Dictionary<string, string> GetChannelSessionData();
  }
}