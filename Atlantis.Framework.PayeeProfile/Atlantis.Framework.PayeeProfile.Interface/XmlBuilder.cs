using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.PayeeProfileClass.Interface
{
  public class XmlBuilder
  {
    #region Private Constants
    private const string CheckingPaymentType = "2";
    private const string GAGPaymentType = "3";
    private const string PayPalPaymentType = "4";
    private const string W9AddressType = "W9";
    private const string PaymentAddressType = "Payment";
    #endregion

    public static string BuildAddPayeeXml(string shopperId, PayeeProfile payee)
    {
      XElement payeeXml = new XElement("AcctPayable",
        new XAttribute("shopperID", shopperId),
        new XAttribute("friendlyName", payee.FriendlyName),
        new XAttribute("taxDeclarationTypeID", payee.TaxDeclarationTypeID),
        new XAttribute("taxStatusTypeID", payee.TaxStatusTypeID),
        new XAttribute("taxStatusText", payee.TaxStatusText),
        new XAttribute("taxID", payee.TaxID),
        new XAttribute("taxIDTypeID", payee.TaxIDTypeID),
        new XAttribute("taxExemptTypeID", payee.TaxExemptTypeID),
        new XAttribute("taxCertificationTypeID", payee.TaxCertificationTypeID),
        new XAttribute("submitterName", payee.SubmitterName),
        new XAttribute("submitterTitle", payee.SubmitterTitle),
        new XAttribute("paymentMethodTypeID", payee.PaymentMethodTypeID));

      foreach (PayeeProfile.AddressClass addr in payee.Address)
      {
        XElement addressXml = new XElement("Address",
          new XAttribute("type", addr.AddressType),
          new XAttribute("contactName", addr.ContactName),
          new XAttribute("address1", addr.Address1),
          new XAttribute("address2", addr.Address2),
          new XAttribute("city", addr.City),
          new XAttribute("stateOrProvince", addr.StateOrProvince),
          new XAttribute("postalCode", addr.PostalCode),
          new XAttribute("country", addr.Country),
          new XAttribute("phone1", addr.Phone1));

        if (addr.AddressType == PaymentAddressType)
        {
          addressXml.Add(new XAttribute("phone2", addr.Phone2),
            new XAttribute("fax", addr.Fax));
        }
        payeeXml.Add(addressXml);
      }

      switch (payee.PaymentMethodTypeID)
      {
        case CheckingPaymentType:
          payeeXml.Add(new XAttribute("achBankName", payee.AchBankName),
            new XAttribute("achRTN", payee.AchRTN),
            new XAttribute("accountNumber", payee.AccountNumber),
            new XAttribute("accountOrganizationTypeID", payee.AccountOrganizationTypeID),
            new XAttribute("accountTypeID", payee.AccountTypeID));
          break;
        case GAGPaymentType:
          XElement gag = new XElement("GAG",
            new XAttribute("shopperID", shopperId));
          payeeXml.Add(gag);
          break;
        case PayPalPaymentType:
          XElement paypal = new XElement("PayPal",
            new XAttribute("email", payee.PayPalEmail));
          payeeXml.Add(paypal);
          break;
      }

      return payeeXml.ToString();
    }
  }
}
