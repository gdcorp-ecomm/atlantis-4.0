using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoData.Interface
{
  public class PromoDataResponseData : IResponseData
  {
    #region Properties

    private AtlantisException _exception = null;

    private IPromoProduct _promoProduct;
    public IPromoProduct PromoProduct
    {
      get { return _promoProduct; }
    }

    private bool _isValid = false;
    public bool IsValid
    {
      get { return _isValid; }
    }

    #endregion Properties

    #region Constructors

    public PromoDataResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public PromoDataResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "PromoDataResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public PromoDataResponseData(IPromoProduct promoProduct)
    {
      this._promoProduct = promoProduct;
      this._isValid = true;
    }

    #endregion Constructors

    #region Public Methods

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("response");

      //foreach (KeyValuePair<string, DatabaseDomainAttributes> domain in this._domainsWithAttributes)
      //{
      //  xtwRequest.WriteStartElement("premiumDomain");
      //  xtwRequest.WriteAttributeString("price", domain.Value.Price.ToString());
      //  xtwRequest.WriteAttributeString("commission", domain.Value.Commission.ToString());
      //  xtwRequest.WriteAttributeString("auctionendtime", domain.Value.AuctionEndTime.ToString());
      //  xtwRequest.WriteValue(domain.Key);
      //  xtwRequest.WriteEndElement();
      //}

      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    #endregion Public Methods
  }

/*
<!--exec gdshop_promo_advget_sp @iscCode_list='GREGTEST1'-->
<Promo promoCode='GREGTEST1'>
    <Product startDate='12/12/2002' endDate='12/13/2002' Active='true' name='Generic_gregtest1'> <!--this is promoName-->
        <Disclaimer></Disclaimer>
        <PrivateLabelTypes>
            <PrivateLabelType privateLabelTypeID='1' ></PrivateLabelType>
        </PrivateLabelTypes>
        <ProductConditions>
            <ProductCondition  unifiedProductId='101' conditionField='' conditionFieldValue='' productMinQty='1' productExactQty='' />
            <ProductCondition  unifiedProductId='102' conditionField='' conditionFieldValue='' productMinQty='' productExactQty='2' />
            <ProductCondition  unifiedProductId='' conditionField='fastballDiscount' conditionFieldValue='34535' productMinQty='' productExactQty='1' />
        </ProductConditions>
        <ProductAwards>
            <ProductAward type='AmountOff' unifiedProductId='102' awardField='' awardFieldValue='' productQtyLimit='-1'>
                <AwardCurrencies>
                    <AwardCurrency transactionalCurrency='USD' awardAmount='100'></AwardCurrency>
                    <AwardCurrency transactionalCurrency='EUR' awardAmount='73'></AwardCurrency>
                    <AwardCurrency transactionalCurrency='GBP' awardAmount='62'></AwardCurrency>
                </AwardCurrencies>
            </ProductAward>
            <ProductAward type='PercentOff' unifiedProductId='103' awardField='' awardFieldValue='' productQtyLimit='-1'>
                <AwardCurrencies>
                    <AwardCurrency transactionalCurrency='*' awardAmount='5'></AwardCurrency>
                </AwardCurrencies>
            </ProductAward>
            <ProductAward type='PercentOff' unifiedProductId='104' awardField='' awardFieldValue='' productQtyLimit='2'>
                <AwardCurrencies>
                    <AwardCurrency transactionalCurrency='*' awardAmount='12'></AwardCurrency>
                </AwardCurrencies>
            </ProductAward>
            <ProductAward type='PercentOff' unifiedProductId='105' awardField='' awardFieldValue='' productQtyLimit='2'>
                <AwardCurrencies>
                    <AwardCurrency transactionalCurrency='*' awardAmount='12'></AwardCurrency>
                </AwardCurrencies>
            </ProductAward>
            <ProductAward type='SetAmount' unifiedProductId='110' awardField='' awardFieldValue='' productQtyLimit='-1'>
                <AwardCurrencies>
                    <AwardCurrency transactionalCurrency='USD' awardAmount='999'></AwardCurrency>
                    <AwardCurrency transactionalCurrency='GBP' awardAmount='611'></AwardCurrency>
                </AwardCurrencies>
            </ProductAward>
            <ProductAward type='AmountOff' unifiedProductId='' awardField='fastballDiscount' awardFieldValue='59892' productQtyLimit='-1'>
                <AwardCurrencies>
                    <AwardCurrency transactionalCurrency='USD' awardAmount='200'></AwardCurrency>
                </AwardCurrencies>
            </ProductAward>
        </ProductAwards>
    </Product>
</Promo>
*/
}
