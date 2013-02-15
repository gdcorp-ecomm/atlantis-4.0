using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductBillingResourceInfo.Interface
{
  public class ProductBillingResourceInfoResponseData : IResponseData
  {
    private AtlantisException _atlException = null;

    public ProductBillingResourceInfoResponseData(int numberOfRecords, int numberOfPages, List<BillingResourceInfo> resourceList)
    {
      IsSuccess = true;
      TotalRecords = numberOfRecords;
      TotalPages = numberOfPages;
      GetBillingResourceList = resourceList;
    }

    public ProductBillingResourceInfoResponseData(AtlantisException exAtlantis)
    {
      IsSuccess = false;
      TotalRecords = 0;
      TotalPages = 0;
      _atlException = exAtlantis;
    }

    public ProductBillingResourceInfoResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      TotalRecords = 0;
      TotalPages = 0;
      _atlException = new AtlantisException(oRequestData, "ProductBillingResourceInfoResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess { get; private set; }

    public int TotalRecords { get; private set; }
    public int TotalPages { get; private set; }
    public List<BillingResourceInfo> GetBillingResourceList { get; private set; }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      StringBuilder sb = new StringBuilder();

      using (XmlWriter writer = XmlWriter.Create(sb))
      {
        writer.WriteStartElement("resources");

        foreach (BillingResourceInfo r in GetBillingResourceList)
        {
          writer.WriteStartElement("resource");

          writer.WriteAttributeString(ColumnName.ResourceId,r.ResourceId.ToString());
          writer.WriteAttributeString(ColumnName.Namespace, r.Namespace);
          writer.WriteAttributeString(ColumnName.ProfileId, r.ProfileId.ToString());
          writer.WriteAttributeString(ColumnName.ProductDescription, r.ProductDescription);
          writer.WriteAttributeString(ColumnName.Info, r.Info);
          writer.WriteAttributeString(ColumnName.BillingDate, r.BillingDate > DateTime.MinValue ? r.BillingDate.ToString("G") : string.Empty);
          writer.WriteAttributeString(ColumnName.OrderId, r.OrderId);
          writer.WriteAttributeString(ColumnName.RenewalSKU, r.RenewalSKU);
          writer.WriteAttributeString(ColumnName.IsLimited,  r.IsLimited > -1 ? r.IsLimited.ToString() : string.Empty);
          writer.WriteAttributeString(ColumnName.AutoRenewFlag, r.AutoRenewFlag.ToString());
          writer.WriteAttributeString(ColumnName.PFID, r.PFID.ToString());
          writer.WriteAttributeString(ColumnName.AllowRenewals, r.AllowRenewals.ToString());
          writer.WriteAttributeString(ColumnName.RecurringPayment, r.RecurringPayment);
          writer.WriteAttributeString(ColumnName.NumberOfPeriods, r.NumberOfPeriods.ToString());
          writer.WriteAttributeString(ColumnName.RenewalPFID, r.RenewalPFID.ToString());
          writer.WriteAttributeString(ColumnName.IsPastDue, r.IsPastDue.ToString());
          writer.WriteAttributeString(ColumnName.UsageStartDate, r.UsageStartDate > DateTime.MinValue ? r.UsageStartDate.ToString("G") : string.Empty);
          writer.WriteAttributeString(ColumnName.UsageEndDate, r.UsageEndDate > DateTime.MinValue ? r.UsageEndDate.ToString("G") : string.Empty);
          writer.WriteAttributeString(ColumnName.ExternalResourceId, r.ExternalResourceId);
          writer.WriteAttributeString(ColumnName.PurchasedDuration, r.PurchasedDuration.ToString());
          writer.WriteAttributeString(ColumnName.ProductTypeId, r.ProductTypeId.ToString());
          writer.WriteAttributeString(ColumnName.IsPrivacyPlusDomain, r.IsPrivacyPlusDomain.ToString());
          writer.WriteAttributeString(ColumnName.MostRecentRenewalOrderId, r.MostRecentRenewalOrderId);
          writer.WriteAttributeString(ColumnName.MostRecentRenewalOrderDate, r.MostRecentRenewalOrderDate > DateTime.MinValue? r.MostRecentRenewalOrderDate.ToString("G") : string.Empty);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      return sb.ToString();
    }

    #endregion
  }
}
