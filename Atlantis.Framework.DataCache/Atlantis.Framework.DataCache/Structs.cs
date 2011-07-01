using System.Collections.Generic;

namespace Atlantis.Framework.DataCache
{
  class Structs
  {

    public struct GetCountryDataValues
    {
      public string Name;
      public string Abbreviation;
      public int CallingCode;
      public bool IsSupported;

      public GetCountryDataValues(string name,
                                  string abbreviation,
                                  int callingCode,
                                  bool isSupported)
      {
        Abbreviation = abbreviation;
        CallingCode = callingCode;
        IsSupported = isSupported;
        Name = name;
      }
    }

    public struct GetListPriceExValues
    {
      public int ListPrice;
      public bool IsEstimate;
      public bool ReturnValue;

      public GetListPriceExValues(int listPrice,
                                  bool isEstimate,
                                  bool returnValue)
      {
        IsEstimate = isEstimate;
        ListPrice = listPrice;
        ReturnValue = returnValue;
      }
    }

    public struct GetMaintNoticeValues
    {
      public bool NoticeIsPresent;
      public string NoticeHeader;
      public string NoticeBody;

      public GetMaintNoticeValues(bool noticeIsPresent,
                                  string noticeHeader,
                                  string noticeBody)
      {
        NoticeIsPresent = noticeIsPresent;
        NoticeHeader = noticeHeader;
        NoticeBody = noticeBody;
      }
    }

    public struct GetPromoPriceByQtyExValues
    {
      public int Price;
      public bool IsEstimate;
      public bool ReturnValue;

      public GetPromoPriceByQtyExValues(int price,
                                        bool isEstimate,
                                        bool returnValue)
      {
        IsEstimate = isEstimate;
        Price = price;
        ReturnValue = returnValue;
      }
    }

    public struct GetRelatedIDsForPrivateLabelValues
    {
      public int ParentPrivateLabelID;
      public int DefaultTurnkeyID;
      public int FreeTurnkeyID;

      public GetRelatedIDsForPrivateLabelValues(int iParentPrivateLabelID,
                                                int iDefaultTurnkeyID,
                                                int iFreeTurnkeyID)
      {
        ParentPrivateLabelID = iParentPrivateLabelID;
        DefaultTurnkeyID = iDefaultTurnkeyID;
        FreeTurnkeyID = iFreeTurnkeyID;
      }
    }

    public struct GetStateDataValues
    {
      public string Name;
      public string Abbreviation;
      public int CountryId;

      public GetStateDataValues(string name,
                                  string abbreviation,
                                  int countryId)
      {
        Name = name;
        Abbreviation = abbreviation;
        CountryId = countryId;
      }
    }

    public struct GetShopperRenewingServicesValues
    {
      public bool HasRenewingServices;
      public bool HasRenewingDomains;

      public GetShopperRenewingServicesValues(bool hasRenewingServices,
                                              bool hasRenewingDomains)
      {
        HasRenewingServices = hasRenewingServices;
        HasRenewingDomains = hasRenewingDomains;
      }
    }

    public struct GetMgrCategoriesForUserValues
    {
      public Dictionary<string, string> ManagerAttributes;
      public List<int> ManagerCategories;

      public GetMgrCategoriesForUserValues(Dictionary<string, string> managerAttributes,
                                           List<int> managerCategories)
      {
        ManagerAttributes = managerAttributes;
        ManagerCategories = managerCategories;
      }
    }

  }
}
