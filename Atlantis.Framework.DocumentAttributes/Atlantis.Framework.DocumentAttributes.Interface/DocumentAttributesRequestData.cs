using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DocumentAttributes.Interface
{
	public class DocumentAttributesRequestData : RequestData
	{
		private int _privateLabelId = 0;
		private bool _getTitle = true;
		private bool _getLastModified = true;
		private bool _getDocumentId = true;
		private bool _getDocumentName = true;
		private List<string> _documentNames = new List<string>();

		TimeSpan _waitTime = TimeSpan.FromMilliseconds(2500);
		TimeSpan _requestTimeout = TimeSpan.FromMilliseconds(2500);

		public DocumentAttributesRequestData(string sShopperID, string sSourceURL, string sOrderID, string sPathway, int iPageCount)
			: base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
		{
		}

		public DocumentAttributesRequestData(string sShopperID, string sSourceURL, string sOrderID, string sPathway, int iPageCount, int privateLabelId, bool getTitle, bool getLastModified, bool getDocumentId, bool getDocumentName)
			: base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
		{
			PrivateLabelId = privateLabelId;
			Title = getTitle;
			LastModified = getLastModified;
			DocumentId = getDocumentId;
			DocumentName = getDocumentName;
			RequestTimeout = _requestTimeout;
		}

		public int PrivateLabelId
		{
			get { return _privateLabelId; }
			set { _privateLabelId = value; }
		}

		public bool Title
		{
			get { return _getTitle; }
			set { _getTitle = value; }
		}

		public bool LastModified
		{
			get { return _getLastModified; }
			set { _getLastModified = value; }
		}

		public bool DocumentId
		{
			get { return _getDocumentId; }
			set { _getDocumentId = value; }
		}

		public bool DocumentName
		{
			get { return _getDocumentName; }
			set { _getDocumentName = value; }
		}

		public TimeSpan WaitTime
		{
			get { return _waitTime; }
			set { _waitTime = value; }
		}

		public TimeSpan RequestTimeout
		{
			get { return _requestTimeout; }
			set { _requestTimeout = value; }
		}

		public void AddDocumentName(string name)
		{
			if (!string.IsNullOrEmpty(name) && !_documentNames.Contains(name))
			{
				_documentNames.Add(name);
			}
		}

		public void AddDocumentNames(List<string> names)
		{
			foreach (string name in names)
			{
				if (!string.IsNullOrEmpty(name) && !_documentNames.Contains(name))
				{
					_documentNames.Add(name);
				}
			}
		}

		#region RequestData Members

		public override string ToXML()
		{
			if (!Title && !LastModified)
			{
				Title = true;
			}

			StringBuilder sbResult = new StringBuilder();
			XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(sbResult));

			xtwResult.WriteStartElement("documents");
			xtwResult.WriteAttributeString("privateLabelID", PrivateLabelId.ToString());
			xtwResult.WriteAttributeString("title", (Title ? "1" : "0"));
			xtwResult.WriteAttributeString("last_modified", (LastModified ? "1" : "0"));
			xtwResult.WriteAttributeString("id", (DocumentId ? "1" : "0"));
			xtwResult.WriteAttributeString("name", (DocumentName ? "1" : "0"));

			foreach (string name in _documentNames)
			{
				xtwResult.WriteStartElement("document");
				xtwResult.WriteAttributeString("name", name);
				xtwResult.WriteEndElement();
			}

			xtwResult.WriteEndElement(); // documents

			return sbResult.ToString();
		}

		public override string GetCacheMD5()
		{
			byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(ToXML());
			MD5 oMD5 = new MD5CryptoServiceProvider();
			oMD5.Initialize();
			byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
			string sValue = BitConverter.ToString(md5Bytes, 0);
			return sValue.Replace("-", "");
		}

		#endregion
	}
}
