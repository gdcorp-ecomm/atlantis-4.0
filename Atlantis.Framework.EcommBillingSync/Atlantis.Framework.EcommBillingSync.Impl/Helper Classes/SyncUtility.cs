using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Atlantis.Framework.EcommBillingSync.Interface;
using Atlantis.Framework.Interface;
using gdOverrideLib;

namespace Atlantis.Framework.EcommBillingSync.Impl.Helper_Classes
{
  public class SyncUtility
  {
    public DateTime SetAnnualBillDate(List<BillingSyncProduct> billingSyncProducts, int syncMonth, int syncDay)
    {
      var newBillingDate = new DateTime();

      foreach (var bsp in billingSyncProducts)
      {
        var tempDate = GetNewExpirationDate(bsp.OriginalBillingDate, bsp.RecurringPaymentType, syncMonth, syncDay);
        if (tempDate > newBillingDate)
        {
          newBillingDate = tempDate;
        }
      }
      return newBillingDate;
    }

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
      const double daysPerYear = 365; // Leap years not factored
      double daysPerPeriod;

      switch (recurringType)
      {
        case "annual":
          daysPerPeriod = daysPerYear;
          break;
        case "monthly":
          daysPerPeriod = daysPerYear / 12;
          break;
        case "quarterly":
          daysPerPeriod = daysPerYear / 4;
          break;
        case "biannual":
          daysPerPeriod = daysPerYear * 2;
          break;
        case "triannual":
          daysPerPeriod = daysPerYear * 3;
          break;
        default:
          daysPerPeriod = daysPerYear;
          break;
      }
      var renewalDays = (newBillDate - oldBillDate).TotalDays;
      
      return (renewalDays / daysPerPeriod).ToString("N3");
    }

    public string GetDurationHash(int privateLabelId, int renewalPfId, double duration)
    {
			var overrideLib = new Duration();
			string hash;

			try
			{
				hash = overrideLib.GetHash(privateLabelId, renewalPfId, duration);
			}
			catch(Exception ex)
			{
			  var data = string.Format("PrivateLabelId: {0} | RenewalPfId: {1} | Duration: {2}", privateLabelId, renewalPfId, duration);
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
  }
}
