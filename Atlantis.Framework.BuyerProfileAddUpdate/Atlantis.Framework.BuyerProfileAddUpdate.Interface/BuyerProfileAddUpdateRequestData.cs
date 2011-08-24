using System;
using System.Text;
using System.Xml;
using Atlantis.Framework.BuyerProfileDetails.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyerProfileAddUpdate.Interface
{
  public class BuyerProfileAddUpdateRequestData : RequestData
  {
 
    public BuyerProfileAddUpdateRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
    }

    public ProfileDetail BuyerProfile
    {
      get;
      set;
    }

    public bool IsNewBuyerProfile
    {
      get;
      set;
    }

    public string ProfileID
    {
      get;
      set;
    }

    #region Request Data Members

    public override string GetCacheMD5()
    {
      throw new Exception("BuyerProfileAddUpdate is not a cacheable request.");
    }

    public override string ToXML()
    {
      string addOrUpdate = IsNewBuyerProfile ? "InsertProfile" : "UpdateProfile";
      string profileId = IsNewBuyerProfile ? string.Empty : ProfileID;
      StringBuilder sb = new StringBuilder();

      if (BuyerProfile != null)
      {
        using (XmlWriter writer = XmlWriter.Create(sb))
        {
          writer.WriteStartElement("messageXml");
          writer.WriteAttributeString("namespace", "buyerProfile");
          writer.WriteAttributeString("type", addOrUpdate);

          writer.WriteStartElement("BuyerProfiles");

          writer.WriteStartElement("BuyerProfile");
          writer.WriteAttributeString("shopperID", ShopperID);
          writer.WriteAttributeString("profileID", profileId);
          writer.WriteAttributeString("profileName", BuyerProfile.ProfileName);
          writer.WriteAttributeString("registrationYears", BuyerProfile.RegLength.ToString());
          writer.WriteAttributeString("autoRenew", Convert.ToInt32(BuyerProfile.AutoRenew).ToString());
          writer.WriteAttributeString("parkDNS", Convert.ToInt32(BuyerProfile.ParkDNS).ToString());
          writer.WriteAttributeString("quickCheckoutFlag", Convert.ToInt32(BuyerProfile.QuickCheckoutFlag).ToString());
          writer.WriteAttributeString("defaultProfileFlag", Convert.ToInt32(BuyerProfile.DefaultProfileFlag).ToString());

          writer.WriteStartElement("contacts");

          if (BuyerProfile.BuyerProfileAddressList.Count > 0)
          {
            foreach (AddressList address in BuyerProfile.BuyerProfileAddressList)
            {
              writer.WriteStartElement("contact");
              writer.WriteAttributeString("type", address.ContactTypeId.ToString());
              writer.WriteAttributeString("firstName", address.FirstName);
              writer.WriteAttributeString("middleName", address.MiddleName);
              writer.WriteAttributeString("lastName", address.LastName);
              writer.WriteAttributeString("organization", address.Organization);
              writer.WriteAttributeString("address1", address.Address1);
              writer.WriteAttributeString("address2", address.Address2);
              writer.WriteAttributeString("city", address.City);
              writer.WriteAttributeString("stateOrProvince", address.StateOrProvince);
              writer.WriteAttributeString("zipCode", address.ZipCode);
              writer.WriteAttributeString("country", address.Country);
              writer.WriteAttributeString("daytimePhone", address.DaytimePhone);
              writer.WriteAttributeString("eveningPhone", address.EveningPhone);
              writer.WriteAttributeString("fax", address.Fax);
              writer.WriteAttributeString("email", address.Email);
              writer.WriteEndElement();
            }
          }

          writer.WriteEndElement(); // contacts
          writer.WriteStartElement("nameservers");

          if (BuyerProfile.HostNameList.Count > 0)
          {
            foreach (string nameServer in BuyerProfile.HostNameList)
            {
              writer.WriteStartElement("server");
              writer.WriteAttributeString("name", nameServer);
              writer.WriteEndElement();
            }
          }

          writer.WriteEndElement(); // nameservers
          writer.WriteEndElement(); // BuyerProfile
          writer.WriteEndElement(); // BuyerProfiles
          writer.WriteFullEndElement(); // messageXml
        }
      }

      return sb.ToString();
    }

    #endregion

  }
}
