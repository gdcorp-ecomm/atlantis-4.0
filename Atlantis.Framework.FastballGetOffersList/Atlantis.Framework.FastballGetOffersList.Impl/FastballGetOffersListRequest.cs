using Atlantis.Framework.Interface;
using System.Collections.Generic;
using Atlantis.Framework.FastballGetOffersList.Interface;

namespace Atlantis.Framework.FastballGetOffersList.Impl
{
  public class FastballGetOffersListRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      return GetStubbedResponse();
    }

    private FastballGetOffersListResponse GetStubbedResponse()
    {
      LocationResult location = new LocationResult()
      {
        CandidateAttributeXml = null,
        RequestUID = null,
        ResultCode = 1,
        SubLocations = new List<SubLocation>() { 
              new SubLocation() { Name = "UpSellFragment", 
                SelectedOffers = new List<SelectedOfferLite>() { 
                  new SelectedOfferLite() { fbiOfferId = "10110", ProductGroupCode = "215", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty } }
              },                                                                        
              new SubLocation() { Name = "DomainSearchFragment",                        
                SelectedOffers = new List<SelectedOfferLite>() {                        
                  new SelectedOfferLite() { fbiOfferId = "10111", ProductGroupCode = "216", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty } }
              },                                                                     
              new SubLocation() { Name = "StandardFragments",                        
                SelectedOffers = new List<SelectedOfferLite>() {                     
                  new SelectedOfferLite() { fbiOfferId = "10112", ProductGroupCode = "217", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty },
                  new SelectedOfferLite() { fbiOfferId = "10113", ProductGroupCode = "218", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty },
                  new SelectedOfferLite() { fbiOfferId = "10114", ProductGroupCode = "219", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty },
                  new SelectedOfferLite() { fbiOfferId = "10115", ProductGroupCode = "220", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty },
                  new SelectedOfferLite() { fbiOfferId = "10116", ProductGroupCode = "221", DiscountType = string.Empty, TargetDate = new System.DateTime(2011, 12, 01), EndDate = new System.DateTime(2012, 12, 31), FastballDiscount = string.Empty, FastballOrderDiscount = string.Empty }
                }
              }
        }
      };


      return new FastballGetOffersListResponse(location);
    }

    #endregion
  }
}
