using System.IO;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.Interface
{
	public abstract class RequestData
	{
		string _sourceURL;
		string _shopperID;
		string _orderID;
		string _pathway;
		int _pageCount;

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

		public abstract string GetCacheMD5();

		public virtual string ToXML()
		{
			StringBuilder sbRequest = new StringBuilder();
			XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

			xtwRequest.WriteStartElement("INFO");
			xtwRequest.WriteAttributeString("ShopperID", _shopperID);
			xtwRequest.WriteAttributeString("SourceURL", _sourceURL);
			xtwRequest.WriteAttributeString("OrderID", _orderID);
			xtwRequest.WriteAttributeString("Pathway", _pathway);
			xtwRequest.WriteAttributeString("PageCount", System.Convert.ToString(_pageCount));
			xtwRequest.WriteEndElement();

			return sbRequest.ToString();

		}
	}
}
