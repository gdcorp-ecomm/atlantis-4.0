using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.Interface
{
  public abstract class RequestData
  {
    readonly string _sourceURL;
    string _shopperID;
    readonly string _orderID;
    readonly string _pathway;
    readonly int _pageCount;
    TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);

    public RequestData()
    {
      _shopperID = string.Empty;
      _sourceURL = string.Empty;
      _orderID = string.Empty;
      _pathway = string.Empty;
      _pageCount = 0;
    }
      
    public RequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
    {
      _shopperID = shopperId;
      _sourceURL = sourceURL;
      _orderID = orderId;
      _pathway = pathway;
      _pageCount = pageCount;
    }

    public string ShopperID
    {
      get { return _shopperID; }
      set { _shopperID = value; }
    }

    public string SourceURL
    {
      get { return _sourceURL; }
    }

    public string OrderID
    {
      get { return _orderID; }
    }

    public string Pathway
    {
      get { return _pathway; }
    }

    public int PageCount
    {
      get { return _pageCount; }
    }

    public TimeSpan RequestTimeout
    {
      get { return _requestTimeout; }
      set { _requestTimeout = value; }
    }

    public virtual string GetCacheMD5()
    {
      string message = this.GetType().Name + " is not cacheable because it does not override GetCacheMD5";
      throw new NotImplementedException(message);
    }

    public virtual string ToXML()
    {
      var sbRequest = new StringBuilder();
      var xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("ShopperID", _shopperID);
      xtwRequest.WriteAttributeString("SourceURL", _sourceURL);
      xtwRequest.WriteAttributeString("OrderID", _orderID);
      xtwRequest.WriteAttributeString("Pathway", _pathway);
      xtwRequest.WriteAttributeString("PageCount", System.Convert.ToString(_pageCount));
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    protected string BuildHashFromStrings(params string[] values)
    {
      var hashedValue = string.Empty;
      if (values != null)
      {
        var keyToHash = string.Join("|", values);

        using (MD5 md5 = new MD5CryptoServiceProvider())
        {
          md5.Initialize();
          var stringBytes = ASCIIEncoding.ASCII.GetBytes(keyToHash);
          var md5Bytes = md5.ComputeHash(stringBytes);
          hashedValue = BitConverter.ToString(md5Bytes, 0);
        }
      }
      return hashedValue;
    }
  }
}
