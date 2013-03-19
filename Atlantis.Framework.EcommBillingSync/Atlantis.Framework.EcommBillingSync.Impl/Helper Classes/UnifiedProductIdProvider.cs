using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.EcommBillingSync.Interface;
using Atlantis.Framework.Interface;
using BillingSyncErrorType = Atlantis.Framework.EcommBillingSync.Interface.BillingSyncErrorData.BillingSyncErrorType;

namespace Atlantis.Framework.EcommBillingSync.Impl.Helper_Classes
{
  public class UnifiedProductIdProvider
  {
    #region Constants & Properties
    private const string GET_UNIFIED_ID_BY_PFID_REQUEST_FORMAT = "<GetUnifiedIDByPFID><param name=\"n_pf_id\" value=\"{0}\"/><param name=\"n_privateLabelID\" value=\"{1}\"/></GetUnifiedIDByPFID>";

    private Dictionary<string, int> _pfidToUnifiedProductIdLookup;
    private Dictionary<string, int> PfidToUnifiedProductIdLookup
    {
      get { return _pfidToUnifiedProductIdLookup ?? (_pfidToUnifiedProductIdLookup = new Dictionary<string, int>()); }
    }

    private EcommBillingSyncRequestData EbsRequest { get; set; }
    #endregion

    public int GetUnifiedProductIdByPfid(int pfid, int privateLabelId, EcommBillingSyncRequestData ebsRequest)
    {

      var key = GetUnifiedProductIdLookupKey(pfid, privateLabelId);
      var result = PfidToUnifiedProductIdLookup.ContainsKey(key) ? PfidToUnifiedProductIdLookup[key] : GetUnifiedProductIdByPfidFromDataCache(pfid, privateLabelId);

      return result;
    }

    #region Private Methods
    private int GetUnifiedProductIdByPfidFromDataCache(int pfid, int privateLabelId)
    {
      var unifiedProductId = int.MinValue;
      var requestXml = string.Format(GET_UNIFIED_ID_BY_PFID_REQUEST_FORMAT, pfid, privateLabelId);

      try
      {
        var resultXml = DataCache.DataCache.GetCacheData(requestXml);

        if (!string.IsNullOrEmpty(resultXml))
        {
          var xdoc = XDocument.Parse(resultXml);

          var resultProductIdString = (from item in xdoc.Descendants("item") select item.Attribute("gdshop_product_unifiedProductID").Value).FirstOrDefault();

          if (!string.IsNullOrEmpty(resultProductIdString))
          {
            if (!int.TryParse(resultProductIdString, out unifiedProductId))
            {
              unifiedProductId = int.MinValue;
            }
            else
            {
              PfidToUnifiedProductIdLookup.Add(GetUnifiedProductIdLookupKey(pfid, privateLabelId), unifiedProductId);
            }
          }
        }
      }
      catch (Exception ex)
      {
        var data = ex.Message + Environment.NewLine + ex.StackTrace;
        EbsRequest.BillingSyncErrors.Add(new BillingSyncErrorData(BillingSyncErrorType.UnifiedProductIdLookupError, ex));
        throw new AtlantisException("EcommBillingSync.Helper_Classes::UnifiedProductIdProvider", "0", ex.Message, data, null, null);
      }

      return unifiedProductId;
    }

    private static string GetUnifiedProductIdLookupKey(int pfid, int privateLabelId)
    {
      return string.Concat(pfid.ToString(), "-", privateLabelId.ToString());
    }
    #endregion

  }
}
