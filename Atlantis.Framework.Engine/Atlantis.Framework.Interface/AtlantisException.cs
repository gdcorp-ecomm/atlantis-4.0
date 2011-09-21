using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Atlantis.Framework.Interface
{
  public class AtlantisException : Exception
  {
    const string _containerKey = "AtlantisException.HttpProviderContainer";

    /// <summary>
    /// In order to properly detect and log proxy information, set this value in your Global Application Begin Request method
    /// </summary>
    /// <param name="container">Pass in the HttpProviderContainer.Instance</param>
    public static void SetWebRequestProviderContainer(IProviderContainer container)
    {
      if (HttpContext.Current != null)
      {
        HttpContext.Current.Items[_containerKey] = container;
      }
    }

    private static IProviderContainer WebRequestProviderContainer
    {
      get
      {
        IProviderContainer result = null;
        if (HttpContext.Current != null)
        {
          result = HttpContext.Current.Items[_containerKey] as IProviderContainer;
        }
        return result;
      }
    }

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

      SourceURL = requestData.SourceURL;
      ClientIP = IPAddress.Loopback.ToString();
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
      ShopperID = requestData.ShopperID;
      OrderID = requestData.OrderID;
      Pathway = requestData.Pathway;
      _pageCount = requestData.PageCount;

      SourceURL = requestData.SourceURL;
      ClientIP = IPAddress.Loopback.ToString();
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

      SourceURL = requestData.SourceURL;
      ClientIP = clientIP;
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

      SourceURL = sourceURL;
      ClientIP = clientIP;
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

      SourceURL = _sourceUrl;
      ClientIP = _clientIP;
    }

    private string GetClientIPFromContext(string givenIP)
    {
      string result = givenIP ?? string.Empty;

      try
      {
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Request != null)
          {
            result = HttpContext.Current.Request.UserHostAddress;
          }

          IProviderContainer container = WebRequestProviderContainer;
          if ((container != null) && (container.CanResolve<IProxyContext>()))
          {
            IProxyContext proxy = container.Resolve<IProxyContext>();
            result = proxy.OriginIP;
          }
        }
      }
      catch (Exception ex)
      {
        result = ex.Message;
      }

      return result;
    }

    private string GetSourceUrlFromContext(string givenUrl)
    {
      string result = givenUrl ?? string.Empty;

      try
      {
        if (HttpContext.Current != null)
        {
          if (HttpContext.Current.Request != null)
          {
            result = HttpContext.Current.Request.Url.OriginalString;
          }

          IProviderContainer container = WebRequestProviderContainer;
          if ((container != null) && (container.CanResolve<IProxyContext>()))
          {
            IProxyContext proxy = container.Resolve<IProxyContext>();

            if (proxy.Status != ProxyStatusType.None)
            {
              if (proxy.IsLocalARR)
              {
                result = "[" + proxy.ARRHost + "]" + result;
              }

              /// Regardless of ARR proxy or not, we could have another external proxy 
              /// from custom reseller domains or our translation servers
              if (proxy.IsResellerDomain)
              {
                result = "[" + proxy.ResellerDomainHost + "]" + result;
              }
              else if (proxy.IsTransalationDomain)
              {
                result = "[" + proxy.TranslationHost + "]" + result;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        result = ex.Message;
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
        _sourceUrl = GetSourceUrlFromContext(value);
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
        {
          _data = value;
        }
      }
    }

    public string ShopperID
    {
      get { return _shopperId; }
      set
      {
        if (!String.IsNullOrEmpty(value))
        {
          _shopperId = value;
        }
      }
    }

    public string OrderID
    {
      get { return _orderId; }
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          _orderId = value;
        }
      }
    }

    public string ClientIP
    {
      get { return _clientIP; }
      set
      {
        _clientIP = GetClientIPFromContext(value);
      }
    }

    public string Pathway
    {
      get { return _pathway; }
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          _pathway = value;
        }
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
