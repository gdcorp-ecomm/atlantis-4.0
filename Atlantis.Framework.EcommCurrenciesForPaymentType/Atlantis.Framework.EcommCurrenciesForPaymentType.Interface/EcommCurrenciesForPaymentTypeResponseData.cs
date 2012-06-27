using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommCurrenciesForPaymentType.Interface
{
  public class EcommCurrenciesForPaymentTypeResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _responseXml;
    bool _isSuccess = false;

    public EcommCurrenciesForPaymentTypeResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isSuccess = true;
    }

    public EcommCurrenciesForPaymentTypeResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public EcommCurrenciesForPaymentTypeResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _exception = new AtlantisException(oRequestData, oRequestData.GetType().ToString(), ex.Message, ex.StackTrace, ex);
    }

    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    List<string> _availableCurrencyList = null;
    public List<string> AvaiblableCurrencyList
    {
      get
      {
        if (_availableCurrencyList == null)
        {
          _availableCurrencyList = new List<string>();
          XmlDocument doc = new XmlDocument();
          if (!string.IsNullOrWhiteSpace(_responseXml))
          {
            doc.LoadXml(_responseXml);
            XmlNodeList nodes = doc.GetElementsByTagName("Currency");
            if (nodes != null && nodes.Count > 0)
            {
              foreach (XmlNode node in nodes)
              {
                _availableCurrencyList.Add(node.InnerText);
              }
            }
          }
        }

        return _availableCurrencyList;
      }
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
