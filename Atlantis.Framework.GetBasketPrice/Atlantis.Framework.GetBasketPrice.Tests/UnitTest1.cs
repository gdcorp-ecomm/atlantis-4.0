using System;
using System.Diagnostics;
using System.Xml;
using Atlantis.Framework.GetBasketPrice.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetBasketPrice.Tests {

  [TestClass]
  public class GetBasketPriceTests
  {
    private const string shopperId = "858421";
    private const int BasketGetPriceRequestId = 34;
    private const string BasketOrderDetailProperty_TotalTotal = "_total_total";

    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void LoadTotals()
    {
      int itemCount = 0;
      if (!string.IsNullOrEmpty(shopperId)) {
        try {
          GetBasketPriceRequestData request = new GetBasketPriceRequestData(
              shopperId,
              string.Empty,
              string.Empty,
              string.Empty,
              1,
              true,
              string.Empty);

          request.RequestTimeout = TimeSpan.FromSeconds(4);

          GetBasketPriceResponseData response = (GetBasketPriceResponseData)Engine.Engine.ProcessRequest(request, BasketGetPriceRequestId);

          itemCount = response.ItemCount;
          if (itemCount > 0) {
            string price = ParseCartXml(response.ToXML());
            Debug.WriteLine(price);
            Assert.IsNotNull(price);
          }
        } catch (Exception ex) {
          string msg = "Error getting CartItemCount for shopper: " + ex.Message + Environment.NewLine + ex.StackTrace;
          Debug.WriteLine(msg);
        }

      } else
      {
        Assert.Fail("No Shopper Id Provided");
      }

    }

    private string ParseCartXml(string cartXml) {
      XmlDocument responseXml = new XmlDocument();
      responseXml.LoadXml(cartXml);
      string basketTotal = null;

      XmlNode orderDetailNode = responseXml.SelectSingleNode("//ORDERDETAIL");
      if (orderDetailNode != null) {

        XmlAttribute totalAttribute = orderDetailNode.Attributes[BasketOrderDetailProperty_TotalTotal];
       
        if (totalAttribute != null)
        {
          int price;
          basketTotal = int.TryParse(totalAttribute.Value, out price) ? string.Format("{0}", price) : string.Empty;
        }
      }

      return basketTotal;
    }



  }

}
