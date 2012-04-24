using Atlantis.Framework.EcommActivationData.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommActivationData.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetEcommActivationDataTests
  {

    //private const string _shopperId = "853392";
    //private const string _orderId = "1464901";
    //private const string _orderId = "1466042";

    //private const string _shopperId = "4321";
    //private const string _orderId = "12345678";

    private const string _shopperId = "855307";
    private const string _orderId = "1466738";

    private const int _requestType = 518;

    public GetEcommActivationDataTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void No_Bracets_On_ERID()
    {
      EcommActivationDataRequestData request = new EcommActivationDataRequestData(_shopperId
                                                                                  , string.Empty
                                                                                  , _orderId
                                                                                  , string.Empty
                                                                                  , 0);
      EcommActivationDataResponseData response = (EcommActivationDataResponseData) Engine.Engine.ProcessRequest(request, _requestType);
      Assert.IsTrue(response.IsSuccess && response.FreeProducts.Count > 0, "Service call did not work or did not return any data.");
      foreach (ProductInfo currentProduct in response.FreeProducts)
      {
        Assert.IsTrue(!currentProduct.ExternalResourceID.StartsWith("{") && !currentProduct.ExternalResourceID.EndsWith("}"), "Brackets were found on either end of ERID");
      }
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EcommActivationDataTest()
    {
      EcommActivationDataRequestData request = new EcommActivationDataRequestData(_shopperId
           , string.Empty
           , _orderId
           , string.Empty
           , 0);
        EcommActivationDataResponseData response = (EcommActivationDataResponseData)Engine.Engine.ProcessRequest(request, _requestType);
        Assert.IsTrue(response.IsSuccess && response.FreeProducts.Count > 0, "Service call did not work or did not return any data.");
        foreach (ProductInfo currentProduct in response.FreeProducts)
        {
          foreach (ActivatedProducts currentFree in currentProduct.ActivatedProducts)
          {
            if (currentFree.ProductType == ProductInfo.PRODUCT_TYPE_EMAIL)
            {
              System.Diagnostics.Debug.WriteLine(currentFree.Password);
            }
          }
        }

      /*
       * 
       * <Activation order_id="12345678" shopper_id="4321">
<FreeProducts>
<item row_id="0" gdshop_product_typeID="130" pf_id="70761" resource_id="440806" externalResourceID="{E56292FE-748D-11E1-A754-14FEB5E7B468}" 
gdshop_activationStatusID="1">
<InstantPageSetup domain="mydomain.com" title="mydomain.com Instant Page." description="mydomain.com Instant Page." backgroundID="109" 
email="info@mydomain.com" promoCode="MyPromo"/>
</item>
<item row_id="0" gdshop_product_typeID="16" pf_id="1865" resource_id="440807" externalResourceID="{3ACF2160-034D-4F35-94CA-CA612A4B1ABF}" 
gdshop_activationStatusID="1">
<Setup domain="mydomain.com" email="info@mydomain.com" password="MyP3QYUds" diskspaceMB="1024000" smtpRelays="250" hasSpamFilter="1"/>
</item> 
</FreeProducts>
</Activation>

  
      */

      //System.Text.StringBuilder sampleResponse = new System.Text.StringBuilder();
      //sampleResponse.Append("<Activation order_id=\"12345678\" shopper_id=\"4321\">");
      //sampleResponse.Append("<FreeProducts>");
      //sampleResponse.Append("<item row_id=\"0\" gdshop_product_typeID=\"130\" pf_id=\"70761\" resource_id=\"440806\" externalResourceID=\"{E56292FE-748D-11E1-A754-14FEB5E7B468}\" gdshop_activationStatusID=\"3\">");
      //sampleResponse.Append("<InstantPageSetup domain=\"mydomain.com\" title=\"mydomain.com Instant Page.\" description=\"mydomain.com Instant Page.\" backgroundID=\"109\" email=\"info@mydomain.com\" promoCode=\"MyPromo\"/>");
      //sampleResponse.Append("</item>");
      //sampleResponse.Append("<item row_id=\"0\" gdshop_product_typeID=\"16\" pf_id=\"1865\" resource_id=\"440807\" externalResourceID=\"{3ACF2160-034D-4F35-94CA-CA612A4B1ABF}\" gdshop_activationStatusID=\"1\">");
      //sampleResponse.Append("<Setup domain=\"mydomain.com\" email=\"info@mydomain.com\" password=\"MyP3QYUds\" diskspaceMB=\"1024000\" smtpRelays=\"250\" hasSpamFilter=\"1\"/>");
      //sampleResponse.Append("</item> ");
      //sampleResponse.Append("</FreeProducts>");
      //sampleResponse.Append("</Activation>");
      //XmlDocument result = new XmlDocument();
      //result.LoadXml(sampleResponse.ToString());
      //EcommActivationDataResponseData response1 = new EcommActivationDataResponseData(result);
      //System.Diagnostics.Debug.WriteLine(response1.IsAllActivated);
      //System.Diagnostics.Debug.WriteLine(response1.IsAllTypeActivated(ProductInfo.PRODUCT_TYPE_EMAIL));
      //System.Diagnostics.Debug.WriteLine(response1.IsAllTypeActivated(ProductInfo.PRODUCT_TYPE_INSTANTPAGE));
    }
  }
}
