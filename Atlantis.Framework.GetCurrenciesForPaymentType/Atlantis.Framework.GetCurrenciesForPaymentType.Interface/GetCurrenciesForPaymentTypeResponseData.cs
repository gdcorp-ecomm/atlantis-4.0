using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetCurrenciesForPaymentType.Interface
{
  public class GetCurrenciesForPaymentTypeResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _responseXml;
    bool _isSuccess = false;

    public GetCurrenciesForPaymentTypeResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isSuccess = true;
    }

    public GetCurrenciesForPaymentTypeResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _exception = exAtlantis;
    }

    public GetCurrenciesForPaymentTypeResponseData(string responseXml, RequestData oRequestData, Exception ex)
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
