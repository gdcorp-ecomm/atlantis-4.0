using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using Atlantis.Framework.DotTypeCache.Interface;
using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.TLDDataCache.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System.Linq;

namespace Atlantis.Framework.DotTypeCache
{
  public class TLDDataImpl : ITLDDataImpl
  {
    const int _OFFEREDTLDREQUEST = 637;
    const int _ACTIVETLDREQUEST = 635;
    const int _CUSTOMTLDGROUPREQUEST = 636;

    private readonly int _privateLabelId;
    private readonly OfferedTLDProductTypes _productType;
    private readonly Lazy<ITLDDataCacheProvider> _tldDataCacheProvider;

    internal TLDDataImpl(IProviderContainer container, int privateLabelId, OfferedTLDProductTypes productType)
    {
      _tldDataCacheProvider = new Lazy<ITLDDataCacheProvider>(container.Resolve<ITLDDataCacheProvider>);
      _privateLabelId = privateLabelId;
      _productType = productType;
    }

    private ITldsActive _tldsActive;
    private ITldsActive TldsActive
    {
      get
      {
        if (_tldsActive == null)
        {
          _tldsActive = _tldDataCacheProvider.Value.GetActiveTlds();
        }
        return _tldsActive;

      }
    }

    private ITldsOffered _tldsOffered;
    private ITldsOffered TldsOffered
    {
      get
      {
        if (_tldsOffered == null)
        {
          _tldsOffered = _tldDataCacheProvider.Value.GetOfferedTlds(_privateLabelId, _productType);
        }
        return _tldsOffered;
      }
    }

    public Dictionary<string, Dictionary<string, bool>> GetDiagnosticsOfferedTLDFlags(string[] tldNames = null)
    {
      tldNames = tldNames ?? new string[0];
      var tldInfo = new Dictionary<string, Dictionary<string, bool>>(1);

      var allFlags = new List<string>();

      ITldsOffered tldsOffered = null;
      var offeredTlds = new List<string>();
      if (_tldDataCacheProvider.Value != null)
      {
        if (TldsActive != null)
        {
          allFlags = TldsActive.AllFlagNames.ToList();
        }
      }

      if (tldNames.Length > 0)
      {
        if (TldsOffered != null)
        {
          foreach (var tld in tldNames)
          {
            if (TldsOffered.OfferedTLDs.Contains(tld, StringComparer.OrdinalIgnoreCase))
            {
              offeredTlds.Add(tld);
            }
          }
        }
      }
      else
      {
        if (TldsOffered != null)
        {
          offeredTlds = TldsOffered.OfferedTLDs.ToList();
        }
      }

      foreach (var offeredTld in offeredTlds)
      {
        var flagSets = new Dictionary<string, bool>();
        if (allFlags.Any())
        {
          if (TldsActive != null)
          {
            foreach (var flag in allFlags)
            {
              flagSets.Add(flag, TldsActive.IsTLDActive(offeredTld, flag));
            }
          }
        }
        if (!tldInfo.ContainsKey(offeredTld))
        {
          tldInfo.Add(offeredTld, flagSets);
        }
      }

      return tldInfo;
    }

    private HashSet<string> _offeredTldsSet;
    private HashSet<string> OfferedTLDsSet
    {
      get
      {
        if (_offeredTldsSet == null)
        {
          _offeredTldsSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

          if (TldsOffered != null && TldsOffered.OfferedTLDs.Any())
          {
            foreach (var tld in TldsOffered.OfferedTLDs)
            {
              _offeredTldsSet.Add(tld);
            }
          }
        }
        return _offeredTldsSet;
      }
    }

    public HashSet<string> GetTLDsSetForAllFlags(params string[] flagNames)
    {
      HashSet<string> aTlds = TldsActive.GetActiveTLDIntersect(flagNames);

      return aTlds;
    }

    private IEnumerable<string> GetTLDsSetForAnyFlags(params string[] flagNames)
    {
      HashSet<string> aTlds = TldsActive.GetActiveTLDUnion(flagNames);

      return aTlds;
    }

    public HashSet<string> GetOfferedTLDsSetForAllFlags(params string[] flagNames)
    {
      var returnTlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      var aTlds = GetTLDsSetForAllFlags(flagNames);
      foreach (var aTld in aTlds)
      {
        if (OfferedTLDsSet.Contains(aTld))
        {
          returnTlds.Add(aTld);
        }
      }

      return returnTlds;
    }

    public HashSet<string> GetOfferedTLDsSetForAnyFlags(params string[] flagNames)
    {
      var returnTlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      var aTlds = GetTLDsSetForAnyFlags(flagNames);
      foreach (var aTld in aTlds)
      {
        if (OfferedTLDsSet.Contains(aTld))
        {
          returnTlds.Add(aTld);
        }
      }

      return returnTlds;
    }

    private List<string> _offeredTldsList; 
    public List<string> OfferedTLDsList
    {
      get
      {
        if (_offeredTldsList == null)
        {
          _offeredTldsList = new List<string>();

          if (TldsOffered != null && TldsOffered.OfferedTLDs.Any())
          {
            _offeredTldsList = TldsOffered.OfferedTLDs.ToList();
          }
        }
        return _offeredTldsList;
      }
    }

    public List<string> GetCustomTLDsOfferedByGroupName(string groupName)
    {
      var tldList = new List<string>(0);
      var request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, groupName);
      var response = (CustomTLDGroupResponseData)DataCache.DataCache.GetProcessRequest(request, _CUSTOMTLDGROUPREQUEST);

      if (response != null && response.TldsInOrder.Any())
      {
        var validTlds = GetAllOfferedTLDs();

        foreach (var tld in response.TldsInOrder)
        {
          if (validTlds.Contains(tld))
          {
            tldList.Add(tld);
          }
        }
      }

      return tldList;
    }

    public bool IsOffered(string tld)
    {
      var validTlds = GetAllOfferedTLDs();
      return validTlds.Contains(tld);
    }

    private HashSet<string> GetAllOfferedTLDs()
    {
      var offeredTlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      var flagNames = new[] { "mainpricebox", "altpricebox" };
      var tlds = GetTLDsSetForAnyFlags(flagNames);

      foreach (var tld in tlds)
      {
        if (OfferedTLDsSet.Contains(tld))
        {
          offeredTlds.Add(tld);
        }
      }

      return offeredTlds;
    }
  }
}
