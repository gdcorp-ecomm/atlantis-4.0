using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
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

    private HashSet<string> _offeredTldsSet;
    private HashSet<string> OfferedTLDsSet
    {
      get
      {
        if (_offeredTldsSet == null)
        {
          _offeredTldsSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

          if (OfferedTldsResponse != null && OfferedTldsResponse.OfferedTLDs.Any())
          {
            foreach (var tld in OfferedTldsResponse.OfferedTLDs)
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
      HashSet<string> aTlds = ActiveTldsResponse.GetActiveTLDIntersect(flagNames);

      return aTlds;
    }

    private IEnumerable<string> GetTLDsSetForAnyFlags(params string[] flagNames)
    {
      HashSet<string> aTlds = ActiveTldsResponse.GetActiveTLDUnion(flagNames);

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

          if (OfferedTldsResponse != null && OfferedTldsResponse.OfferedTLDs.Any())
          {
            _offeredTldsList = OfferedTldsResponse.OfferedTLDs.ToList();
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
