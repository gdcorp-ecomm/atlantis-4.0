using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.GetOffersWithPlId.Interface
{
  public class GetOffersWithPlIdResponseData : IResponseData, ISessionSerializableResponse
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;    
    private List<SmartOffer> _offerList;

    public List<SmartOffer> Offers 
    { 
      get 
      {
         return _offerList; 
      } 
    }
    
    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public GetOffersWithPlIdResponseData()
    {
    }

    public GetOffersWithPlIdResponseData(XmlNode responseNode)
    {
      _offerList = HydrateOffers(responseNode);
      _success = (_offerList != null);
    }

    private List<SmartOffer> HydrateOffers(XmlNode responseNode)
    {
      string shopperId;
      int privateLabelId;

      if (responseNode != null)
      {
        _offerList = new List<SmartOffer>();
        _resultXML = responseNode.OuterXml;

        XmlDocument responseDoc = new XmlDocument();

        responseDoc.LoadXml(_resultXML);
        XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(responseDoc.NameTable);
        xmlnsManager.AddNamespace("sm", "https://fastball.godaddy.com/smartoffers");

        shopperId = responseDoc.SelectSingleNode("//sm:ShopperOffers/sm:shopper_id", xmlnsManager).InnerXml;
        int.TryParse(responseDoc.SelectSingleNode("//sm:ShopperOffers/sm:privateLabelId", xmlnsManager).InnerXml, out privateLabelId);

        foreach (XmlNode productOffer in responseDoc.SelectNodes("//sm:ShopperOffers/sm:offers/sm:offer", xmlnsManager))
        {
          _offerList.Add(new SmartOffer(productOffer, xmlnsManager));
        }
      }

      return _offerList;
    }

    public GetOffersWithPlIdResponseData(RequestData requestData, TimeoutException timeoutException)
    {
      this._exception = new AtlantisException(requestData,
                                   "GetOffersWithPlIdResponseData",
                                   timeoutException.Message,
                                   requestData.ToXML());
    }

    public GetOffersWithPlIdResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public GetOffersWithPlIdResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "GetOffersWithPlIdResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }


    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion


    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return _resultXML;
    }

    public void DeserializeSessionData(string sessionData)
    {
      _resultXML = sessionData;
      if (_resultXML != String.Empty && _offerList == null)
      {
        XmlReader xReader = XmlReader.Create(new StringReader(_resultXML));
        XmlDocument xDoc = new XmlDocument();
        XmlNode xNode = xDoc.ReadNode(xReader);
        _offerList = HydrateOffers(xNode);
      }
      _success = true;
    }

    #endregion
  }
}
