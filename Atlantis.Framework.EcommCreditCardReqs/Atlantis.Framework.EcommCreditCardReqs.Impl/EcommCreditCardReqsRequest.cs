using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Atlantis.Framework.EcommCreditCardReqs.Impl.WSgdCreditCardRequirements;
using Atlantis.Framework.EcommCreditCardReqs.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommCreditCardReqs.Impl
{
  public class EcommCreditCardReqsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result;

      try
      {

        EcommCreditCardReqsRequestData mktgRequest = (EcommCreditCardReqsRequestData)oRequestData;

        CardRequirements service = new CardRequirements
        {
          Url = ((WsConfigElement)oConfig).WSURL,
          Timeout = (int)mktgRequest.RequestTimeout.TotalMilliseconds
        };

        string responseText;
        var xmlDoc = new XmlDocument();
        var rootNode = xmlDoc.CreateElement("Card");
        if (!string.IsNullOrEmpty(mktgRequest.CreditCardNumber))
        {
          rootNode.SetAttribute("xCardNo", mktgRequest.CreditCardNumber);
        }
        else
        {
          rootNode.SetAttribute("profileID", mktgRequest.ProfileId.ToString());
        }
        rootNode.SetAttribute("privateLabelID", mktgRequest.PrivateLabelId.ToString());
        rootNode.SetAttribute("shopperID", mktgRequest.ShopperID);
        rootNode.SetAttribute("basket_type", "gdshop");
        rootNode.SetAttribute("currency", mktgRequest.Currency);
        rootNode.SetAttribute("country", mktgRequest.Country);
        xmlDoc.AppendChild(rootNode);
        var sw = new StringWriter();
        var xw = new XmlTextWriter(sw);
        xmlDoc.WriteTo(xw);
        var cardXml = sw.ToString();
        var success = service.GetRequirementsEx(cardXml, out responseText);
        result = new EcommCreditCardReqsResponseData(responseText, success);
      }
      catch (AtlantisException aex)
      {
        result = new EcommCreditCardReqsResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new EcommCreditCardReqsResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
