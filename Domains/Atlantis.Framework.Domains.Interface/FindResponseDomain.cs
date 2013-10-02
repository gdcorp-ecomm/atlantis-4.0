﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Domains.Interface
{
  public class FindResponseDomain : IFindResponseDomain
  {
    public FindResponseDomain(string sld, string tld, string punyCodeSld, string punyCodeTld, JToken domainToken)
    {
      _responseDomain = new Domain(sld, tld, punyCodeSld, punyCodeTld);
      ParseDomainToken(domainToken);
    }

    private readonly Dictionary<string, string> _cartAttributes = new Dictionary<string, string>();

    private readonly Dictionary<string, string> _preRegPhrases = new Dictionary<string, string>();
    
    private void ParseDomainToken(JToken domainToken)
    {
      if (domainToken != null && domainToken.Any())
      {
        foreach (var domainTokenData in domainToken)
        {
          DateTime timeStamp;
          bool boolValue;
          int intValue;

          var domainTokenValue = domainTokenData["Data"].ToString();

          switch (domainTokenData["Name"].ToString())
          {
            case "vendorid":
              if (int.TryParse(domainTokenValue, out intValue))
              {
                VendorId = intValue;
              }
              break;
            case "commissionpct":
              double commissionPercentage;
              if (double.TryParse(domainTokenValue, out commissionPercentage))
              {
                CommissionPercentage = commissionPercentage;
              }
              break;
            case "price":
              if (int.TryParse(domainTokenValue, out intValue))
              {
                Price = intValue;
              }
              break;
            case "auctionid":
              AuctionId = domainTokenValue;
              break;
            case "auctiontypeid":
              AuctionTypeId = domainTokenValue;
              break;
            case "auctiontype":
              AuctionType = domainTokenValue;
              break;
            case "auctionendtime":
              if (DateTime.TryParse(domainTokenValue, out timeStamp))
              {
                AuctionEndTimeStamp = timeStamp;
              }
              break;
            case "lastupdatedatetime":
              if (DateTime.TryParse(domainTokenValue, out timeStamp))
              {
                LastUpdateTimeStamp = timeStamp;
              }
              break;
            case "database":
              DomainSearchDataBase = domainTokenValue;
              break;
            case "isbad":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                IsBadWord = boolValue;
              }
              break;
            case "databasepercentilerank":
              if (int.TryParse(domainTokenValue, out intValue))
              {
                DatabasePercentileRank = intValue;
              }
              break;
            case "percentilerank":
              if (int.TryParse(domainTokenValue, out intValue))
              {
                PercentileRank = intValue;
              }
              break;
            case "hasdash":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                HasDashes = boolValue;
              }
              break;
            case "hasnumbers":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                HasNumbers = boolValue;
              }
              break;
            case "isavailable":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                IsAvailable = boolValue;
              }
              break;
            case "isidn":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                IsIdn = boolValue;
              }
              break;
            case "language":
              Language = domainTokenValue;
              break;
            case "length":
              if (int.TryParse(domainTokenValue, out intValue))
              {
                LengthOfSld = intValue;
              }
              break;
            case "keyscount":
              if (int.TryParse(domainTokenValue, out intValue))
              {
                NumberOfKeywordsInDomain = intValue;
              }
              break;
            case "isbackorderavailable":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                IsBackOrderAvailable = boolValue;
              }
              break;
            case "currencytype":
              CurrencyType = domainTokenValue;
              break;
            case "istypo":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                IsTypo = boolValue;
              }
              break;
            case "issynonym":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                IsDomainUsingSynonym = boolValue;
              }
              break;
            case "availcheckstatus":
              AvailCheckTypePerformed = domainTokenValue;
              break;
            case "cartattributes":
              if (!string.IsNullOrEmpty(domainTokenValue))
              {
                var tokens = JsonConvert.DeserializeObject<JToken>(domainTokenValue);
                foreach (var token in tokens)
                {
                  _cartAttributes[token["Name"].ToString()] = token["Data"].ToString();
                }
              }
              break;
            case "isinternaltransfer":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                IsInternalTransfer = boolValue;
              }
              break;
            case "whoisexp":
              if (DateTime.TryParse(domainTokenValue, out timeStamp))
              {
                WhoIsExpiration = timeStamp;
              }
              break;
            case "idnscript":
              if (!string.IsNullOrEmpty(domainTokenValue))
              {
                var tokens = JsonConvert.DeserializeObject<JToken>(domainTokenValue);
                var token = tokens.First();

                if (token != null)
                {
                  IdnScript = token["Name"].ToString();
                  IdnScriptId = token["Data"].ToString();
                }
              }
              break;
            case "preregphase":
              if (!string.IsNullOrEmpty(domainTokenValue))
              {
                var tokens = JsonConvert.DeserializeObject<JToken>(domainTokenValue);
                foreach (var token in tokens)
                {
                  _preRegPhrases[token["Name"].ToString()] = token["Data"].ToString();
                }
              }
              break;
            case "hasleafpage":
              if (bool.TryParse(domainTokenValue, out boolValue))
              {
                HasLeafPage = boolValue;
              }
              break;
            case "pricefeatures":
              if (!string.IsNullOrEmpty(domainTokenValue))
              {
                var tokens = JsonConvert.DeserializeObject<JToken>(domainTokenValue);

                SetPriceFeatures(tokens);
              }
              break;
          }
        }
      }
    }

    private void SetPriceFeatures(IEnumerable<JToken> tokens)
    {
      foreach (var token in tokens)
      {
        int intValue;
        switch (token["Name"].ToString())
        {
          case "vendorcost":
            if (int.TryParse(token["Data"].ToString(), out intValue))
            {
              VendorCost = intValue;
            }
            break;
          case "vendortier":
            if (int.TryParse(token["Data"].ToString(), out intValue))
            {
              VendorTier = intValue;
            }
            break;
          case "internaltier":
            if (int.TryParse(token["Data"].ToString(), out intValue))
            {
              InternalTier = intValue;
            }
            break;
        }
      }
    }

    public string IdnScript { get; private set; }

    public string IdnScriptId { get; private set; }

    public double CommissionPercentage { get; private set; }

    public int VendorId { get; private set; }

    public string Language { get; private set; }

    public bool IsAvailable { get; private set; }

    public int Price { get; private set; }

    public bool IsBadWord { get; private set; }

    public bool HasDashes { get; private set; }

    public bool HasNumbers { get; private set; }

    public bool IsIdn { get; private set; }

    public bool IsBackOrderAvailable { get; private set; }

    public bool IsTypo { get; private set; }

    public bool IsInternalTransfer { get; private set; }

    public bool IsDomainUsingSynonym { get; private set; }

    public DateTime AuctionEndTimeStamp { get; private set; }

    public DateTime LastUpdateTimeStamp { get; private set; }

    /// <summary>
    /// If the domain is available for internal transfer, this is the Who Is expiration data that was present on the Avail Check call.
    /// </summary>
    public DateTime WhoIsExpiration { get; private set; }

    /// <summary>
    /// Represents a weight of relevance compared to the search phrase with in the requested database.  If present this value can be used for sorting based on the search relevancy.
    /// </summary>
    public int DatabasePercentileRank { get; private set; }

    /// <summary>
    /// Represents the overall weight of relevance compared to the search phrase.  If present this value can be used for sorting based on the search relevancy.
    /// </summary>
    public int PercentileRank { get; private set; }

    public int LengthOfSld { get; private set; }

    public int NumberOfKeywordsInDomain { get; private set; }

    public string AuctionId { get; private set; }

    public string AuctionType { get; private set; }

    public string CurrencyType { get; private set; }

    public string AvailCheckTypePerformed { get; private set; }

    public string DomainSearchDataBase { get; private set; }

    public string AuctionTypeId { get; private set; }

    /// <summary>
    /// List of attributes and values that should be passed on to the Cart for down stream systems use (example values: isoingo=true).
    /// </summary>
    public Dictionary<string, string> CartAttributes
    {
      get
      {
        return _cartAttributes;
      }
    }

    private readonly IDomain _responseDomain;

    public IDomain Domain
    {
      get { return _responseDomain; }
    }

    public bool IsPremium
    {
      get
      {
        return AuctionType == "premium";
      }
    }

    public bool IsAuction
    {
      get
      {
        return DomainSearchDataBase == "auctions";
      }
    }

    public bool IsCloseOutAuction
    {
      get
      {
        return AuctionType == "closeout";
      }
    }

    public bool IsPrivateSale
    {
      get
      {
        return DomainSearchDataBase == "private";
      }
    }

    private bool? _inWatchPhase;
    public bool InWatchPhase
    {
      get
      {
        if (_inWatchPhase == null)
        {
          _inWatchPhase = _preRegPhrases.Keys.Contains("wa");
        }

        return _inWatchPhase.Value;

      }
    }

    private bool? _inSunrisePhase;
    public bool InSunrisePhase
    {
      get
      {
        if (_inSunrisePhase == null)
        {
          _inSunrisePhase = _preRegPhrases.Keys.Contains("sr");
        }

        return _inSunrisePhase.Value;

      }
    }

    private bool? _inLandrushPhase;
    public bool InLandrushPhase
    {
      get
      {
        if (_inLandrushPhase == null)
        {
          _inLandrushPhase = _preRegPhrases.Keys.Contains("lr");
        }

        return _inLandrushPhase.Value;

      }
    }

    private bool? _inGeneralAvailabilityPhase;
    public bool InGeneralAvailabilityPhase
    {
      get
      {
        if (_inGeneralAvailabilityPhase == null)
        {
          _inGeneralAvailabilityPhase = _preRegPhrases.Count == 0 || _preRegPhrases.Keys.Contains("ga");
        }

        return _inGeneralAvailabilityPhase.Value;
      }
    }

    public bool HasLeafPage { get; private set; }

    public int VendorCost { get; private set; }

    public int VendorTier { get; private set; }

    public int InternalTier { get; private set; }
  }
}
