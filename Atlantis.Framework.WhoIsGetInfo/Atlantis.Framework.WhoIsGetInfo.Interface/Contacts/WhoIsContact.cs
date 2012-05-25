using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.WhoIsGetInfo.Interface.Contacts
{
	public class WhoIsContact
	{
		public WhoIsContactTypes Type { get; set; }
		public  string ROID { get; set; }
		public string FirstName { get; set;}
		public string LastName { get; set;}
		public string Email { get; set;}
		public string Company { get; set;}
		public string Address1 { get; set;}
		public string Address2 { get; set;}
		public string City { get; set;}
		public string State { get; set;}
		public string Zip { get; set;}
		public string Country { get; set;}
		public string Phone { get; set;}
		public string Fax { get; set;}
	}
}
