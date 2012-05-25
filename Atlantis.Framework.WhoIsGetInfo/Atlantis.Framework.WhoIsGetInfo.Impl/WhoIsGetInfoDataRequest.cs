using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.WhoIsGetInfo.Interface;
using Atlantis.Framework.WhoIsGetInfo.Interface.Contacts;
using Atlantis.Framework.WhoIsGetInfo.Interface.Nameservers;
using Atlantis.Framework.WhoIsGetInfo.Interface.Status;

namespace Atlantis.Framework.WhoIsGetInfo.Impl
{
	public class WhoIsGetInfoDataRequest : IRequest
	{
		#region IRequest Members

		public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
		{

			WhoIsGetInfoResponseData responseData = null;
			string whoisInfoXml = string.Empty;

			try
			{
				WhoIsGetInfoRequestData whoisInfoRequestData = (WhoIsGetInfoRequestData)oRequestData;
				whoisInfoXml = GetWhoIsInfo(whoisInfoRequestData, oConfig);

				responseData = new WhoIsGetInfoResponseData(ProcessXML(whoisInfoXml));
			}

			catch (AtlantisException exAtlantis)
			{
				responseData = new WhoIsGetInfoResponseData(exAtlantis);
			}

			catch (Exception ex)
			{
				responseData = new WhoIsGetInfoResponseData(oRequestData, ex);
			}

			return responseData;
		}

		private WhoIsInfo ProcessXML(string whoisInfoXml)
		{
			WhoIsInfo whoIs = new WhoIsInfo();
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(whoisInfoXml);
			}
			catch
			{

			}
			try
			{
				XDocument rawResults = XDocument.Parse(whoisInfoXml);
				whoIs.RawResults = rawResults;

			}
			catch { }

			if (!string.IsNullOrEmpty(doc.OuterXml))
			{
				whoIs.ResponseCode = doc.SelectSingleNode("//RESPONSE/CODE").InnerText;
				whoIs.IsValid = whoIs.ResponseCode == "1000";
				whoIs.ResponseDescription = doc.SelectSingleNode("//RESPONSE/DESCRIPTION").InnerText; ;

				if (whoIs.IsValid)
				{

					try
					{
						whoIs.Name = doc.SelectSingleNode("//DOMAIN/NAME").InnerText;
					}
					catch { }

					try
					{
						whoIs.Registrar = doc.SelectSingleNode("//DOMAIN/REGISTRAR").InnerText;
					}
					catch { }

					try
					{
						whoIs.RegistrarId = doc.SelectSingleNode("//DOMAIN/REGISTRARID").InnerText;
					}
					catch { }
					try
					{
						whoIs.WhoIsServer = doc.SelectSingleNode("//DOMAIN/WHOISSERVER").InnerText;
					}
					catch { }

					try
					{
						whoIs.PrivateLabelId = doc.SelectSingleNode("//DOMAIN/PLID").InnerText;
					}
					catch { }

					try
					{
						whoIs.Reseller = doc.SelectSingleNode("//DOMAIN/RESELLER").InnerText;
					}
					catch { }


					try
					{
						XmlNodeList contactNodes = doc.SelectNodes("//CONTACT");
						if (contactNodes != null)
						{
							foreach (XmlNode contact in contactNodes)
							{
								WhoIsContact newContact = new WhoIsContact();

								try
								{
									string whoIsType = contact.SelectSingleNode("./TYPE").InnerText;
									newContact.Type = (WhoIsContactTypes)Enum.Parse(typeof(WhoIsContactTypes), whoIsType);
								}
								catch { }
								try
								{
									newContact.Company = contact.SelectSingleNode("./COMPANY").InnerText;
								}
								catch { }
								try
								{
									newContact.FirstName = contact.SelectSingleNode("./FNAME").InnerText;
								}
								catch { }
								try
								{
									newContact.LastName = contact.SelectSingleNode("./LNAME").InnerText;

								}
								catch { }
								try
								{
									newContact.Address1 = contact.SelectSingleNode("./ADDRESS[1]").InnerText;

								}
								catch { }
								try
								{
									newContact.Address2 = contact.SelectSingleNode("./ADDRESS[2]").InnerText;

								}
								catch { }
								try
								{
									newContact.City = contact.SelectSingleNode("./CITY").InnerText;

								}
								catch { }

								try
								{
									newContact.State = contact.SelectSingleNode("./STATE").InnerText;

								}
								catch { }
								try
								{
									newContact.Zip = contact.SelectSingleNode("./ZIP").InnerText;

								}
								catch { }

								try
								{
									newContact.Country = contact.SelectSingleNode("./COUNTRY").InnerText;
								}
								catch { }

								try
								{
									newContact.Phone = contact.SelectSingleNode("./PHONE").InnerText;

								}
								catch { }
								try
								{
									newContact.Fax = contact.SelectSingleNode("./FAX").InnerText;
								}
								catch { }

								try
								{
									newContact.Email = contact.SelectSingleNode("./EMAIL").InnerText;
								}
								catch { }

								try
								{
									newContact.ROID = contact.SelectSingleNode("./ROID").InnerText;
								}
								catch { }

								whoIs.Contacts.Add(newContact);
								newContact = null;
							}
						}
					}
					catch { }

					try
					{
						string[] tempStatus = doc.SelectSingleNode("//STATUS").InnerText.Split("|".ToCharArray());
						foreach (string theStatus in tempStatus)
						{
							if (!string.IsNullOrEmpty(theStatus))
							{
								WhoIsStatus status = new WhoIsStatus();
								status.StatusText = theStatus;

								whoIs.Status.Add(status);
								status = null;
							}
						}
					}
					catch { }


					try
					{
						string[] tempNameServers = doc.SelectSingleNode("//NS").InnerText.Split("|".ToCharArray());
						foreach (string ns in tempNameServers)
						{
							if (!string.IsNullOrEmpty(ns))
							{
								WhoIsNameServer nameServer = new WhoIsNameServer();
								nameServer.Server = ns;

								whoIs.NameServers.Add(nameServer);
								nameServer = null;
							}
						}
					}
					catch { }

					try
					{
						whoIs.RegistrarDescriptiveText = doc.SelectSingleNode("//TYPE1TEXT").InnerText;

					}
					catch { }

					try
					{
						try
						{
							whoIs.CreatedDate = DateTime.Parse(doc.SelectSingleNode("//DOMAIN/CRDATE").InnerText);
						}
						catch { }
						try
						{
							whoIs.ExpirationDate = DateTime.Parse(doc.SelectSingleNode("//DOMAIN/EXPDATE").InnerText);
						}
						catch { }
						try
						{
							whoIs.UpdateDate = DateTime.Parse(doc.SelectSingleNode("//DOMAIN/UPDATEDATE").InnerText);
						}
						catch { }
						try
						{
							string tempAvail = doc.SelectSingleNode("//DOMAIN/AVAIL").InnerText;
							whoIs.IsAvailable = tempAvail == "1";
						}
						catch { }
						try
						{
							string tempProxy = doc.SelectSingleNode("//DOMAIN/ISPROXIED").InnerText;
							whoIs.IsProxied = tempProxy == "1";
						}
						catch { }

					}
					catch { }
				}
			}
			return whoIs;
		}

		private string GetWhoIsInfo(WhoIsGetInfoRequestData whoisInfoRequestData, ConfigElement oConfig)
		{
		  string whoIsXml = string.Empty;
			string url = string.Empty;
			switch (whoisInfoRequestData.PrivateLabelId)
			{
				case 1:
					url = oConfig.GetConfigValue("WhoIs_Request_URL");
					break;
				case 2:
					url = oConfig.GetConfigValue("BR_WhoIs_Request_URL");
					break;
				default:
					url = oConfig.GetConfigValue("PL_WhoIs_Request_URL");
					break;
			}
			
			string requestUrl = url + "?querytype=5&domain=" + whoisInfoRequestData.DomainToLookup;
			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
      
			httpRequest.Timeout = (int)whoisInfoRequestData.RequestTimeout.TotalMilliseconds;
			httpRequest.UserAgent = "Atlantis Framework Fetch Request";

			HttpWebResponse webResponse = (HttpWebResponse)httpRequest.GetResponse();
      using (webResponse)
		  {
        Encoding enc = Encoding.GetEncoding("UTF-8");
        StreamReader responseStream = new StreamReader(webResponse.GetResponseStream(), enc);
        using (responseStream)
        {
          whoIsXml = responseStream.ReadToEnd();
        }
        responseStream.Close();
      }
			webResponse.Close();
	
			return whoIsXml;
		}
    
    #endregion
	}
}
