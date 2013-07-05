﻿using System;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.DomainSearch.Interface
{
  internal class FindResponseDomain : IFindResponseDomain
  {
    public FindResponseDomain(string sld, string tld, string domainName, JToken domainToken)
    {
      _responseDomain = new Domain(sld, tld, domainName);
      ParseDomainToken(domainToken);
    }

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
              VendorId = domainTokenValue;
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
                WeightRelevancePercentileRank = intValue;
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
          }
        }
      }
    }
    
    public double CommissionPercentage { get; private set; }

    public string VendorId { get; private set; }

    public string Language { get; private set; }

    public bool IsAvailable { get; private set; }
    
    public int Price { get; private set; }

    public bool IsBadWord { get; private set; }

    public bool HasDashes { get; private set; }

    public bool HasNumbers { get; private set; }

    public bool IsIdn { get; private set; }

    public bool IsBackOrderAvailable { get; private set; }

    public bool IsTypo { get; private set; }

    public bool IsDomainUsingSynonym { get; private set; }

    public DateTime AuctionEndTimeStamp { get; private set; }

    public DateTime LastUpdateTimeStamp { get; private set; }

    public int WeightRelevancePercentileRank { get; private set; }

    public int LengthOfSld { get; private set; }

    public int NumberOfKeywordsInDomain { get; private set; }
    
    public string AuctionId { get; private set; }

    public string AuctionType { get; private set; }

    public string CurrencyType { get; private set; }

    public string AvailCheckTypePerformed { get; private set; }

    public string DomainSearchDataBase { get; private set; }

    public string AuctionTypeId { get; private set; }

    private readonly IDomain _responseDomain;
    public IDomain Domain
    {
      get { return _responseDomain; }
    }
  }
}