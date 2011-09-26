using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Auction.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionSearchGetSaved.Interface
{
  public class AuctionSearchGetSavedResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisEx;
    private string _auctionReponseXml = string.Empty;

    public bool IsSuccess { get; private set; }

    public ReadOnlyCollection<SavedSearch> SavedSearches { get; private set; }
    public int TotalRecords { get; private set; }
    public string FriendlyErrorMessage { get; private set; }
    public string RawErrorMessage { get; private set; }
    public string ErrorNumber { get; private set; }

    public AuctionSearchGetSavedResponseData(string auctionXml)
    {
      if (!string.IsNullOrEmpty(auctionXml) && SetSavedSearches(auctionXml))
      {
        IsSuccess = true;
      }
      else
      {
        SavedSearches = new List<SavedSearch>(0).AsReadOnly();
        IsSuccess = false;
      }
    }
    
    public AuctionSearchGetSavedResponseData(RequestData requestData, Exception exception)
    {
      _atlantisEx = new AtlantisException(requestData,
                                   "AuctionSearchGetSavedResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    bool SetSavedSearches(string auctionResponse)
    {
      var savedSearches = new List<SavedSearch>();
      _auctionReponseXml = auctionResponse;
      var auctionsXml = XDocument.Parse(_auctionReponseXml);

      var success = false;

      var auctionSearchResponseElement = auctionsXml.Root;

      if (auctionSearchResponseElement != null)
      {
        var validAttribute = auctionSearchResponseElement.Attribute("Valid");
        if (validAttribute != null && validAttribute.Value == "false")
        {
          var rawErrorStringElementList = auctionSearchResponseElement.Descendants("RawErrorString");
          if (rawErrorStringElementList.Count() > 0)
          {
            var rawErrorStringElement = rawErrorStringElementList.First();
            RawErrorMessage = rawErrorStringElement.Value;
          }

          var errorDataElementList = auctionSearchResponseElement.Descendants("ErrorData");
          if (errorDataElementList.Count() > 0)
          {
            var errorDataElement = errorDataElementList.First();
            var errorNumber = errorDataElement.Attribute("errornumber");
            if (errorNumber != null)
            {
              ErrorNumber = errorNumber.Value;
            }
          }

          var friendlyErrorStringElementList = auctionSearchResponseElement.Descendants("FriendlyErrorString");
          if (friendlyErrorStringElementList.Count() > 0)
          {
            var friendlyErrorStringElement = friendlyErrorStringElementList.First();
            FriendlyErrorMessage = friendlyErrorStringElement.Value;
          }
        }
        else
        {
          var parsedTotalRecords = 0;
          var auctionElementList = auctionsXml.Descendants("SavedSearch");

          foreach (var auctionElement in auctionElementList)
          {

            var searchAttribute = auctionElement.Attribute("SearchName");
            if (searchAttribute != null)
            {
              savedSearches.Add(new SavedSearch(searchAttribute.Value));
              parsedTotalRecords++;
            }
          }

          TotalRecords = parsedTotalRecords;
          SavedSearches = savedSearches.AsReadOnly();
          success = true;
        }
      }
      return success;
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _auctionReponseXml;
    }

    public AtlantisException GetException()
    {
      return _atlantisEx;
    }

    #endregion

  }
}
