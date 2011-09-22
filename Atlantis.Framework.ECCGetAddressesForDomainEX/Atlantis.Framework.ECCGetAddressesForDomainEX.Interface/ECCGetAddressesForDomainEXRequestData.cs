using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface.Constants;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAddressesForDomainEX.Interface
{
  public class ECCGetAddressesForDomainEXRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public string DomainName { get; set; }
    public int EmailType { get; private set; }
    public bool Active { get; private set; }
    public string SubAccount { get; private set; }

    private List<string> _fields = new List<string>();
    public List<string> Fields { get { return _fields; } }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
   
    public ECCGetAddressesForDomainEXRequestData(string shopperId,
      int privateLabelId,
      int emailType,
      bool active,
      string domainName,
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
      DomainName = domainName;
      _fields.Add(ECCExtendedDataFields.STATUS);
      _fields.Add(ECCExtendedDataFields.DELIVERY_MODE);
    }

    public ECCGetAddressesForDomainEXRequestData(string shopperId,
      int privateLabelId,
      int emailType,
      bool active,
      string domainName,
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
      DomainName = domainName;
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
      throw new Exception("ECCGetAddressesForDomainEX is not a cacheable request");
    }

    #endregion
  }
}
