using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.SearchShoppers.Interface
{
  public class SearchShoppersResponseData : IResponseData
  {
    readonly List<Dictionary<string, string>> _shoppersFound = new List<Dictionary<string, string>>();
    string _responseXml;
    AtlantisException _exception;

    public SearchShoppersResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _exception = null;
      PopulateFromXml(responseXml);
    }

    public SearchShoppersResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public SearchShoppersResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData, 
                                   "SearchShoppersResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }

    public string GetShopperAttribute(int index, string fieldName)
    {
      return _shoppersFound[index][fieldName];
    }

    public Dictionary<string, string> GetShopperAttributes(int index)
    {
      return _shoppersFound[index];
    }

    public int ShopperCount
    {
      get { return _shoppersFound.Count; }
    }

    void PopulateFromXml(string shopperXml)
    {
      XmlDocument xdDoc = new XmlDocument();
      xdDoc.LoadXml(shopperXml);
      XmlNodeList xnlShoppers = xdDoc.SelectNodes("/ShopperSearchReturn/Shopper");

      if (xnlShoppers != null)
      {
        foreach (XmlElement xlShopper in xnlShoppers)
        {
          Dictionary<string, string> dictShopper = new Dictionary<string, string>();
          foreach (XmlAttribute attr in xlShopper.Attributes)
          {
            dictShopper.Add(attr.Name, attr.Value);
          }
          _shoppersFound.Add(dictShopper);
        }
      }
    }

    public bool IsSuccess
    {
      get { return true; }
    }

    #region IResponseData Members



    public AtlantisException GetException()
    {
      return _exception;
    }



    public string ToXML()
    {
      return _responseXml;
    }



    #endregion

  }
}
