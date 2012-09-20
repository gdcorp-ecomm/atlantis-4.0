using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasketObjects.Interface
{
  public class CartXMLSearchProvider 
  {
    private string _cartXML = string.Empty;
    private XmlDocument _cartXMLDoc = new XmlDocument();
    private XPathDocument _cartXPathDoc;
    private Dictionary<string, XmlNodeList> _searchResults = new Dictionary<string, XmlNodeList>();

    public CartXMLSearchProvider(string cartXML)
    {
      _cartXML = cartXML;
      _cartXMLDoc.LoadXml(cartXML);
      System.IO.StringReader oXMLstring = new System.IO.StringReader(_cartXML);
      _cartXPathDoc = new XPathDocument(oXMLstring);
    }

    public string CartXML
    {
      get
      {
        return _cartXML;
      }
    }

    private CartBasketOrder _basket = null;
    public CartBasketOrder CurrentBasket
    {
      get
      {
        if (_basket == null)
        {
          _basket = new CartBasketOrder(_cartXML);
        }
        return _basket;
      }
    }    

    public XmlNodeList SearchElementsByXPath(string xpath)
    {
      XmlNodeList searchResults;
      if (_searchResults.ContainsKey(xpath))
      {
        searchResults = _searchResults[xpath];
      }
      else
      {
        searchResults = _cartXMLDoc.SelectNodes(xpath);
        _searchResults.Add(xpath, searchResults);
      }
      return searchResults;
    }

    public XmlNode SearchElementByXPath(string xpath)
    {
      XmlNodeList searchResults;
      if (_searchResults.ContainsKey(xpath))
      {
        searchResults = _searchResults[xpath];
      }
      else
      {
        searchResults = _cartXMLDoc.SelectNodes(xpath);
        _searchResults.Add(xpath, searchResults);
      }
      if (searchResults != null && searchResults.Count > 0)
      {
        return searchResults[0];
      }
      return null;
    }

    public object ProcessXSLTExpression(string xpath)
    {
      
      XPathNavigator express = _cartXPathDoc.CreateNavigator();
      return express.Evaluate(xpath);
    }
  }
}
