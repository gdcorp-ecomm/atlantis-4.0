using System.Runtime.Serialization;

namespace Atlantis.Framework.FastballGetOffersMsgData.Interface
{
  /*
  <BannerAd nsCICode="39341" imgURL="http://img1.wsimg.com/mobile/iphone/banner_ad_wst.png" order="1" caption="Website Tonight" name="WebSite Tonight®">
        <View type="Web">
          <Detail name="url">http://www.godaddymobile.com/Products/ApplicationLanding/WST2.aspx?hidetoolbar=1</Detail>
        </View>
      </BannerAd>
   */
  [DataContract]
  public class FastBallBannerAd
  {
    [DataMember(Name = "vtype")]
    public string ViewType { get; set; }

    [DataMember(Name = "imgurl")]
    public string ImageUrl { get; set; }

    [DataMember(Name = "cap")]
    public string Caption { get; set; }

    [DataMember(Name = "nm")]
    public string Name { get; set; }

    [DataMember(Name = "curl")]
    public string ClickUrl { get; set; }

    [DataMember(Name = "ord")]
    public string Order { get; set; }

    [DataMember(Name = "ci")]
    public string CICode { get; set; }

    [DataMember(Name = "fboffid")]
    public string FastBallOfferId { get; set; }

    [DataMember(Name = "fbdsc")]
    public string FastballDiscount { get; set; }

    [DataMember(Name = "fbodsc")]
    public string FastballOrderDiscount { get; set; }

    [DataMember(Name = "prd")]
    public string Product { get; set; }
  }
}
