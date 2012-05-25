using System;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.WhoIsGetInfo.Interface.Contacts;
using Atlantis.Framework.WhoIsGetInfo.Interface.Nameservers;
using Atlantis.Framework.WhoIsGetInfo.Interface.Status;

namespace Atlantis.Framework.WhoIsGetInfo.Interface
{
	public class WhoIsGetInfoResponseData : IResponseData
	{

		private AtlantisException _exception = null;
		private bool _success = false;
		private WhoIsInfo _whoIsInfo;

		public bool IsSuccess
		{
			get { return _success; }
		}

		public WhoIsInfo whoIsInfo
		{
			get { return _whoIsInfo; }
		}

		public WhoIsGetInfoResponseData(WhoIsInfo whoIsInfo)
		{

			_whoIsInfo = whoIsInfo;
			_success = true;
		}

		public WhoIsGetInfoResponseData(AtlantisException atlantisException)
		{
			_exception = atlantisException;
		}

		public WhoIsGetInfoResponseData(RequestData requestData, Exception exception)
		{
			_exception = new AtlantisException(requestData,
																				 "GetWhoIsInfoResponseData",
			                                   exception.Message,
			                                   requestData.ToXML());
		}

		#region Implementation of IResponseData

		public string ToXML()
		{
			StringBuilder sb = new StringBuilder();

			try
			{
				using (XmlWriter writer = XmlWriter.Create(sb))
				{
					writer.WriteStartElement("Response");
					writer.WriteAttributeString("Code", _whoIsInfo.ResponseCode);
					writer.WriteAttributeString("Description", _whoIsInfo.ResponseDescription);
					writer.WriteAttributeString("IsValid", _whoIsInfo.IsValid.ToString());
					

					writer.WriteStartElement("WhoIsInquiry");
					writer.WriteAttributeString("CreatedDate", _whoIsInfo.CreatedDate.ToString());
					writer.WriteAttributeString("ExpirationDate", _whoIsInfo.ExpirationDate.ToString());
					writer.WriteAttributeString("UpdateDate", _whoIsInfo.UpdateDate.ToString());
					writer.WriteAttributeString("IsAvailable", _whoIsInfo.IsAvailable.ToString());
					writer.WriteAttributeString("IsProxied", _whoIsInfo.IsProxied.ToString());
					
					writer.WriteAttributeString("DomainName", _whoIsInfo.Name);
					writer.WriteAttributeString("PLID", _whoIsInfo.PrivateLabelId);
					writer.WriteAttributeString("Registrar", _whoIsInfo.Registrar);
					writer.WriteAttributeString("RegistrarId", _whoIsInfo.RegistrarId);
					writer.WriteAttributeString("Reseller", _whoIsInfo.Reseller);
					writer.WriteAttributeString("WhoIsServer", _whoIsInfo.WhoIsServer);
					writer.WriteElementString("AdditionalText", _whoIsInfo.RegistrarDescriptiveText);
          
					writer.WriteStartElement("Contacts");
					foreach (WhoIsContact c in _whoIsInfo.Contacts)
					{
						writer.WriteStartElement("Contact");

						writer.WriteAttributeString("Company", c.Company);
						writer.WriteAttributeString("FirstName", c.FirstName);
						writer.WriteAttributeString("LastName", c.LastName);
						writer.WriteAttributeString("Address1", c.Address1);
						writer.WriteAttributeString("Address2", c.Address2);
						writer.WriteAttributeString("City", c.City);
						writer.WriteAttributeString("State", c.State);
						writer.WriteAttributeString("Zip", c.Zip);
						writer.WriteAttributeString("Country", c.Country);
						writer.WriteAttributeString("Email", c.Email);
						writer.WriteAttributeString("Fax", c.Fax);
						writer.WriteAttributeString("Phone", c.Phone);
						writer.WriteAttributeString("ROID", c.ROID);
						writer.WriteAttributeString("ContactType", Enum.GetName(typeof(WhoIsContactTypes),c.Type));

						writer.WriteEndElement();
					}
					writer.WriteEndElement();

					
					writer.WriteStartElement("NameServers");
					foreach (WhoIsNameServer ns in _whoIsInfo.NameServers)
					{
						writer.WriteStartElement("NameServer");
						writer.WriteAttributeString("Server", ns.Server);
						writer.WriteEndElement();
					}
					writer.WriteEndElement();

					
					writer.WriteStartElement("StatusFlags");
					foreach (WhoIsStatus s in _whoIsInfo.Status)
					{
						writer.WriteStartElement("Status");
						writer.WriteAttributeString("Value", s.StatusText);
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
			}
			catch (Exception ex)
			{
				throw new AtlantisException("GetWhoIsInfoResponseData::ToXml", string.Empty, string.Empty, "Error Converting Response Object To XML", ex.Message, string.Empty, string.Empty, string.Empty, string.Empty, 0);
			}

			return sb.ToString();
		}

		public AtlantisException GetException()
		{
			return _exception;
		}

		#endregion
	}
}
