using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.BPBlogSubscriberQuery.Interface;
using Atlantis.Framework.DataProvider.Interface;
using Atlantis.Framework.GetShopper.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OptIn.Interface.Enums;
using Atlantis.Framework.OptInGetInfo.Interface;
using Atlantis.Framework.OptIn.Interface;

namespace Atlantis.Framework.OptInGetInfo.Impl
{
  class OptInGetInfoRequest : IRequest
  {
//    private int databaseQueriedOptInRangeStart = 0;
//    private int shopperRelatedOptInRangeStart = 100;
//    private int specializedOptInRangeStart = 200;
//    private int additionalOptInRangeStart = 300;


    #region Shopper Fields

    private static readonly IList<string> ShopperFields = new List<string> { 
                                                                Shopper.FK_DO_NOT_CALL,
                                                                Shopper.FK_DO_NOT_EMAIL, 
                                                                Shopper.FK_FIRST_NAME, 
                                                                Shopper.FK_LAST_NAME, 
                                                                Shopper.FK_MIDDLE_NAME, 
                                                                Shopper.FK_MKTG_EMAIL, 
                                                                Shopper.FK_MKTG_MAIL, 
                                                                Shopper.FK_MKTG_NONPROMO
                                                              };
    #endregion

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      OptInGetInfoResponseData result;
      var request = requestData as OptInGetInfoRequestData;

      var currentOptIns = new List<OptIn.Interface.OptIn>();
      List<OptIn.Interface.OptIn> workingOptIns;

      try
      {
        if (request != null)
        {
          //known shopper data
          if (request.OptIns.Where(
              (x => x == ((int)OptInPublicationTypes.NonPromotional) ||
                  x == ((int)OptInPublicationTypes.RelatedOffers) ||
                  x == ((int)OptInPublicationTypes.PostalCommunications) ||
                  x == ((int)OptInPublicationTypes.SmsCommunications) ||
                  x == ((int)OptInPublicationTypes.BusinessOffers) ||
                  x == ((int)OptInPublicationTypes.PhoneCommunications))

              ).Count() > 0)
          {
            //get shopper
            if (GetShopperMarketingPrefs(request.ShopperID, out workingOptIns, request.WebServiceTimeout))
            {
              if (workingOptIns != null && workingOptIns.Count > 0)
              {
                foreach (OptIn.Interface.OptIn item in workingOptIns)
                {
                  OptIn.Interface.OptIn item1 = item;
                  if (request.OptIns.Where(x => x == item1.OptInId).Count() > 0)
                  {
                    currentOptIns.Add(item);
                  }
                }
              }
            }
          }


          //known bobs blog
          if (request.OptIns.Where((x => x == ((int)OptInPublicationTypes.BobsBlog))).Count() > 0)
          {
            //get bobs blog status
            bool status;
            if (GetBPBlogSubscription(request.EmailAddress, out status, request.WebServiceTimeout))
            {
              currentOptIns.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BobsBlog, status, string.Empty));
            }
          }

          //get email optins - potential here for unknown items
          if (GetOptInEmailInfo(request.ShopperID, request.EmailAddress, request.PrivateLabelId, out workingOptIns, request.DatabaseTimeout))
          {
            if (workingOptIns != null && workingOptIns.Count > 0)
            {
              foreach (OptIn.Interface.OptIn item in workingOptIns)
              {
                OptIn.Interface.OptIn item1 = item;
                if (request.OptIns.Where((x => x == item1.OptInId)).Count() > 0)
                {
                  currentOptIns.Add(item);
                }
              }
            }
          }

          if (currentOptIns.Count < request.OptIns.Count())
          {
            FillMissingItems(request.OptIns, currentOptIns, out workingOptIns);
            if (workingOptIns != null && workingOptIns.Count > 0)
            {
              currentOptIns = workingOptIns;
            }
          }

        }
        result = new OptInGetInfoResponseData(currentOptIns);
      }
      catch (AtlantisException aex)
      {
        result = new OptInGetInfoResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new OptInGetInfoResponseData(requestData as OptInGetInfoRequestData, ex);
      }

      return result;
    }

    private static void FillMissingItems(IEnumerable<int> requestedOptIns ,IEnumerable<OptIn.Interface.OptIn> currentOptIns, out List<OptIn.Interface.OptIn> workingOptIns)
    {
      workingOptIns = new List<OptIn.Interface.OptIn>();
      //clone current set
      workingOptIns.AddRange(currentOptIns);

      //loop too see if there are requested options that were not included.
      
      foreach (int value in requestedOptIns)
      {
        int value1 = value;
        if (currentOptIns.Where(x => x.OptInId == value1).Count() == 0)
        {
          workingOptIns.Add(new OptIn.Interface.OptIn(value, false, string.Empty));
        }
      }
    }

    #endregion

    private static bool GetBPBlogSubscription(string emailAddress, out bool isActive, TimeSpan webServiceTimeout)
    {
      bool bSuccess = false;
      isActive = false;
      try
      {
        var request = new BPBlogSubscriberQueryRequestData(
          string.Empty, string.Empty, string.Empty, string.Empty, 0, emailAddress) {RequestTimeout = webServiceTimeout};

        var response =
          (BPBlogSubscriberQueryResponseData)
          Engine.Engine.ProcessRequest(request, OptInGetInfoEngineRequests.BobsBlogQuery);

        isActive = response.EmailExists;
        bSuccess = response.IsSuccess;
      }
      catch (Exception ex)
      {
        bSuccess = false;
      }
      return bSuccess;
    }

    private static bool GetShopperMarketingPrefs(string shopperId, out List<OptIn.Interface.OptIn> shoppePrefs, TimeSpan timeout)
    {
      bool bSuccess = false;
      Shopper shopperData = null;
      shoppePrefs = new List<OptIn.Interface.OptIn>();

      try
      {
        GetShopperResponseData getShopperResponseData = GetShopperFromFortKnox(shopperId, timeout);
        shopperData = new Shopper(getShopperResponseData);
        
        shoppePrefs.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.PostalCommunications,shopperData.MarketingStandardMailOptIn, string.Empty));
        shoppePrefs.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.NonPromotional, shopperData.MarketingNonPromotionalOptIn, string.Empty));
        shoppePrefs.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.BusinessOffers, shopperData.BusinessOffersOptIn, string.Empty));
        shoppePrefs.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.RelatedOffers, shopperData.MarketingEmailOptIn, string.Empty));
        shoppePrefs.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.SmsCommunications, shopperData.SmsOptIn, string.Empty));
        //Phone communications is opposite of the DoNotCallFlag! to indicate if we CAN call, not who we CANNOT call (which is stored in the shopper DNC flag) 
        shoppePrefs.Add(new OptIn.Interface.OptIn(OptInPublicationTypes.PhoneCommunications, !shopperData.HasDoNotCallSet, string.Empty));
        
        bSuccess = true;
      }
      catch (Exception ex)
      {
        bSuccess = false;
      }
      return bSuccess;
    }

    private static GetShopperResponseData GetShopperFromFortKnox(string shopperId, TimeSpan timeout)
    {
      var request = new GetShopperRequestData(shopperId,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                1);

      foreach (string shopperField in ShopperFields)
      {
        request.AddField(shopperField);
      }

      request.AddCommunicationPref(2); // SMS Prefs
      request.AddInterestPref(1, 1); //Business Offers

      request.RequestTimeout = timeout;

      return (GetShopperResponseData)Engine.Engine.ProcessRequest(request, OptInGetInfoEngineRequests.GetShopper);
    }

    private bool GetOptInEmailInfo(string shopperId, string emailAddress, int privateLabelId, out List<OptIn.Interface.OptIn> optIns, TimeSpan timeout)
    {
      optIns = null;

      const string optInEmailProc = "emailoptinget";
      string emailHash = HashEmail(emailAddress);

      var parameters = new Dictionary<string, object>(1);
      parameters["contactHash"] = emailHash;
      parameters["PrivateLabelId"] = privateLabelId;

      var request = new DataProviderRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, optInEmailProc, parameters);
      request.RequestTimeout = timeout;

      var response = (DataProviderResponseData)Engine.Engine.ProcessRequest(request, OptInGetInfoEngineRequests.DataProvider);
      var result = response.GetResponseObject() as DataSet;

      if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
      {
        optIns = new List<OptIn.Interface.OptIn>();

        DataTable dtOptIns = result.Tables[0];

        foreach (DataRow row in dtOptIns.Rows)
        {
          int mktgPublicationId = Convert.ToInt32(row["gdshop_mktg_publication_id"]);
          var currentOptIn = new OptIn.Interface.OptIn(mktgPublicationId, true, string.Empty);
          optIns.Add(currentOptIn);

          
          //4  Life Online Email
          //10 marketing email
          //11 marketing non promo email
          //12 marketing mail (snail mail) // 12 is a PLACEHOLDER!! GET THE RIGHT VALUE WHEN CREATED.
          //17 Special Offers, Discounts, Coupons
          //18 Online Presence
          //26 Blog Email
          //39 eNews News email
        }
      }
      return true;
    }

    public string HashEmail(string email)
    {
      if (email == string.Empty)
      {
        return string.Empty;
      }
      var managed = new SHA256Managed();
      var encoding = new ASCIIEncoding();

      byte[] buffer = managed.ComputeHash(encoding.GetBytes(email));
      byte[] bytes = new byte[0x40];

      const int num = 0x20;
      int num3 = num * 2;
      for (int i = num - 1; i >= 0; i--)
      {
        byte num4 = buffer[i];
        bytes[--num3] = (byte)((num4 % 10) + 0x61);
        bytes[--num3] = (byte)((num4 / 10) + 0x61);
      }
      return encoding.GetString(bytes);
    }

  }


  internal class Shopper
  {
    #region Shopper Fort Knox Fields

    internal const string FK_DO_NOT_CALL = "doNotCallFlag";
    internal const string FK_DO_NOT_EMAIL = "doNotEmailFlag";
    internal const string FK_MKTG_EMAIL = "mktg_email";
    internal const string FK_MKTG_MAIL = "mktg_mail";
    internal const string FK_MKTG_NONPROMO = "mktg_nonpromotional_notices";
    internal const string FK_FIRST_NAME = "first_name";
    internal const string FK_MIDDLE_NAME = "middle_name";
    internal const string FK_LAST_NAME = "last_name";

    #endregion

    #region Properties

    public bool HasDoNotCallSet { get; set; }
    public bool HasDoNotEmailFlag { get; set; }
    public bool MarketingEmailOptIn { get; set; }
    public bool MarketingStandardMailOptIn { get; set; }
    public bool MarketingNonPromotionalOptIn { get; set; }
    public bool SmsOptIn { get; set; }
    public bool BusinessOffersOptIn { get; set; }

    //[MaxLength(30)]
    public string ShopperId { get; set; }

    //[MaxLength(30)]
    public string FirstName { get; set; }

    //[MaxLength(20)]
    public string MiddleName { get; set; }

    //[MaxLength(50)]
    public string LastName { get; set; }



    #endregion

    internal Shopper(GetShopperResponseData getShopperResponseData)
    {
      if (getShopperResponseData.GetException() == null)
      {
        FirstName = getShopperResponseData.GetField(FK_FIRST_NAME);
        MiddleName = getShopperResponseData.GetField(FK_MIDDLE_NAME);
        LastName = getShopperResponseData.GetField(FK_LAST_NAME);

        int hasDoNotCall;
        int.TryParse(getShopperResponseData.GetField(FK_DO_NOT_CALL), out hasDoNotCall);
        HasDoNotCallSet = hasDoNotCall == 0 ? false : true;

        int hasDoNotEmail;
        int.TryParse(getShopperResponseData.GetField(FK_DO_NOT_EMAIL), out hasDoNotEmail);
        HasDoNotEmailFlag = hasDoNotEmail == 0 ? false : true;

        string emailOptIn = getShopperResponseData.GetField(FK_MKTG_EMAIL) ?? "no";
        MarketingEmailOptIn = emailOptIn == "yes" ? true : false;

        string standardMailOptIn = getShopperResponseData.GetField(FK_MKTG_MAIL) ?? "no";
        MarketingStandardMailOptIn = standardMailOptIn == "yes" ? true : false;

        string nonpromoOptIn = getShopperResponseData.GetField(FK_MKTG_NONPROMO) ?? "no";
        MarketingNonPromotionalOptIn = nonpromoOptIn == "yes" ? true : false;


        int smsOptIn;
        int.TryParse(getShopperResponseData.GetCommunicationPref(2).ToString(), out smsOptIn);
        SmsOptIn = smsOptIn == 1 ? true : false;

        int businessOfferOptIn;
        int.TryParse(getShopperResponseData.GetInterestPref(1, 1).ToString(), out businessOfferOptIn);
        BusinessOffersOptIn = businessOfferOptIn == 1 ? true : false;
      }
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("First Name = " + FirstName + "\r\n");
      sb.Append("Middle Name = " + MiddleName + "\r\n");
      sb.Append("Last Name = " + LastName + "\r\n");
      sb.Append("MarketingEmailOptIn = " + MarketingEmailOptIn + "\r\n");
      sb.Append("MarketingNonPromotionalOptIn = " + MarketingNonPromotionalOptIn + "\r\n");
      sb.Append("BusinessOffersOptIn = " + BusinessOffersOptIn + "\r\n");
      sb.Append("MarketingStandardMailOptIn = " + MarketingStandardMailOptIn + "\r\n");
      return sb.ToString();
    }
  }

}
