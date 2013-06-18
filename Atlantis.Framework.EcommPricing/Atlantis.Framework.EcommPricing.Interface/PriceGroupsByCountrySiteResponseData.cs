using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Interface
{
  public class PriceGroupsByCountrySiteResponseData : IResponseData
  {
    private static PriceGroupsByCountrySiteResponseData _noMappingResponse;

    public static PriceGroupsByCountrySiteResponseData NoPriceGroupsMappingResponse
    {
      get { return _noMappingResponse; }
    }

    static PriceGroupsByCountrySiteResponseData()
    {
      _noMappingResponse = new PriceGroupsByCountrySiteResponseData();
    }

    private Dictionary<string, int> PriceGroupMapping
    {
      get;
      set;
    }

    private PriceGroupsByCountrySiteResponseData()
    {
      PriceGroupMapping = new Dictionary<string, int>();
    }    

    private PriceGroupsByCountrySiteResponseData(string mapping)
    {
      PriceGroupMapping = LoadPriceGroupMapping(mapping);
    }

    public static PriceGroupsByCountrySiteResponseData FromCountrySiteMapping(string mapping) 
    {
      if (string.IsNullOrWhiteSpace(mapping))
      {
        return PriceGroupsByCountrySiteResponseData.NoPriceGroupsMappingResponse;
      }
      else
      {
        return new PriceGroupsByCountrySiteResponseData(mapping);
      }
    }

    private Dictionary<string, int> LoadPriceGroupMapping(string priceGroupMapping)
    {
      Dictionary<string, int> result = new Dictionary<string, int>();

      if (!string.IsNullOrWhiteSpace(priceGroupMapping))
      {
        IEnumerable<string> countryMappings = priceGroupMapping.Split('|');
        foreach (string countryMapping in countryMappings)
        {
          string[] mapping = countryMapping.Split(':');
          if (mapping.Length == 2 && !string.IsNullOrWhiteSpace(mapping[0]))
          {
            string countrySiteId = mapping[0].ToUpper();
            int priceGroupId;
            if (int.TryParse(mapping[1], out priceGroupId))
            {
              result[countrySiteId] = priceGroupId;
            }
          }
        }
      }

      return result;
    }

    public int GetPriceGroupId(string countrySite)
    {
      int result = 0;
      if (!string.IsNullOrWhiteSpace(countrySite) && PriceGroupMapping != null && PriceGroupMapping.ContainsKey(countrySite.ToUpper()))
      {
        result = PriceGroupMapping[countrySite.ToUpper()];
      }
      return result;
    }

    #region IResponseData Members

    public string ToXML()
    {
      return String.Empty;
    }

    public AtlantisException GetException()
    {
      return null;
    }

    #endregion
  }
}
