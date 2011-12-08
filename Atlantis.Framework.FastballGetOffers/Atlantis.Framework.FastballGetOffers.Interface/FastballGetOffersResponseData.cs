using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.FastballGetOffers.Interface
{
  public class FastballGetOffersResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    private string _xmlResponse = string.Empty;

    public FastballGetOffersResponseData()
    {
    }

    public FastballGetOffersResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(
        requestData, "Atlantis.Framework.FastballGetOffers", ex.Message, ex.StackTrace, ex);
    }

    public FastballGetOffersResponseData(string xmlResponse)
    {
      _xmlResponse = xmlResponse;
      ResetResultXml();  
    }

    private void ResetResultXml()
    {
      _offerResultXml = null;
    }

    private XDocument _offerResultXml;
    public XDocument OfferResultXml
    {
      get
      {
        if (_offerResultXml == null)
        {
          string parseError = null;
          if (!string.IsNullOrEmpty(_xmlResponse))
          {
            try
            {
              _offerResultXml = XDocument.Parse(_xmlResponse);
            }
            catch (Exception ex)
            {
              parseError = ex.Message;
              _offerResultXml = null;
            }
          }

          if (_offerResultXml == null)
          {
            XElement root = new XElement("OfferResultXml");
            if (parseError != null)
            {
              XElement parseErrorElement = new XElement("ParseError", parseError);
              root.Add(parseErrorElement);
            }
            _offerResultXml = new XDocument(root);
          }
        }
        return _offerResultXml;
      }
    }

    public bool IsSuccess
    {
      get 
      {
        bool result = false;

        if (ResultCode == "0")
        {
          result = true;
        }

        return result;
      }
    }

    public string ResultCode
    {
      get
      {
        string result = null;

        XElement resultCodeElement = OfferResultXml.Descendants("ResultCode").FirstOrDefault();
        if (resultCodeElement != null)
        {
          result = resultCodeElement.Value;
        }

        return result;
      }
    }

    public string PersonaId
    {
      get
      {
        string result = string.Empty;

        XElement shopper = OfferResultXml.Descendants("Shopper").FirstOrDefault();
        if (shopper != null)
        {
          XAttribute personaId = shopper.Attribute("personaID");
          if (personaId != null)
          {
            result = personaId.Value;
          }
        }

        return result;
      }
    }

    public string LcaCode
    {
      get
      {
        string result = string.Empty;

        XElement shopper = OfferResultXml.Descendants("Shopper").FirstOrDefault();
        if (shopper != null)
        {
          XAttribute lcaCode = shopper.Attribute("lcaCode");
          if (lcaCode != null)
          {
            result = lcaCode.Value;
          }
        }

        return result;
      }
    }

    public string FirstFbiOfferId
    {
      get
      {
        string result = string.Empty;

        XElement offer = OfferResultXml.Descendants("Offer").FirstOrDefault();
        if (offer != null)
        {
          XAttribute fbiOfferid = offer.Attribute("fbiOfferId");
          if (fbiOfferid != null)
          {
            result = fbiOfferid.Value;
          }
        }

        return result;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _xmlResponse;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return _xmlResponse;
    }

    public void DeserializeSessionData(string sessionData)
    {
      _xmlResponse = sessionData;
      ResetResultXml();
    }

    #endregion
  }
}
