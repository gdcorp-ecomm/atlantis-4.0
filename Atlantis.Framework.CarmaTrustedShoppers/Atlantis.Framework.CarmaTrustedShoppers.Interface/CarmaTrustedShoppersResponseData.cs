using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaTrustedShoppers.Interface
{
  public class CarmaTrustedShoppersResponseData : IResponseData
  {
    #region Properties
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public bool HasTrustedShoppers { get; private set; }
    public List<TrustedShopper> TrustedShoppers { get; private set; }
    #endregion

    public CarmaTrustedShoppersResponseData(string xml)
    {
      _resultXML = xml;
      TrustedShoppers = new List<TrustedShopper>();
      
      XDocument xDoc = XDocument.Parse(xml);
      HasTrustedShoppers = !xDoc.Element("TrustedShoppers").Attribute("count").Value.Equals("0");

      if (HasTrustedShoppers)
      {
        TrustedShoppers = (from ts in xDoc.Element("TrustedShoppers").Elements()
                           select new TrustedShopper()
                           {
                             FirstName = ts.Attribute("first_name").Value,
                             LastName = ts.Attribute("last_name").Value,
                             ShopperId = ts.Attribute("shopper_id").Value
                           }
                          ).ToList<TrustedShopper>();
      }
    }

     public CarmaTrustedShoppersResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public CarmaTrustedShoppersResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "CarmaTrustedShoppersResponseData"
        , exception.Message
        , requestData.ToXML());
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

  }
}
