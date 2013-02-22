using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Atlantis.Framework.EcommBillingSync.Interface;
using Atlantis.Framework.EcommProductAddOns.Interface;
using Atlantis.Framework.Interface;
using gdOverrideLib;
using BillingSyncErrorType = Atlantis.Framework.EcommBillingSync.Interface.BillingSyncErrorData.BillingSyncErrorType;

namespace Atlantis.Framework.EcommBillingSync.Impl.Helper_Classes
{
  public class SyncUtility
  {
    #region Expiration Date & Duration Calculations 
    public DateTime GetNewExpirationDate(DateTime originalDate, string recurringPaymentType, int syncMonth, int syncDay)
    {
      // Make sure that the day of month supplied is valid, 1998 (non leap year) chosen at random
      if (syncDay > DateTime.DaysInMonth(1998, syncMonth))
      {
        syncDay = DateTime.DaysInMonth(1998, syncMonth);
      }

      var newBillDate = originalDate;
      while (newBillDate.Day != syncDay)
      {
        newBillDate = newBillDate.AddDays(1);
      }
      switch (recurringPaymentType)
      {
        case "monthly":
          newBillDate = newBillDate.AddMonths(1);
          break;
        case "quarterly":
          newBillDate = newBillDate.AddMonths(3);
          break;
        default:  // annual is default
          while (newBillDate.Month != syncMonth)
          {
            newBillDate = newBillDate.AddMonths(1);
          }
          newBillDate = newBillDate.AddYears(1);
          break;
      }
      return newBillDate;
    }
    
    /// <summary>
    /// On the service's end, a validation is done against TargetExpirationDate and the duration we send.  Because the duration has limited precision,
    /// (3 decimal places), there are plus/minus 7 days leeway.  The following code closely mirrors that of the service for consistency.
    /// </summary>
    /// <param name="oldBillDate"></param>
    /// <param name="newBillDate"></param>
    /// <param name="recurringType"></param>
    /// <returns></returns>
    public string GetDuration(DateTime oldBillDate, DateTime newBillDate, string recurringType)
    {
      const double DAYS_PER_YEAR = 365; // Leap years not factored
      double daysPerPeriod;

      switch (recurringType)
      {
        case "annual":
          daysPerPeriod = DAYS_PER_YEAR;
          break;
        case "monthly":
          daysPerPeriod = DAYS_PER_YEAR / 12;
          break;
        case "quarterly":
          daysPerPeriod = DAYS_PER_YEAR / 4;
          break;
        case "biannual":
          daysPerPeriod = DAYS_PER_YEAR * 2;
          break;
        case "triannual":
          daysPerPeriod = DAYS_PER_YEAR * 3;
          break;
        default:
          daysPerPeriod = DAYS_PER_YEAR;
          break;
      }
      var renewalDays = (newBillDate - oldBillDate).TotalDays;
      
      return (renewalDays / daysPerPeriod).ToString("N3");
    }

    public string GetDurationHash(EcommBillingSyncRequestData ebsRequest, int renewalProductId, double duration, BillingSyncProduct bsp)
    {
			var overrideLib = new Duration();
			string hash;

			try
			{
				hash = overrideLib.GetHash(ebsRequest.PrivateLabelId, renewalProductId, duration);
			}
			catch(Exception ex)
			{
        var data = string.Format("PrivateLabelId: {0} | RenewalProductId: {1} | Duration: {2}", ebsRequest.PrivateLabelId, renewalProductId, duration);
        ebsRequest.BillingSyncErrors.Add(new BillingSyncErrorData(BillingSyncErrorType.GdOverrideLibSvcError, ex, bsp));
        throw new AtlantisException("EcommBillingSync.Helper_Classes::GetDurationHash", "0", ex.Message, data, null, null);
			}
			finally
			{
			  if (Marshal.IsComObject(overrideLib))
			  {
			    Marshal.ReleaseComObject(overrideLib);
			  }
			}

			return hash;      
    }
    #endregion

    #region Multi-year Renewal ProductId Retrieval
    public int GetRenewalProductId(int renewalProductId, string recurringPaymentType, int privateLabelId)
    {
      const string BILLING_SYNC_CALL = "<BillingSyncProductList><param name=\"n_pf_id\" value=\"{0}\"/><param name=\"n_privatelabelResellerTypeID\" value=\"{1}\"/></BillingSyncProductList>";
      var correctRenewalProductId = 0;

      var productList = DataCache.DataCache.GetCacheData(string.Format(BILLING_SYNC_CALL, renewalProductId, privateLabelId));

      var xDoc = XDocument.Parse(productList);

      var dataElement = xDoc.Element("data");
      if (dataElement != null)
      {
        var renewalItems = dataElement.Elements("item").Where(item => item.Attribute("isRenewal").Value == "1");
        var xElements = renewalItems as IList<XElement> ?? renewalItems.ToList();
        var renewalItemsList = xElements.ToList();
        var onePeriodMatchingRecurringRenewalItem = renewalItemsList.Find(ri => ri.Attribute("numberOfPeriods").Value == "1" && String.Compare(ri.Attribute("recurring_payment").Value, recurringPaymentType, StringComparison.OrdinalIgnoreCase) == 0);
        correctRenewalProductId = Convert.ToInt32(onePeriodMatchingRecurringRenewalItem.Attribute("catalog_productUnifiedProductID").Value);
      }
      return correctRenewalProductId;
    }
    #endregion

    #region Process AddOns
    public bool BillingSyncProductHasAddOns(EcommBillingSyncRequestData ebsRequest, BillingSyncProduct bsp, int privateLabelId, int index, string duration, out List<AddToCartItem> addOnItems, out string addOnGuidId)
    {
      var hasAddOns = false;
      addOnItems = new List<AddToCartItem>();
      addOnGuidId = string.Empty;

      //Configuration Proc Missing for NameSpace: cashparkHy - so don't check for this product
      if (!string.IsNullOrEmpty(bsp.OrionId) && String.Compare(bsp.Namespace, "cashparkHy", StringComparison.OrdinalIgnoreCase) != 0)
      {
        try
        {
          var request = new EcommProductAddOnsRequestData(ebsRequest.ShopperID
            , ebsRequest.SourceURL
            , ebsRequest.OrderID
            , ebsRequest.Pathway
            , ebsRequest.PageCount
            , ebsRequest.PrivateLabelId
            , bsp.OrionId
            , bsp.Namespace);

          var response = (EcommProductAddOnsResponseData)Engine.Engine.ProcessRequest(request, ebsRequest.EcommProductAddOnsRequestType);

          if (response.IsSuccess && response.HasAddOns)
          {
            hasAddOns = true;
            addOnGuidId = Guid.NewGuid().ToString();
            addOnItems = CreateAddOnItems(ebsRequest, response.AddOnProducts, bsp, addOnGuidId, index, duration);
          }
        }
        catch (Exception ex)
        {
          var data = string.Format("PrivateLabelId: {0} | OrionId: {1} | ResourceType: {2}", privateLabelId, bsp.OrionId, bsp.Namespace);
          ebsRequest.BillingSyncErrors.Add(new BillingSyncErrorData(BillingSyncErrorType.BillingStateSvcError, ex, bsp));
          throw new AtlantisException("EcommBillingSync.Helper_Classes::BillingSyncProductHasAddOns", "0", ex.Message, data, null, null);
        }
      }
      return hasAddOns;
    }

    private List<AddToCartItem> CreateAddOnItems(EcommBillingSyncRequestData ebsRequest, IEnumerable<AddOnProduct> addOnProducts, BillingSyncProduct bsp, string guid, int index, string duration)
    {
      const int QUANTITY = 1;
      var addToCartItems = new List<AddToCartItem>();

      foreach (var addOnProduct in addOnProducts)
      {
        var durationHash = GetDurationHash(ebsRequest, addOnProduct.RenewalUnifiedProductId, Convert.ToDouble(duration), bsp);
        var item = new AddToCartItem(string.Format("{0}_{1}", bsp.BillingResourceId, index), addOnProduct.RenewalUnifiedProductId, bsp.BillingResourceId, QUANTITY, ebsRequest.ItemTrackingCode, duration, durationHash);

        // Only add Guid Info. Do not add TARGET_EXPIRATION_DATE for any add-ons. Will cause cart to fail on 
        // duration as duration validation compares target date to this billing date, which is null for add-ons.
        item[AddToCartItemProperty.GROUP_ID] = guid;
        item[AddToCartItemProperty.PARENT_GROUP_ID] = guid;
        addToCartItems.Add(item);
      }

      return addToCartItems;
    }
    #endregion 

    #region Custom XML
    public string BuildCustomXmlIfNecessary(string nameSpace, string productNameInfo, int resourceId, string duration)
    {
      List<string> nameParts;
      var customXml = string.Empty;

      switch (nameSpace)
      {
        case "proxima":
          nameParts = GetDomainNameParts(nameSpace, productNameInfo, resourceId);
          customXml = BuildCustomXml("ProximaRenewal", nameParts, duration, "resource_id", resourceId);
          break;
        case "smrtdomain":
          nameParts = GetDomainNameParts(nameSpace, productNameInfo, resourceId);
          customXml = BuildCustomXml("smartDomainRenewal", nameParts, "1", "resourceid", resourceId);
          break;
        case "domainRep":
          nameParts = GetDomainNameParts(nameSpace, productNameInfo, resourceId);
          customXml = BuildCustomXml("certifiedDomainRenewal", nameParts, duration, "resource_id", resourceId);
          break;
        case "cashparkHy":
          nameParts = GetDomainNameParts(nameSpace, productNameInfo, resourceId);
          customXml = BuildCustomXml("hybridParkingRenewal", nameParts, duration, "resource_id", resourceId);
          break;
      }

      return customXml;
    }

    private static string BuildCustomXml(string root, IList<string> nameParts, string duration, string resourceIdName, int resourceIdValue)
    {
      var customXml = new XElement(root,
                        new XElement("domain",
                          new XAttribute("sld", nameParts[0]),
                          new XAttribute("tld", nameParts[1]),
                          new XAttribute("duration", duration),
                          new XAttribute(resourceIdName, resourceIdValue)));

      return customXml.ToString();
    }

    private static List<string> GetDomainNameParts(string nameSpace, string productNameInfo, int resourceId)
    {
      var nameParts = new List<string>();
      nameParts.AddRange(productNameInfo.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries));
      if (nameParts.Count < 2)
      {
        if (nameSpace.Equals("smrtDomain"))
        {
          var data = string.Format("Namespace: {0} | CommonName: {1} | ResourceId: {2}", nameSpace, productNameInfo, resourceId);
          throw new AtlantisException("EcommBillingSync.Helper_Classes::BuildCustomXmlIfNecessary", "0", "Invalid Domain", data, null, null);
        }
        nameParts.Add(string.Empty);
        nameParts.Add(string.Empty);
      }

      if (nameParts.Count > 2)
      {
        nameParts.RemoveRange(0, nameParts.Count - 2);
      }

      return nameParts;
    }
    #endregion
  }
}
