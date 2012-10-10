using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.BPBlogSubscriberAdd.Interface;
using Atlantis.Framework.BPSubscribeRemove.Interface;
using Atlantis.Framework.DataProvider.Interface;
using Atlantis.Framework.EEMResellerOptIn.Interface;
using Atlantis.Framework.EEMResellerOptOut.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MktgSubscribeAdd.Interface;
using Atlantis.Framework.MktgSubscribeRemove.Interface;
using Atlantis.Framework.OptIn.Interface;
using Atlantis.Framework.OptIn.Interface.Enums;
using Atlantis.Framework.OptInUpdateInfo.Interface;
using Atlantis.Framework.UpdateShopper.Interface;
using Atlantis.Framework.MktgSetShopperCommPref.Interface;

namespace Atlantis.Framework.OptInUpdateInfo.Impl
{
  public class OptInUpdateInfoRequest : IRequest
  {

    #region Implementation of IRequest

    private List<Exception> _problems = new List<Exception>();

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var results = new Dictionary<string, bool>();

      OptInUpdateInfoResponseData response;

      try
      {
        //check each individually.  Will allow processing of partial sets.
        bool bSuccess;
        Exception currentException;
        try
        {
          bSuccess = SaveShopperOptIns((OptInUpdateInfoRequestData)requestData, out currentException);
          results.Add("Shopper", bSuccess);
          if (currentException != null)
          {
            _problems.Add(currentException);
          }
        }
        catch (Exception ex1)
        {
          _problems.Add(ex1);
        }

        try
        {
          bSuccess = SaveDatabaseOptIns((OptInUpdateInfoRequestData)requestData, out currentException);
          results.Add("Database", bSuccess);
          if (currentException != null)
          {
            _problems.Add(currentException);
          }
        }
        catch (Exception ex2)
        {
          _problems.Add(ex2);
        }

        try
        {
          bSuccess = AddTwitterOptin((OptInUpdateInfoRequestData)requestData, out currentException);
          results.Add("Twitter", bSuccess);
          if (currentException != null)
          {
            _problems.Add(currentException);
          }
        }
        catch (Exception exT)
        {
          _problems.Add(exT);
        }

        //try
        //{
        //  if (((OptInUpdateInfoRequestData)requestData).IsReseller)
        //  {
        //    bSuccess = SaveResellerOptIns((OptInUpdateInfoRequestData)requestData, out currentException);
        //    results.Add("Reseller", bSuccess);
        //    if (currentException != null)
        //    {
        //      _problems.Add(currentException);
        //    }
        //  }
        //}
        //catch (Exception ex3)
        //{
        //  _problems.Add(ex3);
        //}

        try
        {
          bSuccess = SaveBobsBlogOptIn((OptInUpdateInfoRequestData)requestData, out currentException);
          results.Add("Bobs Blog", bSuccess);
          if (currentException != null)
          {
            _problems.Add(currentException);
          }
        }
        catch (Exception ex4)
        {
          _problems.Add(ex4);
        }


        try
        {
          bSuccess = SaveWhoIsOptIn((OptInUpdateInfoRequestData)requestData, out currentException);
          results.Add("WhoIs", bSuccess);
          if (currentException != null)
          {
            _problems.Add(currentException);
          }
        }
        catch (Exception ex5)
        {
          _problems.Add(ex5);
        }

        response = new OptInUpdateInfoResponseData(results, _problems);

      }
      catch (AtlantisException aex)
      {
        _problems.Add(aex);
        response = new OptInUpdateInfoResponseData(requestData, _problems);
      }
      catch (Exception ex)
      {
        _problems.Add(ex);
        response = new OptInUpdateInfoResponseData(_problems);
      }

      return response;
    }

    private static bool SaveShopperOptIns(OptInUpdateInfoRequestData request, out Exception exception)
    {
      bool bSuccess;
      exception = null;
      var savedBy = string.Format("Cust-{0}", request.ShopperID);

      try
      {
        var fieldList = new List<UpdateShopperRequestData.UpdateField>();
        

        var businessOfferOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.BusinessOffers);
        var smsOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.SmsCommunications);
        var nonPromoOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.NonPromotional);
        var relatedOffersOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.RelatedOffers);
        var postalOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.PostalCommunications);
        var phoneOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.PhoneCommunications);



        if ((businessOfferOptIn != null && businessOfferOptIn.IsModified) ||
            (smsOptIn != null && smsOptIn.IsModified) ||
            (nonPromoOptIn != null && nonPromoOptIn.IsModified) ||
            (relatedOffersOptIn != null && relatedOffersOptIn.IsModified) ||
            (postalOptIn != null && postalOptIn.IsModified) ||
            (phoneOptIn != null && phoneOptIn.IsModified))
        {

          if  (relatedOffersOptIn != null && relatedOffersOptIn.IsModified)
          {
            fieldList.Add(new UpdateShopperRequestData.UpdateField("mktg_email", relatedOffersOptIn.Status ? "yes" : "no"));
          }

          if (postalOptIn != null && postalOptIn.IsModified)
          {
            fieldList.Add(new UpdateShopperRequestData.UpdateField("mktg_mail",
                                                                  postalOptIn.Status ? "yes" : "no"));
          }

          if (nonPromoOptIn != null && nonPromoOptIn.IsModified)
          {
            fieldList.Add(new UpdateShopperRequestData.UpdateField("mktg_nonpromotional_notices",
                                                                   nonPromoOptIn.Status ? "yes" : "no"));
          }

          //the phoneOptIn status contains true when shopper WANTS to be contacted via phone, FK_Shopper field holds true when shopper DOES NOT want 
          //phone contact so the opposite is used in this case
          if (phoneOptIn != null && phoneOptIn.IsModified)
          {
            fieldList.Add(new UpdateShopperRequestData.UpdateField("doNotCallFlag", phoneOptIn.Status ? "0":"1"));
          }

          var shopperRequest = new UpdateShopperRequestData(
            request.ShopperID,
            request.SourceURL,
            request.OrderID,
            request.Pathway,
            request.PageCount,
            savedBy,
            request.UserHostAddress,
            (fieldList.Count > 0 ? fieldList : new List<UpdateShopperRequestData.UpdateField>())
            ) {RequestedBy = savedBy};


          if (smsOptIn != null && smsOptIn.IsModified)
          {
            var commPrefs = new Hashtable();
            commPrefs.Add("CommTypeID", ((int)PreferenceIds.SmsCommunication).ToString());
            commPrefs.Add("OptIn", smsOptIn.Status ? "1" : "0");
            shopperRequest.AddPreference("Communication", commPrefs);
          }

          if ((businessOfferOptIn != null && businessOfferOptIn.IsModified))
          {
            var interestPrefs = new Hashtable();
            interestPrefs.Add("InterestTypeID", ((int)PreferenceIds.BusinessOffersInterest).ToString());
            interestPrefs.Add("CommTypeID", ((int)PreferenceIds.EmailCommunication).ToString());
            interestPrefs.Add("OptIn", businessOfferOptIn.Status ? "1" : "0");
            shopperRequest.AddPreference("Interest", interestPrefs);
          }
          var shopperUpdateResponse = (UpdateShopperResponseData)Engine.Engine.ProcessRequest(shopperRequest, OptInUpdateInfoEngineRequests.UpdateShopper);
          bSuccess = shopperUpdateResponse.IsSuccess;

        }
        else
        {
          //nothing to do, return true
          bSuccess = true;
        }
      }
      catch (Exception ex)
      {
        exception = ex;
        string message = "Error saving results: " + ex.Message + " " + ex.StackTrace;
        var aex = new AtlantisException(request, "OptInUpdateInfoRequest.SaveShopperOptIns", message, string.Empty, ex);
        Engine.Engine.LogAtlantisException(aex);
        bSuccess = false;
      }

      return bSuccess;
    }

    private static bool SaveDatabaseOptIns(OptInUpdateInfoRequestData request, out Exception exception)
    {
      bool bSuccess = false;
      exception = null;

      try
      {
        var savedBy = string.Format("Cust-{0}", request.ShopperID);
        //var savedBy = request.ShopperID;

        //call to dataprovider triplet, maybe.
        //call may be using the new addOptIn / removeOptIn stuff
        foreach (var optIn in request.OptIns)
        {
          if ((!IsKnownType(optIn) || IsDatabaseType(optIn)) && optIn.IsModified)
          {
            if (optIn.Status)
            {
              var mktgSubAddRequest = new MktgSubscribeAddRequestData(request.ShopperID, request.SourceURL,
                                                                      request.OrderID, request.Pathway,
                                                                      request.PageCount, request.EmailAddress,
                                                                      optIn.OptInId, request.PrivateLabelId,
                                                                      request.EmailTypeId, request.FirstName,
                                                                      request.LastName, true, savedBy, request.UserHostAddress)
                                                                      {
                                                                        RequestTimeout = request.RequestTimeout
                                                                      };

              var mktgSubAddResponse =
                (MktgSubscribeAddResponseData)
                Engine.Engine.ProcessRequest(mktgSubAddRequest, OptInUpdateInfoEngineRequests.MarketingSubscriptionAdd);
              bSuccess = mktgSubAddResponse.IsSuccess;
              exception = mktgSubAddResponse.GetException();
              if (mktgSubAddResponse.ToString().ToLower().Contains("error"))
              {
                exception = new Exception(mktgSubAddResponse.ToString());
              }
            }
            else
            {
              var mktgSubRemoveRequest = new MktgSubscribeRemoveRequestData(request.ShopperID, request.SourceURL,
                                                                            request.OrderID, request.Pathway,
                                                                            request.PageCount, request.EmailAddress,
                                                                            optIn.OptInId, request.PrivateLabelId,
                                                                            savedBy, request.UserHostAddress)
                                                                           {
                                                                             RequestTimeout = request.RequestTimeout
                                                                           };

              var mktgSubRemoveResponse =
                (MktgSubscribeRemoveResponseData)
                Engine.Engine.ProcessRequest(mktgSubRemoveRequest,
                                             OptInUpdateInfoEngineRequests.MarketingSubscriptionRemove);
              bSuccess = mktgSubRemoveResponse.IsSuccess;
              exception = mktgSubRemoveResponse.GetException();
              if (mktgSubRemoveResponse.ToString().ToLower().Contains("error"))
              {
                exception = new Exception(mktgSubRemoveResponse.ToString());
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        exception = ex;
        string message = "Error saving database results: " + ex.Message + " " + ex.StackTrace;
        var aex = new AtlantisException(request, "OptInUpdateInfoRequest.SaveDatabaseOptIns", message, string.Empty, ex);
        Engine.Engine.LogAtlantisException(aex);
        bSuccess = false;
      }

      return bSuccess;
    }

    private static bool AddTwitterOptin(OptInUpdateInfoRequestData request, out Exception exception)
    {
      bool bSuccess = true;
      exception = null;
      var twitterOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.Twitter);
      if ((twitterOptIn != null && twitterOptIn.IsModified))
      {
        bool isOpted = twitterOptIn.Status ? true : false;
        bSuccess = SaveCommunicationPreferenceOptin(request, (int)PreferenceIds.TwitterCommunication, isOpted, out exception);
      }
      return bSuccess;

    }

    private static bool SaveCommunicationPreferenceOptin(OptInUpdateInfoRequestData requestData, int optinId, bool isOptedIn, out Exception exception)
    {
      bool bSuccess = false;
      exception = null;
      try
      {
        MktgSetShopperCommPrefRequestData request = new MktgSetShopperCommPrefRequestData(requestData.ShopperID, requestData.SourceURL, requestData.OrderID, requestData.Pathway, requestData.PageCount, optinId, isOptedIn);
        MktgSetShopperCommPrefResponseData response = (MktgSetShopperCommPrefResponseData)Engine.Engine.ProcessRequest(request, OptInUpdateInfoEngineRequests.MktgSetShopperCommPreference);
        if (response.IsSuccess)
        {
          bSuccess = true;
        }
      }
      catch (Exception ex)
      {
        exception = ex;
        string message = "Error saving Comm pref: " + ex.Message + " " + ex.StackTrace;
        var aex = new AtlantisException(requestData, "OptInUpdateInfoRequest.SaveCommunicationPreferenceOptin", message, string.Empty, ex);
        Engine.Engine.LogAtlantisException(aex);
        bSuccess = false;
      }


      return bSuccess;
    }

    private static bool IsDatabaseType(OptIn.Interface.OptIn optIn)
    {
      const int minDbType = 0;
      const int maxDbType = 100;
      bool bStatus = false;

      if (optIn.OptInId > minDbType && optIn.OptInId < maxDbType)
      {
        bStatus = true;
      }

      return bStatus;
    }

    private static bool IsKnownType(OptIn.Interface.OptIn optIn)
    {
      bool bSuccess;

      try
      {
        Enum.Parse(typeof (OptInPublicationTypes), optIn.OptInId.ToString());
        bSuccess = true;
      }
      catch (Exception)
      {
        bSuccess = false;
      }
      
      
      return bSuccess;

    }

    /// <summary>
    /// Save the Reseller Opt In information.
    /// Should only be called for Private Label Context
    /// </summary>
    /// <param name="request"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private static bool SaveResellerOptIns(OptInUpdateInfoRequestData request, out Exception exception)
    {
      bool bSuccess = false;
      exception = null;
      bool isModified = false;
      bool relatedOfferOptInStatus = false;
      try
      {

        var relatedOfferOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.RelatedOffers);

        if (relatedOfferOptIn != null)
        {
          isModified = relatedOfferOptIn.IsModified;

          if (isModified)
          {
            relatedOfferOptInStatus = relatedOfferOptIn.Status;
            if (relatedOfferOptInStatus)
            {
              var resellerOptInRequest = new EEMResellerOptInRequestData(request.ShopperID, request.SourceURL,
                                                                         request.OrderID,
                                                                         request.Pathway, request.PageCount,
                                                                         request.PrivateLabelId, request.EmailTypeId,
                                                                         request.EmailAddress, request.FirstName,
                                                                         request.LastName) { RequestTimeout = request.RequestTimeout };

              var resellerOptInResponse =
                (EEMResellerOptInResponseData)
                Engine.Engine.ProcessRequest(resellerOptInRequest, OptInUpdateInfoEngineRequests.ResellerOptIn);
              bSuccess = resellerOptInResponse != null && resellerOptInResponse.IsSuccess;
            }
            else
            {
              var resellerOptOutRequest = new EEMResellerOptOutRequestData(request.ShopperID, request.SourceURL,
                                                                           request.OrderID,
                                                                           request.Pathway, request.PageCount,
                                                                           request.PrivateLabelId, request.EmailAddress) { RequestTimeout = request.RequestTimeout };

              var resellerOptOutResponse =
                (EEMResellerOptOutResponseData)
                Engine.Engine.ProcessRequest(resellerOptOutRequest, OptInUpdateInfoEngineRequests.ResellerOptOut);
              bSuccess = resellerOptOutResponse != null && resellerOptOutResponse.IsSuccess;
            }
          }
        }
      }
      catch (Exception ex)
      {
        exception = ex;
        string message = "Error saving reseller results: " + ex.Message + " " + ex.StackTrace;
        var aex = new AtlantisException(request, "OptInUpdateInfoRequest.SaveResellerOptIns", message, string.Format("isModified:{0}, relatedOfferOptInStatus:{1}", isModified, relatedOfferOptInStatus), ex);
        Engine.Engine.LogAtlantisException(aex);
        bSuccess = false;
      }

      return bSuccess;
    }

    private static bool SaveBobsBlogOptIn(OptInUpdateInfoRequestData request, out Exception exception)
    {
      bool bSuccess = false;
      exception = null;

      //call to BPBlog subcribe/unsubscribe triplet(s)
      OptIn.Interface.OptIn blogItem = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.BobsBlog);
      try
      {
        if (blogItem != null && blogItem.IsModified)
        {
          if (blogItem.Status)
          {
            //add        
            var blogSubscriptionRequest = new BPBlogSubscriberAddRequestData(
                                                                                request.ShopperID,
                                                                                request.SourceURL,
                                                                                request.OrderID,
                                                                                request.Pathway,
                                                                                request.PageCount,
                                                                                request.EmailAddress,
                                                                                request.FirstName,
                                                                                request.LastName,
                                                                                false) { RequestTimeout = request.RequestTimeout };

            var response = (BPBlogSubscriberAddResponseData)Engine.Engine.ProcessRequest(blogSubscriptionRequest, OptInUpdateInfoEngineRequests.BPSubscriptionAdd);
            bSuccess = response.IsSuccess;

          }
          else
          {
            //remove
            var blogRemoveSubscriptionRequest = new BPSubscribeRemoveRequestData(
                                                                                request.ShopperID,
                                                                                request.SourceURL,
                                                                                request.OrderID,
                                                                                request.Pathway,
                                                                                request.PageCount,
                                                                                request.EmailAddress) { RequestTimeout = request.RequestTimeout };

            var response = (BPSubscribeRemoveResponseData)Engine.Engine.ProcessRequest(blogRemoveSubscriptionRequest, OptInUpdateInfoEngineRequests.BPSubscriptionRemove);
            bSuccess = (response.IsSuccess && response.EmailRemoved);
          }
        }
      }
      catch (Exception ex)
      {
        exception = ex;
        string message = "Error saving bobs blog results: " + ex.Message + " " + ex.StackTrace;
        var aex = new AtlantisException(request, "OptInUpdateInfoRequest.SaveBobsBlogOptIn", message, string.Empty, ex);
        Engine.Engine.LogAtlantisException(aex);
        bSuccess = false;
      }

      return bSuccess;
    }

    private static bool SaveWhoIsOptIn(OptInUpdateInfoRequestData request, out Exception exception)
    {
      bool bSuccess;
      exception = null;

      try
      {
        OptIn.Interface.OptIn whoIsOptIn = request.OptIns.FirstOrDefault(x => x.Type == OptInPublicationTypes.WhoIsMailer);

        if (whoIsOptIn != null && whoIsOptIn.IsModified)
        {
          const string whoisOptInProc = "ActivateWhoIsMailer";
          string xml = "<whoisoptoutactivate><username>ssoweb</username><shoppers><shopper shopperid=\"{0}\" modifiedby=\"{1}\" ip=\"{2}\" comments=\"SSO: shopper_new bulk whois optout\" /></shoppers></whoisoptoutactivate>";
          xml = string.Format(xml, request.ShopperID, Environment.MachineName, request.UserHostAddress);

          var parameters = new Dictionary<string, object>(1);
          parameters["requestXml"] = xml;

          var whoIsRequest = new DataProviderRequestData(request.ShopperID, string.Empty, string.Empty,
                                                         string.Empty, 0, whoisOptInProc, parameters) { RequestTimeout = request.RequestTimeout };

          var whoIsResponse = (DataProviderResponseData)Engine.Engine.ProcessRequest(whoIsRequest, OptInUpdateInfoEngineRequests.DataProvider);

          whoIsResponse.GetResponseObject();

        }
        bSuccess = true;

      }
      catch (Exception ex)
      {
        exception = ex;
        string message = "Error saving results: " + ex.Message + " " + ex.StackTrace;
        var aex = new AtlantisException(request, "OptInUpdateInfoRequest.SaveWhoIsOptIn", message, string.Empty, ex);
        Engine.Engine.LogAtlantisException(aex);
        bSuccess = false;
      }
      return bSuccess;
    }

    #endregion
  }
}
