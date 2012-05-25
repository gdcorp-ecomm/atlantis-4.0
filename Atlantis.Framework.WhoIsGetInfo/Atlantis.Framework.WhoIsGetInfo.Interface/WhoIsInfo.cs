using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.WhoIsGetInfo.Interface.Contacts;
using Atlantis.Framework.WhoIsGetInfo.Interface.Nameservers;
using Atlantis.Framework.WhoIsGetInfo.Interface.Status;

namespace Atlantis.Framework.WhoIsGetInfo.Interface
{
	public class WhoIsInfo: IComparable<WhoIsInfo>
	{
		public string Name { get; set;}
		public string WhoIsServer { get; set;}
		public string Registrar { get; set;}
		public string RegistrarId { get; set;}
		public DateTime CreatedDate { get; set;}
		public DateTime ExpirationDate { get; set;}
		public DateTime UpdateDate { get; set;}
		public List<WhoIsContact> Contacts { get; set; }
		public List<WhoIsNameServer> NameServers { get; set;}
		public List<WhoIsStatus> Status { get; set;}
		public bool IsAvailable { get; set;}
		public string PrivateLabelId { get; set;}
		public string Reseller { get; set; }
		public bool IsProxied{ get; set;}
		public string RegistrarDescriptiveText{ get; set;}

		public bool IsValid { get; set; }
		public string ResponseCode { get; set; }
		public string ResponseDescription { get; set;}

		public XDocument RawResults { get; set; }

		public WhoIsInfo()
		{
			this.Contacts = new List<WhoIsContact>();
			this.Status = new List<WhoIsStatus>();
			this.NameServers = new List<WhoIsNameServer>();
		}

		public int CompareTo(WhoIsInfo other)
		{
			return String.Compare(Name, other.Name);
		}

		
	}
}
