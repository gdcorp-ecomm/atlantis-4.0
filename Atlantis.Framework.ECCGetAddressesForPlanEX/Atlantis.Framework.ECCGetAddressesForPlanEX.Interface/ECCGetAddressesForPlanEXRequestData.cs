using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface.Constants;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAddressesForPlanEX.Interface
{
  public class ECCGetAddressesForPlanEXRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public string AccountUid { get; set; }
    public bool Active { get; private set; }
    public string SubAccount { get; private set; }

    private List<string> _fields = new List<string>();
    public List<string> Fields { get { return _fields; } }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
   
    public ECCGetAddressesForPlanEXRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, string accountUid, bool active)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      base.RequestTimeout = _requestTimeout;
      PrivateLabelId = privateLabelId;
      AccountUid = accountUid;
      Active = active;
      _fields.Add(ECCExtendedDataFields.STATUS);
      _fields.Add(ECCExtendedDataFields.DELIVERY_MODE);
    }

    public ECCGetAddressesForPlanEXRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int privateLabelId, string accountUid, IEnumerable<string> fields, bool active)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {

      base.RequestTimeout = _requestTimeout;
      PrivateLabelId = privateLabelId;
      AccountUid = accountUid;
      Active = active;
      _fields.Add(ECCExtendedDataFields.STATUS);
      _fields.Add(ECCExtendedDataFields.DELIVERY_MODE);
      AddFields(fields);
    }

    public void AddFields(IEnumerable<string> fields)
    {
      if (fields != null)
      {
        foreach (string field in fields)
        {
          if (!_fields.Contains(field))
          {
            _fields.Add(field);
          }
        }
      }
    }

    public void AddField(string sField)
    {
      if (sField != null)
      {
        if (!_fields.Contains(sField))
        {
          _fields.Add(sField);
        }
      }
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetAddressesForPlanEX is not a cacheable request");
    }

    #endregion
  }
}

