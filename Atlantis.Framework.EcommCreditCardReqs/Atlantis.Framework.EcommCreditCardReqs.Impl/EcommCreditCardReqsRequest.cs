using System;
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
                                       Url = ((WsConfigElement) oConfig).WSURL,
                                       Timeout = (int) mktgRequest.RequestTimeout.TotalMilliseconds
                                     };
        string responseText;
        bool success;
        if (!String.IsNullOrEmpty(mktgRequest.CreditCardNumber))
        {
          XmlDocument xmlDoc = new XmlDocument();
          XmlElement rootNode = xmlDoc.CreateElement("Card");
          rootNode.SetAttribute("xCardNo", mktgRequest.CreditCardNumber);
          rootNode.SetAttribute("privateLabelID", mktgRequest.PrivateLabelId.ToString());
          rootNode.SetAttribute("basket_type", "gdshop");
          rootNode.SetAttribute("currency", mktgRequest.Currency);
          xmlDoc.AppendChild(rootNode);
          StringWriter sw = new StringWriter();
          XmlTextWriter xw = new XmlTextWriter(sw);
          xmlDoc.WriteTo(xw);
          string cardXml = sw.ToString();
          
          success = service.GetRequirementsEx(cardXml, out responseText);
        }
        else
        {
          success = service.GetRequirementsByProfile(mktgRequest.ShopperID, mktgRequest.ProfileId, out responseText);
        }
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
