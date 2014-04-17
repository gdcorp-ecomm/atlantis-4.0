using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Support.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Atlantis.Framework.Tokens.Interface;
using Atlantis.Framework.Providers.Support;


[assembly: InternalsVisibleTo("Atlantis.Framework.TH.Support.Tests")]
namespace Atlantis.Framework.TH.Support
{

  internal class SupportRenderContext
  {
    private readonly Lazy<ISupportContactProvider> _supportProvider;

    public SupportRenderContext(IProviderContainer container)
    {
      _supportProvider = new Lazy<ISupportContactProvider>(() => container.Resolve<ISupportContactProvider>());
    }
    private ISupportContactProvider SupportContactProvider
    {
      get
      {
        return _supportProvider.Value;
      }
    }

    public bool RenderToken(IToken token)
    {
      bool returnValue = false;

      SupportToken castToken = token as SupportToken;
      if (!ReferenceEquals(null, castToken) && !string.IsNullOrEmpty(castToken.RenderType))
      {
        ISupportLocation location = GetSupportLocation(castToken);
        if (!ReferenceEquals(null, location) && !ReferenceEquals(null, location.Contacts))
        {
          KeyValuePair<string, ISupportContact> firstContact = location.Contacts.FirstOrDefault();

          if (!ReferenceEquals(null, firstContact) && !ReferenceEquals(null, firstContact.Value))
          {
            token.TokenResult = firstContact.Value.Value;
          }
        }

        returnValue = !string.IsNullOrEmpty(token.TokenResult);
      }
      else
      {
        token.TokenError = "SupportToken contains invalid RenderType";
      }

      return returnValue;
    }

    private ISupportLocation GetSupportLocation(SupportToken token)
    {
      ISupportLocation returnValue = null;

      if (!ReferenceEquals(null, token))
      {
        IMarketSupportLocations marketLocations = SupportContactProvider.GetAllMarketSupportLocations(token.RenderType);
        if (!string.IsNullOrEmpty(token.CityId) && !ReferenceEquals(null, marketLocations.Cities) && marketLocations.Cities.ContainsKey(token.CityId))
        {
          returnValue = marketLocations.Cities[token.CityId];
        }

        ISupportLocation firstCitySupportLocation = null;
        if (!ReferenceEquals(null, marketLocations.Cities))
        {
          firstCitySupportLocation = marketLocations.Cities.FirstOrDefault().Value;
        }

        returnValue = returnValue ?? marketLocations.Market ?? firstCitySupportLocation;
      }

      return returnValue;
    }

  }
}
