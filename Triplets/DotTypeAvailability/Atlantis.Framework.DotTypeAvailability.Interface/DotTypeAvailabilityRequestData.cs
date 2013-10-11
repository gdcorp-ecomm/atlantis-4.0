using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeAvailability.Interface
{
  public class DotTypeAvailabilityRequestData : RequestData
  {
    public DotTypeAvailabilityRequestData()
    {
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string GetCacheMD5()
    {
      return "DotTypeAvailabilityRequestData";
    }
  }
}
