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

      foreach (PayeeProfile.AddressClass address in payee.Address)
      {
        payeeXml.Add(BuildAddressXml(address));
      }

      BuildPaymentMethodXml(shopperId, payee, ref payeeXml, false);

      return payeeXml.ToString();
    }

    public static string BuildUpdatePayeeXml(string shopperId, PayeeProfile originalPayee, PayeeProfile updatedPayee)
    {
      XElement payeeXml = new XElement("AcctPayable",
        new XAttribute("shopperID", shopperId),
        new XAttribute("capID", originalPayee.CapID));

      if (originalPayee.FriendlyName != updatedPayee.FriendlyName)
      {
        payeeXml.Add(new XAttribute("friendlyName", updatedPayee.FriendlyName));
      }
      if (originalPayee.TaxDeclarationTypeID != updatedPayee.TaxDeclarationTypeID)
      {
        payeeXml.Add(new XAttribute("taxDeclarationTypeID", updatedPayee.TaxDeclarationTypeID));
      }
      if (originalPayee.TaxStatusTypeID != updatedPayee.TaxStatusTypeID)
      {
        payeeXml.Add(new XAttribute("taxStatusTypeID", updatedPayee.TaxStatusTypeID));
      }
      if (originalPayee.TaxStatusText != updatedPayee.TaxStatusText)
      {
        payeeXml.Add(new XAttribute("taxStatusText", updatedPayee.TaxStatusText));
      }
      if (originalPayee.TaxID != updatedPayee.TaxID && updatedPayee.TaxID != null)
      {
        payeeXml.Add(new XAttribute("taxID", updatedPayee.TaxID));
      }
      if (originalPayee.TaxIDTypeID != updatedPayee.TaxIDTypeID || (originalPayee.TaxID != updatedPayee.TaxID && updatedPayee.TaxID != null))
      {
        payeeXml.Add(new XAttribute("taxIDTypeID", updatedPayee.TaxIDTypeID));
      }
      if (originalPayee.TaxExemptTypeID != updatedPayee.TaxExemptTypeID || (originalPayee.TaxID != updatedPayee.TaxID && updatedPayee.TaxID != null))
      {
        payeeXml.Add(new XAttribute("taxExemptTypeID", updatedPayee.TaxExemptTypeID));
      }
      if (originalPayee.TaxCertificationTypeID != updatedPayee.TaxCertificationTypeID || (originalPayee.TaxID != updatedPayee.TaxID && updatedPayee.TaxID != null))
      {
        payeeXml.Add(new XAttribute("taxCertificationTypeID", updatedPayee.TaxCertificationTypeID));
      }
      if (originalPayee.SubmitterName != updatedPayee.SubmitterName)
      {
        payeeXml.Add(new XAttribute("submitterName", updatedPayee.SubmitterName));
      }
      if (originalPayee.SubmitterTitle != updatedPayee.SubmitterTitle)
      {
        payeeXml.Add(new XAttribute("submitterTitle", updatedPayee.SubmitterTitle));
      }

      // Per Ecomm - Always update PaymentMethodTypeId
      payeeXml.Add(new XAttribute("paymentMethodTypeID", updatedPayee.PaymentMethodTypeID));

      foreach (PayeeProfile.AddressClass address in updatedPayee.Address)
      {
        payeeXml.Add(BuildAddressXml(address));
      }

      BuildPaymentMethodXml(shopperId, originalPayee, updatedPayee, ref payeeXml, true);

      return payeeXml.ToString();
    }

    #region Xml Component Builders
    private static void BuildPaymentMethodXml(string shopperId, PayeeProfile payee, ref XElement payeeXml, bool isUpdate)
    {
      BuildPaymentMethodXml(shopperId, new PayeeProfile(), payee, ref payeeXml, false);
    }

    private static void BuildPaymentMethodXml(string shopperId, PayeeProfile originalPayee, PayeeProfile payee, ref XElement payeeXml, bool isUpdate)
    {
      switch (payee.PaymentMethodTypeID)
      {
        case GAGPaymentType:
          XElement gag = new XElement("GAG",
            new XAttribute("shopperID", shopperId));
          payeeXml.Add(gag);
          break;
        case PayPalPaymentType:
          XElement paypal = new XElement("PayPal",
            new XAttribute("email", payee.PayPal.Email));
          payeeXml.Add(paypal);
          break;
        case CheckingPaymentType:
          if (isUpdate)
          {
            if (originalPayee.ACH.AchBankName != payee.ACH.AchBankName)
            {
              payeeXml.Add(new XAttribute("achBankName", payee.ACH.AchBankName));
            }
            if (originalPayee.ACH.AchRTN != payee.ACH.AchRTN)
            {
              payeeXml.Add(new XAttribute("achRTN", payee.ACH.AchRTN));
            }
            if (originalPayee.ACH.AccountNumber != payee.ACH.AccountNumber)
            {
              payeeXml.Add(new XAttribute("accountNumber", payee.ACH.AccountNumber));
            }
            if (originalPayee.ACH.AccountOrganizationTypeID != payee.ACH.AccountOrganizationTypeID)
            {
              payeeXml.Add(new XAttribute("accountOrganizationTypeID", payee.ACH.AccountOrganizationTypeID));
            }
            if (originalPayee.ACH.AccountTypeID != payee.ACH.AccountTypeID)
            {
              payeeXml.Add(new XAttribute("accountTypeID", payee.ACH.AccountTypeID));
            }
          }
          else
          {
            XElement ach = new XElement("ACH",
              new XAttribute("achBankName", payee.ACH.AchBankName),
              new XAttribute("achRTN", payee.ACH.AchRTN),
              new XAttribute("accountNumber", payee.ACH.AccountNumber),
              new XAttribute("accountOrganizationTypeID", payee.ACH.AccountOrganizationTypeID),
              new XAttribute("accountTypeID", payee.ACH.AccountTypeID));
            payeeXml.Add(ach);
          }
          break;
      }
    }

    private static XElement BuildAddressXml(PayeeProfile.AddressClass address)
    {
      XElement addressXml = new XElement("Address",
        new XAttribute("type", address.AddressType),
        new XAttribute("contactName", address.ContactName),
        new XAttribute("address1", address.Address1),
        new XAttribute("address2", address.Address2),
        new XAttribute("city", address.City),
        new XAttribute("stateOrProvince", address.StateOrProvince),
        new XAttribute("postalCode", address.PostalCode),
        new XAttribute("country", address.Country),
        new XAttribute("phone1", address.Phone1));

      if (address.AddressType == PaymentAddressType)
      {
        addressXml.Add(new XAttribute("phone2", address.Phone2),
          new XAttribute("fax", address.Fax));
      }

      return addressXml;
    }
    #endregion
  }
}
