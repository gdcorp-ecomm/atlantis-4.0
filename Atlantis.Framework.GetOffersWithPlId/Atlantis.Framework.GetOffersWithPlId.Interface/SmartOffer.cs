using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace Atlantis.Framework.GetOffersWithPlId.Interface
{
  public class SmartOffer
  {
    #region Properties

    private string _shopperId;
    private int _privateLabelId;
    private string _offerId;
    private string _offerType;
    private string _discountType;
    private string _fbiOfferId;
    private string _fastBallOrderDiscount;
    private string _fastBallDiscount;
    private string _ciCode;
    private DateTime _offerStartDate;
    private DateTime _offerEndDate;
    private string _shortDescription;
    private string _longDescription;
    private string _deptId;
    private string _offerPage;
    private string _offerPageFull;
    private List<int> _eligiblePfIds;
    private List<int> _plProductGroups;
    private int _overallScore;
    private int _catalogId;

    public string ShopperId
    {
      [DebuggerStepThrough]
      get { return _shopperId; }    
    }

    public int PrivateLabelId
    {
      [DebuggerStepThrough]
      get { return _privateLabelId; }
      }

    public string OfferId
    {
      [DebuggerStepThrough]
      get { return _offerId; }      
    }

    public string OfferType
    {
      [DebuggerStepThrough]
      get { return _offerType; }      
    }

    public string DiscountType
    {
      [DebuggerStepThrough]
      get { return _discountType; }      
    }

    public string FbiOfferId
    {
      [DebuggerStepThrough]
      get { return _fbiOfferId; }      
    }

    public string FastBallOrderDiscount
    {
      [DebuggerStepThrough]
      get { return _fastBallOrderDiscount; }      
    }

    public string FastBallDiscount
    {
      [DebuggerStepThrough]
      get { return _fastBallDiscount; }      
    }

    public string CiCode
    {
      [DebuggerStepThrough]
      get { return _ciCode; }      
    }

    public DateTime OfferStartDate
    {
      [DebuggerStepThrough]
      get { return _offerStartDate; }      
    }

    public DateTime OfferEndDate
    {
      [DebuggerStepThrough]
      get { return _offerEndDate; }      
    }

    public string ShortDescription
    {
      [DebuggerStepThrough]
      get { return _shortDescription; }      
    }

    public string LongDescription
    {
      [DebuggerStepThrough]
      get { return _longDescription; }      
    }

    public string DeptId
    {
      [DebuggerStepThrough]
      get { return _deptId; }      
    }

    public string OfferPage
    {
      [DebuggerStepThrough]
      get { return _offerPage; }      
    }

    public string OfferPageFull
    {
      [DebuggerStepThrough]
      get { return _offerPageFull; }
    }

    public List<int> EligiblePfIds
    {
      [DebuggerStepThrough]
      get { return _eligiblePfIds; }      
    }

    public List<int> PlProductGroups
    {
      [DebuggerStepThrough]
      get { return _plProductGroups; }
    }


    public int CatalogId
    {
      [DebuggerStepThrough]
      get { return _catalogId;}
    }

    public int OverallScore
    {
      [DebuggerStepThrough]
      get { return _overallScore; } 
    }

    #endregion

    public SmartOffer( string shopperId
      , int privateLabelId
      , string offerId
      , string offerType
      , string discountType
      , string fbiOfferId
      , string fastBallOrderDiscount
      , string fastBallDiscount
      , string ciCode
      , DateTime offerStartDate
      , DateTime offerEndDate
      , string shortDescription
      , string longDescription
      , string deptId
      , string offerPage
      , string offerPageFull
      , List<int> eligiblePfIds
      , List<int> plProductGroups
      , int overallScore
      , int catalogId
    )
    {
       _shopperId  = shopperId;
       _privateLabelId  = privateLabelId;
       _offerId  = offerId;
       _offerType  = offerType;
       _discountType  = discountType;
       _fbiOfferId  = fbiOfferId;
       _fastBallOrderDiscount  = fastBallOrderDiscount;
       _fastBallDiscount  = fastBallDiscount;
       _ciCode  = ciCode;
       _offerStartDate  = offerStartDate;
       _offerEndDate  = offerEndDate;
       _shortDescription  = shortDescription;
       _longDescription  = longDescription;
       _deptId  = deptId;
       _offerPage  = offerPage;
       _offerPageFull  = offerPageFull;
       _eligiblePfIds  = eligiblePfIds;
       _plProductGroups = plProductGroups;
       _overallScore  = overallScore;
       _catalogId = catalogId;
    }

    public SmartOffer(XmlNode productOffer, XmlNamespaceManager xmlnsManager)
    {
      _offerId = productOffer.Attributes["offer_id"].InnerXml;
      _offerType = productOffer.SelectSingleNode("sm:offerType", xmlnsManager).InnerXml;
      _discountType = productOffer.SelectSingleNode("sm:discountType", xmlnsManager).InnerXml;
      _fbiOfferId = productOffer.SelectSingleNode("sm:fbiOfferId", xmlnsManager).InnerXml;
      _fastBallOrderDiscount = productOffer.SelectSingleNode("sm:fastballOrderDiscount", xmlnsManager).InnerXml;
      _fastBallDiscount = productOffer.SelectSingleNode("sm:fastballDiscount", xmlnsManager).InnerXml;
      _ciCode = productOffer.SelectSingleNode("sm:ciCode", xmlnsManager).InnerXml;

      DateTime.TryParse(productOffer.SelectSingleNode("sm:endDate", xmlnsManager).InnerXml, out _offerStartDate);
      DateTime.TryParse(productOffer.SelectSingleNode("sm:startDate", xmlnsManager).InnerXml, out _offerEndDate);

      _shortDescription = productOffer.SelectSingleNode("sm:shortDescription", xmlnsManager).InnerXml;
      _longDescription = productOffer.SelectSingleNode("sm:longDescription", xmlnsManager).InnerXml;
      _deptId = productOffer.SelectSingleNode("sm:dept_id", xmlnsManager).InnerXml;
      _offerPage = productOffer.SelectSingleNode("sm:offerPage", xmlnsManager).InnerXml;
      _offerPageFull = productOffer.SelectSingleNode("sm:offerPageFull", xmlnsManager).InnerXml;


      int groupId = 0;
      XmlNodeList plGroupNodeList = productOffer.SelectNodes("sm:pl_productGroups/sm:pl_productGroupID", xmlnsManager);
      _plProductGroups = new List<int>();

      foreach (XmlNode plGroup in plGroupNodeList)
      {
        int.TryParse(plGroup.InnerXml, out groupId);

        if (groupId > 0)
        {
          _plProductGroups.Add(groupId);
        }
      }

      int pfid = 0;
      XmlNodeList eligibleProducts = productOffer.SelectNodes("sm:eligibleProducts/sm:pf_id", xmlnsManager);
      _eligiblePfIds = new List<int>();

      foreach (XmlNode eligibleProduct in eligibleProducts)
      {
        int.TryParse(eligibleProduct.InnerXml, out pfid);

        if (pfid > 0)
        {
          _eligiblePfIds.Add(pfid);
        }
      }

      int.TryParse(productOffer.SelectSingleNode("sm:overallScore", xmlnsManager).InnerXml, out _overallScore);
      int.TryParse(productOffer.SelectSingleNode("sm:catalogId", xmlnsManager).InnerXml, out _catalogId);
    }
  }
}
