using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.BizRegDomain.Interface;
using Atlantis.Framework.BizRegDomain.Impl.BizRegWS;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BizRegDomain.Impl
{
  public class BizRegDomainRequest : IRequest
  {
    #region IRequest members
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      BizRegDomainResponseData oResponseData = null;

      try
      {
        BizRegDomainRequestData request = (BizRegDomainRequestData)oRequestData;
        BusinessRegistration service = new BusinessRegistration();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = request.Timeout;
        BizRegWS.BusinessDTO businessDTO = service.GetBusiness(request.DomainName);

        LocalBusiness bd = new LocalBusiness();

        bd.BusinessID = businessDTO.BusinessID;
        bd.GDShopperID = businessDTO.GDShopperID;
        bd.BusinessName = businessDTO.BusinessName;
        bd.BusinessDescription = businessDTO.BusinessDescription;
        bd.BusinessRegistrationTypeCode = businessDTO.BusinessRegistrationTypeCode;
        bd.BusinessRegistrationName = businessDTO.BusinessRegistrationName;
        bd.BusinessRatingTypeID = businessDTO.BusinessRatingTypeID;
        bd.BusinessRatingName = businessDTO.BusinessRatingName;
        bd.IsReviewEnabled = businessDTO.IsReviewEnabled;
        bd.IsFlagEnabled = businessDTO.IsFlagEnabled;
        bd.IsRatingEnabled = businessDTO.IsRatingEnabled;
        bd.ReviewCount = businessDTO.ReviewCount;
        bd.FavoriteCount = businessDTO.FavoriteCount;
        bd.FlagCount = businessDTO.FlagCount;
        bd.ViewCount = businessDTO.ViewCount;
        bd.BusinessStatusCode = businessDTO.BusinessStatusCode;
        bd.BusinessStatus = businessDTO.BusinessStatus;
        bd.IsFlaggedByUserID = businessDTO.IsFlaggedByUserID;
        bd.BusinessStatusByUserID = businessDTO.BusinessStatusByUserID;
        bd.ModifyByUserID = businessDTO.ModifyByUserID;
        bd.IsUpgrade = businessDTO.IsUpgrade;
        if (businessDTO.Mode != null)
          bd.Mode = businessDTO.Mode.ToString();
        bd.MarketplaceShopID = businessDTO.MarketplaceShopID;
        bd.MarketplaceStatusMask = businessDTO.MarketplaceStatusMask;
        bd.IsNotPublic = businessDTO.IsNotPublic;
        bd.IsPersonal = businessDTO.IsPersonal;


        if (businessDTO.Categories != null)
          foreach (string catPath in businessDTO.Categories)
            bd.CategoryPaths.Add(catPath);

        if (businessDTO.Tags != null)
          foreach (BizRegWS.BusinessTag bt in businessDTO.Tags)
            bd.Tags.Add(bt.BusinessTagID);

        if (businessDTO.BizData != null)
          foreach (BizRegWS.BizDataExtended bt in businessDTO.BizData)
          {
            Interface.BusinessData bizData = new Interface.BusinessData();
            bizData.BusinessDataID = bt.BusinessDataID;
            bizData.BusinessID = bt.BusinessID;
            bizData.DataType = bt.DataType;
            bizData.DisplayText = bt.DisplayText;
            bizData.URL = bt.URL;
            bizData.AltText = bt.AltText;
            bizData.Position = bt.Position;
            bizData.ModifyByUserID = bt.ModifyByUserID;
            bizData.IsImageAvailable = bt.IsImageAvailable;
            bizData.ImageFile = bt.ImageFile;
            bizData.ImagePath = bt.ImagePath;
            bizData.ImageUrl = bt.ImageURL;
            bizData.ImageHeight = bt.ImageHeight;
            bizData.ImageWidth = bt.ImageWidth;
            bd.BizData.Add(bizData);
          }

        Interface.BusinessFlag bf = new Interface.BusinessFlag();
        if (businessDTO.BusinessFlag != null)
        {
          bf.BusinessFlagID = businessDTO.BusinessFlag.BusinessFlagID;
          bf.UserID = businessDTO.BusinessFlag.UserID;
          bf.BusinessID = businessDTO.BusinessFlag.BusinessID;
          bf.UserScreenName = businessDTO.BusinessFlag.UserScreenName;
          bf.IsUserAvatarAvailable = businessDTO.BusinessFlag.IsUserAvatarAvailable;
          bf.FlagTypeID = businessDTO.BusinessFlag.FlagTypeID;
          bf.FlagType = businessDTO.BusinessFlag.FlagType;
          bf.FlagReason = businessDTO.BusinessFlag.FlagReason;
          bf.IsCleared = businessDTO.BusinessFlag.IsCleared;
          bf.ModeratedByUserID = businessDTO.BusinessFlag.ModeratedByUserID;
          bf.ModeratedByUserScreenName = businessDTO.BusinessFlag.ModeratedByUserScreenName;
          bf.ModeratedByManagerUserName = businessDTO.BusinessFlag.ModeratedByManagerUserName;
          bf.IsModeratedByUserAvatarAvailable = businessDTO.BusinessFlag.IsModeratedByUserAvatarAvailable;
          bf.ModeratedDateString = businessDTO.BusinessFlag.ModeratedDateString;
          bf.CreateDateString = businessDTO.BusinessFlag.CreateDateString;
          bf.ModifyByUserID = businessDTO.BusinessFlag.ModifyByUserID;
          bf.ModifyDateString = businessDTO.BusinessFlag.ModifyDateString;
        }
        bd.Flag = bf;

        Interface.BusinessLocation bl = new Interface.BusinessLocation();
        if (businessDTO.Location != null)
        {
          bl.BusinessLocationID = businessDTO.Location.BusinessLocationID;
          bl.BusinessID = businessDTO.Location.BusinessID;
          bl.IsHQ = businessDTO.Location.IsHQ;
          bl.AddressLine1 = businessDTO.Location.AddressLine1;
          bl.AddressLine2 = businessDTO.Location.AddressLine2;
          bl.City = businessDTO.Location.City;
          bl.State = businessDTO.Location.State;
          bl.Country = businessDTO.Location.Country;
          bl.CountryCode = businessDTO.Location.CountryCode;
          bl.PostalCode = businessDTO.Location.PostalCode;
          bl.PhoneNumber1 = businessDTO.Location.PhoneNumber1;
          bl.Phone1CallingCode = businessDTO.Location.Phone1CallingCode;
          bl.PhoneNumber2 = businessDTO.Location.PhoneNumber2;
          bl.Phone2CallingCode = businessDTO.Location.Phone2CallingCode;
          bl.FaxNumber = businessDTO.Location.FaxNumber;
          bl.FaxCallingCode = businessDTO.Location.FaxCallingCode;
          bl.Email = businessDTO.Location.Email;
          bl.WebSiteURL = businessDTO.Location.WebSiteURL;
          bl.Latitude = businessDTO.Location.Latitude;
          bl.Longitude = businessDTO.Location.Longitude;
          bl.BusinessHours = businessDTO.Location.BusinessHours;
          bl.IsShowMap = businessDTO.Location.IsShowMap;
          bl.ModifyByUserID = businessDTO.Location.ModifyByUserID;
        }
        bd.Location = bl;

        Interface.BusinessRating br = new Interface.BusinessRating();
        if (businessDTO.Rating != null)
        {
          br.BusinessRatingID = businessDTO.Rating.BusinessRatingID;
          br.BusinessID = businessDTO.Rating.BusinessID;
          br.BusinessRatingTypeID = businessDTO.Rating.BusinessRatingTypeID;
          br.BusinessRatingType = businessDTO.Rating.BusinessRatingType;
          br.RatingVotes = businessDTO.Rating.RatingVotes;
          br.TotalRating = businessDTO.Rating.TotalRating;
        }
        bd.Rating = br;

        Interface.BusinessDomain busDomain = new Interface.BusinessDomain();
        if (businessDTO.Domain != null)
        {
          busDomain.BusinessDomainID = businessDTO.Domain.BusinessDomainID;
          busDomain.BusinessID = businessDTO.Domain.BusinessID;
          busDomain.DomainID = businessDTO.Domain.DomainID;
          busDomain.PrivateLabelID = businessDTO.Domain.PrivateLabelID;
          busDomain.DomainName = businessDTO.Domain.DomainName;
          busDomain.TopLevelDomain = businessDTO.Domain.TopLevelDomain;
          busDomain.SecondLevelDomain = businessDTO.Domain.SecondLevelDomain;
          busDomain.CreateDateString = businessDTO.Domain.CreateDateString;
        }
        bd.Domain = busDomain;


        oResponseData = new BizRegDomainResponseData(bd);

      }
      catch (Exception ex)
      {
        oResponseData = new BizRegDomainResponseData(oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}