﻿using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class ActiveTLDsResponseData : IResponseData
  {
    private AtlantisException _exception;
    private string _xmlData;

    private Dictionary<string, HashSet<string>> _tldSetsByActiveFlags;

    public static ActiveTLDsResponseData FromException(RequestData requestData, Exception ex)
    {
      return new ActiveTLDsResponseData(requestData, ex);
    }

    private ActiveTLDsResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "ActiveTLDsResponseData.ctor", message, inputData);
    }

    public static ActiveTLDsResponseData FromDataCacheElement(XElement dataCacheElement)
    {
      return new ActiveTLDsResponseData(dataCacheElement);
    }

    private ActiveTLDsResponseData(XElement dataCacheElement)
    {
      _xmlData = dataCacheElement.ToString();
      _tldSetsByActiveFlags = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

      foreach (XElement itemElement in dataCacheElement.Elements("item"))
      {
        try
        {
          string tld = itemElement.Attribute("tld").Value;

          foreach (XAttribute itemAtt in itemElement.Attributes())
          {
            string name = itemAtt.Name.ToString();
            if ((name != "tld") && (name != "tldid"))
            {
              if (itemAtt.Value == "1")
              {
                AddTldToFlagSet(name, tld);
              }
            }
          }

        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          AtlantisException aex = new AtlantisException("ActiveTLDsResponseData.ctor", "0", message, itemElement.ToString(), null, null);
          Engine.Engine.LogAtlantisException(aex);
        }

      }
    }

    private void AddTldToFlagSet(string name, string tld)
    {
      HashSet<string> tldSet;
      if (!_tldSetsByActiveFlags.TryGetValue(name, out tldSet))
      {
        tldSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        _tldSetsByActiveFlags[name] = tldSet;
      }
      tldSet.Add(tld);
    }

    public bool IsTLDActive(string tld, string flagName)
    {
      bool result = false;
      
      HashSet<string> tldSet;
      if (_tldSetsByActiveFlags.TryGetValue(flagName, out tldSet))
      {
        result = tldSet.Contains(tld);
      }

      return result;
    }

    public HashSet<string> GetActiveTLDUnion(params string[] flagNames)
    {
      HashSet<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      if (flagNames != null)
      {
        foreach (string flagName in flagNames)
        {
          HashSet<string> tldSetThatShouldNotBeChanged;
          if (_tldSetsByActiveFlags.TryGetValue(flagName, out tldSetThatShouldNotBeChanged))
          {
            result.UnionWith(tldSetThatShouldNotBeChanged);
          }
        }
      }

      return result;
    }

    public HashSet<string> GetActiveTLDIntersect(params string[] flagNames)
    {
      HashSet<string> result = null;

      if (flagNames != null)
      {
        foreach (string flagName in flagNames)
        {
          HashSet<string> tldSetThatShouldNotBeChanged;
          if (_tldSetsByActiveFlags.TryGetValue(flagName, out tldSetThatShouldNotBeChanged))
          {
            if (result == null)
            {
              result = new HashSet<string>(tldSetThatShouldNotBeChanged, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
              result.IntersectWith(tldSetThatShouldNotBeChanged);
            }
          }
          else
          {
            if (result == null)
            {
              result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            else
            {
              result.Clear();
            }
          }
        }
      }

      return result ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    public int GetActiveTLDCount(string flagName)
    {
      int result = 0;

      HashSet<string> tldSet;
      if (_tldSetsByActiveFlags.TryGetValue(flagName, out tldSet))
      {
        result = tldSet.Count;
      }

      return result;
    }

    public IEnumerable<string> AllFlagNames
    {
      get
      {
        return _tldSetsByActiveFlags.Keys;
      }
    }

    public string ToXML()
    {
      string result = "<exception/>";
      if (_xmlData != null)
      {
        result = _xmlData;
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}