using System;
using System.Diagnostics.CodeAnalysis;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Support;
using Atlantis.Framework.Providers.Support.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Localization;
using Atlantis.Framework.SupportApi.Interface.Models;

namespace Atlantis.Framework.TH.Support.Tests
{
  [ExcludeFromCodeCoverage]
  public class TestHelpers
  {
    public const string SUPPORT_TOKEN_KEY = "support";
    public const string SUPPORT_TOKEN_FORMAT = "[@T[support:{0}]@T]";
    
    //public const string HOURS_TOKEN_FORMAT = "[@T[support: <hours cityid={0} />]@T]";
    //public const string LOCATION_TOKEN_FORMAT = "[@T[support:<location cityid=1 />]@T]";

    public static IProviderContainer SetBasicContextAndProviders(int privateLabelId, string marketId)
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<ISupportProvider, SupportProvider>();
      result.RegisterProvider<ISupportContactProvider, SupportContactProvider>();
      result.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();
      result.RegisterProvider<SupportContactInfoDataCacheLoader, SupportContactInfoDataCacheLoader>();

      result.SetData(MockLocalizationProvider.MARKET_INFO, new MockMarketInfo(marketId, "DESC", true, "en-US"));
      result.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      return result;
    }

    public static IEnumerable<SupportContactInfo> GetTestContacts()
    {
      var contacts = new List<SupportContactInfo>
      {
        new SupportContactInfo
        {
          Type = "marketcontact",
          MarketId = "en-us",
          ContactType = "ContactType1",
          SupportType = "SupportType1",
          Value = "11111"
        },
        new SupportContactInfo
        {
          Type = "marketcontact",
          MarketId = "en-us",
          ContactType = "ContactType2",
          SupportType = "SupportType2",
          Value = "22222"
        },
        new SupportContactInfo
        {
          Type = "marketcontact",
          MarketId = "en-us",
          ContactType = "ContactType3",
          SupportType = "SupportType3",
          Value = "33333"
        },
        new SupportContactInfo
        {
          CityId = "A",
          CityDisplayName = "CityA",
          Type = "citycontact",
          MarketId = "en-us",
          ContactType = "ContactType4",
          SupportType = "SupportType4",
          Value = "44444"
        },
        new SupportContactInfo
        {
          CityId = "B",
          CityDisplayName = "CityB",
          Type = "citycontact",
          MarketId = "en-us",
          ContactType = "ContactType5",
          SupportType = "SupportType5",
          Value = "55555"
        },
        new SupportContactInfo
        {
          CityId = "C",
          CityDisplayName = "CityC",
          Type = "citycontact",
          MarketId = "en-us",
          ContactType = "ContactType6",
          SupportType = "SupportType6",
          Value = "66666"
        },
        new SupportContactInfo
        {
          CityId = "C",
          CityDisplayName = "CityC",
          Type = "citycontact",
          MarketId = "en-us",
          ContactType = "ContactType7",
          SupportType = "SupportType7",
          Value = "77777"
        },
        new SupportContactInfo
        {
          CityId = "C",
          CityDisplayName = "CityC",
          Type = "citycontact",
          MarketId = "en-us",
          ContactType = "ContactType8",
          SupportType = "SupportType2",
          Value = "828282"
        },
        new SupportContactInfo
        {
          CityId = "B",
          CityDisplayName = "CityB",
          Type = "citycontact",
          MarketId = "en-us",
          ContactType = "ContactType9",
          SupportType = "SupportType2",
          Value = "929292"
        },
        new SupportContactInfo
        {
          CityId = "A",
          CityDisplayName = "CityA",
          Type = "citycontact",
          MarketId = "en-us",
          ContactType = "ContactType10",
          SupportType = "SupportType2",
          Value = "102102102"
        }
      };
      return contacts;
    }    
  }
}

