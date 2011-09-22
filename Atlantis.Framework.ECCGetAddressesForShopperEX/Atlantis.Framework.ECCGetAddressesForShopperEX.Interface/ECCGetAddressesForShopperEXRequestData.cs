using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Ecc.Interface.Constants;

namespace Atlantis.Framework.ECCGetAddressesForShopperEX.Interface
{
  public class ECCGetAddressesForShopperEXRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public int EmailType { get; private set; }
    public bool Active { get; private set; }
    public string SubAccount { get; private set; }

    private List<string> _fields = new List<string>();
    public List<string> Fields { get { return _fields; } }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
 

    public ECCGetAddressesForShopperEXRequestData(string shopperId,
      int privateLabelId,
      int emailType,
      bool active,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      base.RequestTimeout = _requestTimeout;
      PrivateLabelId = privateLabelId;
      EmailType = emailType;
      Active = active;
      _fields.Add(ECCExtendedDataFields.STATUS);
      _fields.Add(ECCExtendedDataFields.DELIVERY_MODE);
    }

    public ECCGetAddressesForShopperEXRequestData(string shopperId,
      int privateLabelId,
      int emailType,
      bool active,
      IEnumerable<string> fields,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      base.RequestTimeout = _requestTimeout;
      PrivateLabelId = privateLabelId;
      EmailType = emailType;
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
      throw new Exception("ECCGetAddressesForShopperEX is not a cacheable request");
    }

    #endregion
  }
}
