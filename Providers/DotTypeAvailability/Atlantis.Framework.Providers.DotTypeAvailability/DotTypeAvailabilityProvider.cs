using System;
using Atlantis.Framework.DotTypeAvailability.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeAvailability.Interface;

namespace Atlantis.Framework.Providers.DotTypeAvailability
{
  public class DotTypeAvailabilityProvider : ProviderBase, IDotTypeAvailabilityProvider
  {
    public DotTypeAvailabilityProvider(IProviderContainer container) : base(container)
    {
    }

    public bool HasLeafPage(string tldName)
    {
      bool hasLeafPage = false;
      ITldAvailability tldAvailability = GetTldAvailabilityData(tldName);

      if (tldAvailability != null && tldAvailability.HasLeafPage)
      {
        hasLeafPage = true;
      }

      return hasLeafPage;
    }

    private ITldAvailability GetTldAvailabilityData(string tldName)
    {
      ITldAvailability tldAvailability = null;

      try
      {
        var request = new DotTypeAvailabilityRequestData();
        var response = (DotTypeAvailabilityResponseData)DataCache.DataCache.GetProcessRequest(request, 753);

        if (response != null && response.TldAvailabilityDictionary != null)
        {
          response.TldAvailabilityDictionary.TryGetValue(tldName, out tldAvailability);
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeAvailabilityProvider.GetTldAvailabilityData(tldName)", "0", ex.Message + ex.StackTrace, tldName, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return tldAvailability;
    }
  }
}
