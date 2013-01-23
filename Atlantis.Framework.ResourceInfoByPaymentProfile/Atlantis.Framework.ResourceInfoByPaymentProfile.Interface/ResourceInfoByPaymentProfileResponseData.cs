using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ResourceInfoByPaymentProfile.Interface
{
  public class ResourceInfoByPaymentProfileResponseData : IResponseData
  {
    private AtlantisException _atlException = null;

    public ResourceInfoByPaymentProfileResponseData(int numberOfRecords, int numberOfPages, List<ResourceInfo> resourceList)
    {
      IsSuccess = true;
      TotalRecords = numberOfRecords;
      TotalPages = numberOfPages;
      GetResourceList = resourceList;
    }

    public ResourceInfoByPaymentProfileResponseData(AtlantisException exAtlantis)
    {
      IsSuccess = false;
      TotalRecords = 0;
      TotalPages = 0;
      _atlException = exAtlantis;
    }

    public ResourceInfoByPaymentProfileResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      TotalRecords = 0;
      TotalPages = 0;
      _atlException = new AtlantisException(oRequestData, "ResourceInfoByPaymentProfileResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess { get; private set; }

    public int TotalRecords { get; private set; }
    public int TotalPages { get; private set; }
    public List<ResourceInfo> GetResourceList { get; private set; }

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

        foreach (ResourceInfo r in GetResourceList)
        {
          writer.WriteStartElement("resource");

          writer.WriteAttributeString(ColumnName.WorkId, r.WorkId.ToString());
          writer.WriteAttributeString(ColumnName.ResourceId,r.ResourceId.ToString());
          writer.WriteAttributeString(ColumnName.Namespace, r.Namespace);
          writer.WriteAttributeString(ColumnName.ProfileId, r.ProfileId.ToString());
          writer.WriteAttributeString(ColumnName.ProductDescription, r.ProductDescription);
          writer.WriteAttributeString(ColumnName.Info, r.Info);
          writer.WriteAttributeString(ColumnName.BillingDate, r.BillingDate > DateTime.MinValue ? r.BillingDate.ToString("G") : string.Empty);
          writer.WriteAttributeString(ColumnName.OrderId, r.OrderId);
          writer.WriteAttributeString(ColumnName.RenewalSKU, r.RenewalSKU);
          writer.WriteAttributeString(ColumnName.IsLimited,  r.IsLimited > -1 ? r.IsLimited.ToString() : string.Empty);
          writer.WriteAttributeString(ColumnName.PFID, r.PFID.ToString());
          writer.WriteAttributeString(ColumnName.RecordToKeep, r.RecordToKeep > -1 ? r.RecordToKeep.ToString() : string.Empty);
          writer.WriteAttributeString(ColumnName.AutoRenewFlag, r.AutoRenewFlag.ToString());
          writer.WriteAttributeString(ColumnName.AllowRenewals, r.AllowRenewals.ToString());
          writer.WriteAttributeString(ColumnName.RecurringPayment, r.RecurringPayment);
          writer.WriteAttributeString(ColumnName.NumberOfPeriods, r.NumberOfPeriods.ToString());
          writer.WriteAttributeString(ColumnName.RenewalPFID, r.RenewalPFID.ToString());
          writer.WriteAttributeString(ColumnName.ProductTypeId, r.ProductTypeId.ToString());
          writer.WriteAttributeString(ColumnName.IsPastDue, r.IsPastDue.ToString());
          writer.WriteAttributeString(ColumnName.UsageStartDate, r.UsageStartDate > DateTime.MinValue ? r.UsageStartDate.ToString("G") : string.Empty);
          writer.WriteAttributeString(ColumnName.UsageEndDate, r.UsageEndDate > DateTime.MinValue ? r.UsageEndDate.ToString("G") : string.Empty);
          writer.WriteAttributeString(ColumnName.ExternalResourceId, r.ExternalResourceId);

          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      return sb.ToString();
    }

    #endregion
  }
}
