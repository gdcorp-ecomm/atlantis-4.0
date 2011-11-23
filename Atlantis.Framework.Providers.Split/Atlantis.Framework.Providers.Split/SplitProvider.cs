using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Split.Interface;

namespace Atlantis.Framework.Providers.Split
{
  public class SplitProvider : ProviderBase, ISplitProvider
  {
    Lazy<StandardSplitValue> _standardSplit;
    Lazy<PCSplitValue> _pcSplit;
    Lazy<ISiteContext> _siteContext;

    public SplitProvider(IProviderContainer container)
      : base(container)
    { 
      _siteContext = new Lazy<ISiteContext>(() =>
      {
        return Container.Resolve<ISiteContext>();
      });

      _standardSplit = new Lazy<StandardSplitValue>(() => 
      {
        return new StandardSplitValue(_siteContext.Value);
      });

      _pcSplit = new Lazy<PCSplitValue>(() =>
      {
        return new PCSplitValue(_siteContext.Value);
      });
    }

    public int SplitValue
    {
      get
      {
        return _standardSplit.Value.SplitValue;
      }
      set
      {
        _standardSplit.Value.SplitValue = value;
      }
    }

    public int PCSplitValue
    {
      get
      {
        return _pcSplit.Value.SplitValue;
      }
      set
      {
        _pcSplit.Value.SplitValue = value;
      }
    }

    private static string _splitCookieLifeAppSetting = "ATLANTIS_SPLITPROVIDER_COOKIELIFE_HOURS";
    public static string SplitCookieLifeAppsettingName
    {
      get
      {
        return _splitCookieLifeAppSetting;
      }
      set
      {
        _splitCookieLifeAppSetting = value;
      }
    }

  }
}
