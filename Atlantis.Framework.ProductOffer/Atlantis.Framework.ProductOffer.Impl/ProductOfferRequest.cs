using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductOffer.Interface;

namespace Atlantis.Framework.ProductOffer.Impl
{
  public class ProductOfferRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      Dictionary<int, string> offerings = null;

      try
      {
        offerings = new Dictionary<int, string>();

        string offeringXml = DataCache.DataCache.GetCacheData(oRequestData.ToXML());
        XmlDocument offeringDoc = new XmlDocument();
        offeringDoc.LoadXml(offeringXml);

        XmlNodeList itemNodes = offeringDoc.SelectNodes("//item");
        foreach (XmlElement itemElement in itemNodes)
        {
          if (itemElement != null)
          {
            int groupId;
            string groupIdString = itemElement.GetAttribute("pl_productGroupID");
            if (int.TryParse(groupIdString, out groupId))
            {
              offerings[groupId] = itemElement.GetAttribute("description");
            }
          }
        }

        if (offerings.Count == 0)
        {
          ProductOfferRequestData request = oRequestData as ProductOfferRequestData;
          if ((request != null) && (request.PrivateLabelID == 1))
          {
            throw new Exception("ProductOffer GetCacheData returned no offerings for private label 1.");
          }
        }

        result = new ProductOfferResponseData(offerings);
      }
      catch (AtlantisException exAtlantis)
      {
        result = new ProductOfferResponseData(offerings, exAtlantis);
      }
      catch (Exception ex)
      {
        result = new ProductOfferResponseData(offerings, oRequestData, ex);
      }

      return result;
    }

    #endregion

  }
}
