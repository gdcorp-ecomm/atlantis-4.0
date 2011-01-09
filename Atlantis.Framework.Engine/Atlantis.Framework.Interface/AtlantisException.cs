using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Web;

namespace Atlantis.Framework.Interface
{
  public class AtlantisException : Exception
  {
    DateTime _logTime;
    string _sourceServer = string.Empty;
    string _sourceFunction = string.Empty;
    string _sourceUrl = "-- http://localhost --";
    string _errorNumber = "0";
    string _errorDescription = string.Empty;
    string _data = "-- input data --";
    string _shopperId = "unknown";
    string _orderId = "0";
    string _clientIP = IPAddress.Loopback.ToString();
    string _pathway = "{00000000-0000-0000-0000-000000000000}";
    int _pageCount = 0;

    public AtlantisException(RequestData requestData,
                              string sourceFunction,
                              string errorDescription,
                              string data)
      : base(errorDescription)
    {
      _logTime = DateTime.Now;
      _sourceServer = Dns.GetHostName();
      _sourceFunction = sourceFunction;
      _errorDescription = errorDescription;
      ExData = data;
      ShopperID = requestData.ShopperID;
      OrderID = requestData.OrderID;
      Pathway = requestData.Pathway;
      _pageCount = requestData.PageCount;

      if (!string.IsNullOrEmpty(requestData.SourceURL))
      {
        SourceURL = requestData.SourceURL;
      }
      else
      {
        SourceURL = GetSourceUrlFromContext();
      }

      ClientIP = GetClientIPFromContext();

    }

    public AtlantisException(RequestData requestData,
                              string sourceFunction,
                              string errorDescription,
                              string data,
                              Exception ex)
      : base(ex.Message, ex.InnerException)
    {
      _logTime = DateTime.Now;
      _sourceServer = Dns.GetHostName();
      _sourceFunction = sourceFunction;
      _errorDescription = errorDescription;
      ExData = data;
      SourceURL = requestData.SourceURL;
      ShopperID = requestData.ShopperID;
      OrderID = requestData.OrderID;
      Pathway = requestData.Pathway;
      _pageCount = requestData.PageCount;
    }

    public AtlantisException(RequestData requestData,
                              string sourceFunction,
                              string errorNumber,
                              string errorDescription,
                              string data,
                              string clientIP)
      : base(errorDescription)
    {
      _logTime = DateTime.Now;
      _sourceServer = Dns.GetHostName();
      _sourceFunction = sourceFunction;
      _errorDescription = errorDescription;
      ExData = data;
      ShopperID = requestData.ShopperID;
      OrderID = requestData.OrderID;
      Pathway = requestData.Pathway;
      _pageCount = requestData.PageCount;

      if (!string.IsNullOrEmpty(requestData.SourceURL))
      {
        SourceURL = requestData.SourceURL;
      }
      else
      {
        SourceURL = GetSourceUrlFromContext();
      }

      if (!string.IsNullOrEmpty(clientIP))
      {
        ClientIP = clientIP;
      }
      else
      {
        ClientIP = GetClientIPFromContext();
      }

    }

    public AtlantisException(string sourceFunction,
                              string sourceURL,
                              string errorNumber,
                              string errorDescription,
                              string data,
                              string shopperId,
                              string orderId,
                              string clientIP,
                              string pathway,
                              int pageCount)
      : base(errorDescription)
    {
      _logTime = DateTime.Now;
      _sourceServer = Dns.GetHostName();
      _sourceFunction = sourceFunction;
      _errorNumber = errorNumber;
      _errorDescription = errorDescription;
      ExData = data;
      ShopperID = shopperId;
      OrderID = orderId;
      Pathway = pathway;
      _pageCount = pageCount;

      if (!string.IsNullOrEmpty(sourceURL))
      {
        SourceURL = sourceURL;
      }
      else
      {
        SourceURL = GetSourceUrlFromContext();
      }

      if (!string.IsNullOrEmpty(clientIP))
      {
        ClientIP = clientIP;
      }
      else
      {
        ClientIP = GetClientIPFromContext();
      }
    }

    public AtlantisException(
      string sourceFunction, 
      string errorNumber, 
      string errorDescription,
      string data,
      ISiteContext siteContext,
      IShopperContext shopperContext)
      : base(errorDescription)
    {
      _logTime = DateTime.Now;
      _sourceServer = Dns.GetHostName();
      _sourceFunction = sourceFunction;
      _errorNumber = errorNumber;
      _errorDescription = errorDescription;
      ExData = data;

      if (shopperContext != null)
      {
        ShopperID = shopperContext.ShopperId;
      }

      if (siteContext != null)
      {
        Pathway = siteContext.Pathway;
        _pageCount = siteContext.PageCount;
      }

      SourceURL = GetSourceUrlFromContext();
      ClientIP = GetClientIPFromContext();
    }

    private string GetClientIPFromContext()
    {
      string result = string.Empty;
      if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
      {
        result = HttpContext.Current.Request.UserHostAddress;
      }
      return result;
    }

    private string GetSourceUrlFromContext()
    {
      string result = string.Empty;
      if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
      {
        result = HttpContext.Current.Request.RawUrl;
      }

      return result;
    }

    public DateTime LogTime
    {
      get { return _logTime; }
    }

    public string SourceServer
    {
      get { return _sourceServer; }
    }

    public string SourceFunction
    {
      get { return _sourceFunction; }
    }

    public string SourceURL
    {
      get { return _sourceUrl; }
      set
      {
        if (!string.IsNullOrEmpty(value))
          _sourceUrl = value;
      }
    }

    public string ErrorNumber
    {
      get { return _errorNumber; }
      set { _errorNumber = value; }
    }

    public string ErrorDescription
    {
      get { return _errorDescription; }
    }

    public string ExData
    {
      get { return _data; }
      set
      {
        if (!string.IsNullOrEmpty(value))
          _data = value;
      }
    }

    public string ShopperID
    {
      get { return _shopperId; }
      set
      {
        if (!String.IsNullOrEmpty(value))
          _shopperId = value;
      }
    }

    public string OrderID
    {
      get { return _orderId; }
      set
      {
        if (!string.IsNullOrEmpty(value))
          _orderId = value;
      }
    }

    public string ClientIP
    {
      get { return _clientIP; }
      set
      {
        IPAddress address = null;
        if (!string.IsNullOrEmpty(value) && IPAddress.TryParse(value, out address))
          _clientIP = address.ToString();
      }
    }

    public string Pathway
    {
      get { return _pathway; }
      set
      {
        if (!string.IsNullOrEmpty(value))
          _pathway = value;
      }
    }

    public int PageCount
    {
      get { return _pageCount; }
      set { _pageCount = value; }
    }

    public string ToXml()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("SITE_LOG_ERROR");
      xtwRequest.WriteAttributeString("source_server", _sourceServer);
      xtwRequest.WriteAttributeString("source_function", _sourceFunction);
      xtwRequest.WriteAttributeString("error_number", _errorNumber);
      xtwRequest.WriteAttributeString("error_description", _errorDescription);
      xtwRequest.WriteAttributeString("order_id", _orderId);
      xtwRequest.WriteAttributeString("shopper_id", _shopperId);
      xtwRequest.WriteAttributeString("url", _sourceUrl);
      xtwRequest.WriteAttributeString("input_data", _data);
      xtwRequest.WriteAttributeString("client_ip", _clientIP);
      xtwRequest.WriteAttributeString("date_logged", _logTime.ToString());
      xtwRequest.WriteAttributeString("pathway", _pathway);
      xtwRequest.WriteAttributeString("page_count", System.Convert.ToString(_pageCount));
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }
  }
}
