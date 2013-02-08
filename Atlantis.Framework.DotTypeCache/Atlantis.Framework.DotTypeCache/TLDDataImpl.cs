using Atlantis.Framework.DotTypeCache.Interface;
using System;
using System.Collections.Generic;
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

    internal TLDDataImpl(int privateLabelId, OfferedTLDProductTypes productType)
    {
      _privateLabelId = privateLabelId;
      _productType = productType;
    }

    private ActiveTLDsResponseData _activeTldsResponse;
    private ActiveTLDsResponseData ActiveTldsResponse
    {
      get
      {
        if (_activeTldsResponse == null)
        {
          var aRequest = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
          var aResponse = (ActiveTLDsResponseData) DataCache.DataCache.GetProcessRequest(aRequest, _ACTIVETLDREQUEST);

          _activeTldsResponse = aResponse;
        }
        return _activeTldsResponse;

      }
    }

    private OfferedTLDsResponseData _offeredTldsResponse;
    private OfferedTLDsResponseData OfferedTldsResponse
    {
      get
      {
        if (_offeredTldsResponse == null)
        {
          var oRequest = new OfferedTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, _privateLabelId, _productType);
          var oResponse = (OfferedTLDsResponseData) DataCache.DataCache.GetProcessRequest(oRequest, _OFFEREDTLDREQUEST);
          _offeredTldsResponse =  oResponse;
        }
        return _offeredTldsResponse;
      }
    }

    public Dictionary<string, Dictionary<string, bool>> GetDiagnosticsOfferedTLDFlags(string[] tldNames = null)
    {
      tldNames = tldNames ?? new string[0];
      var tldInfo = new Dictionary<string, Dictionary<string, bool>>(1);

      var allFlags = ActiveTldsResponse.AllFlagNames.ToList();
      var oResponse = OfferedTldsResponse;

      var offeredTlds = new List<string>();
      if (tldNames.Length > 0)
      {
        foreach (var tld in tldNames)
        {
          if (oResponse.OfferedTLDs.Contains(tld, StringComparer.OrdinalIgnoreCase))
          {
            offeredTlds.Add(tld);
          }
        }
      }
      else
      {
        offeredTlds = oResponse.OfferedTLDs.ToList();
      }

      foreach (var offeredTld in offeredTlds)
      {
        var flagSets = new Dictionary<string, bool>();
        if (allFlags.Any())
        {
          foreach (var flag in allFlags)
          {
            flagSets.Add(flag, ActiveTldsResponse.IsTLDActive(offeredTld, flag));
          }
        }
        if (!tldInfo.ContainsKey(offeredTld))
        {
          tldInfo.Add(offeredTld, flagSets);
        }
      }

      return tldInfo;
    }

    private HashSet<string> _tldSet;
    private HashSet<string> TLDSet
    {
      get
      {
        if (_tldSet == null)
        {
          _tldSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

          if (OfferedTldsResponse != null && OfferedTldsResponse.OfferedTLDs.Any())
          {
            foreach (var tld in OfferedTldsResponse.OfferedTLDs)
            {
              if (DotTypeCache.HasDotTypeInfo(tld))
              {
                _tldSet.Add(tld);
              }
            }
          }
        }
        return _tldSet;
      }
    }

    public HashSet<string> GetTLDsSetForAllFlags(params string[] flagNames)
    {
      HashSet<string> aTlds = ActiveTldsResponse.GetActiveTLDIntersect(flagNames);

      return aTlds;
    }

    private HashSet<string> GetTLDsSetForAnyFlags(params string[] flagNames)
    {
      HashSet<string> aTlds = ActiveTldsResponse.GetActiveTLDUnion(flagNames);

      return aTlds;
    }

    public HashSet<string> GetOfferedTLDsSetForAllFlags(params string[] flagNames)
    {
      var returnTlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      foreach (var aTld in GetTLDsSetForAllFlags(flagNames))
      {
        if (TLDSet.Contains(aTld))
        {
          returnTlds.Add(aTld);
        }
      }

      return returnTlds;
    }

    public HashSet<string> GetOfferedTLDsSetForAnyFlags(params string[] flagNames)
    {
      var returnTlds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      foreach (var aTld in GetTLDsSetForAnyFlags(flagNames))
      {
        if (TLDSet.Contains(aTld))
        {
          returnTlds.Add(aTld);
        }
      }

      return returnTlds;
    }

    private List<string> _tldList; 
    public List<string> TLDList
    {
      get
      {
        if (_tldList == null)
        {
          _tldList = new List<string>();

          if (OfferedTldsResponse != null && OfferedTldsResponse.OfferedTLDs.Any())
          {
            foreach (var tld in OfferedTldsResponse.OfferedTLDs)
            {
              if (DotTypeCache.HasDotTypeInfo(tld))
              {
                _tldList.Add(tld);
              }
            }
          }
        }
        return _tldList;
        
      }
    }

    private List<string> GetOfferedTLDsListForAllFlags(params string[] flagNames)
    {
      HashSet<string> aTlds = ActiveTldsResponse.GetActiveTLDIntersect(flagNames);

      return OfferedTldsResponse.OfferedTLDs.Where(oTld => aTlds.Contains(oTld) && TLDSet.Contains(oTld)).ToList();
    }

    private List<string> GetOfferedTLDsListForAnyFlags(params string[] flagNames)
    {
      HashSet<string> aTlds = ActiveTldsResponse.GetActiveTLDUnion(flagNames);

      return OfferedTldsResponse.OfferedTLDs.Where(oTld => aTlds.Contains(oTld) && TLDSet.Contains(oTld)).ToList();
    }

    public List<string> GetCustomTLDsByGroupName(string groupName)
    {
      var tldList = new List<string>(0);
      var request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, groupName);
      var response = (CustomTLDGroupResponseData)DataCache.DataCache.GetProcessRequest(request, _CUSTOMTLDGROUPREQUEST);

      if (response != null && response.TldsInOrder.Any())
      {
        tldList = response.TldsInOrder.ToList();
      }

      return tldList;
    }

    private List<string> GetOfferedCustomTLDsByGroupName(string groupName)
    {
      var tldList = new List<string>(0);

      foreach (var tld in GetCustomTLDsByGroupName(groupName))
      {
        if (TLDSet.Contains(tld))
        {
          tldList.Add(tld);
        }
      }

      return tldList;
    }

    public List<string> FilterNonOfferedTLDs(List<string> tldListToFilter)
    {
      var list = new List<string>();

      if (tldListToFilter.Any())
      {
        var validTlds = GetAllOfferedTLDs();

        foreach (string tld in tldListToFilter)
        {
          if (validTlds.Contains(tld))
          {
            list.Add(tld);
          }
        }
      }
      return list;      
    }

    public HashSet<string> FilterNonOfferedTLDs(HashSet<string> tldHashSetToFilter)
    {
      var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      if (tldHashSetToFilter.Any())
      {
        var validTlds = GetAllOfferedTLDs();

        foreach (string tld in tldHashSetToFilter)
        {
          if (validTlds.Contains(tld))
          {
            set.Add(tld);
          }
        }
      }
      return set;      
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
        if (TLDSet.Contains(tld))
        {
          offeredTlds.Add(tld);
        }
      }
      var overRideTlds = new HashSet<string>(GetCustomTLDsByGroupName("OverRideTLDs"), StringComparer.OrdinalIgnoreCase);

      offeredTlds.UnionWith(overRideTlds);
      return offeredTlds;
    }
  }
}
