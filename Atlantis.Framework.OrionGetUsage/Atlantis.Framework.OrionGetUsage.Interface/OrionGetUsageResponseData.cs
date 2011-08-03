using System;
using System.Xml.Linq;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionGetUsage.Interface
{
  public class OrionGetUsageResponseData : IResponseData
  {
    private readonly AtlantisException _exception;

    public bool IsSuccess { get; private set; }
    public string AccountUID { get; private set; }
    public decimal Amount { get; private set; }
    public string BaseQuotaElementUID { get; private set; }
    public decimal CurrentBaseQuotaAllowed { get; private set; }
    public decimal CurrentExtendedQuotaAllowed { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime FirstReportedUsage { get; private set; }
    public DateTime LastReportedUsage { get; private set; }
    public string MeasurementUnit { get; private set; }
    public bool OverageProtection { get; private set; }
    public decimal TotalUsage { get; private set; }
    public string UsageType { get; private set; }
    public int ResultCode { get; private set; }
    public string Error { get; private set; }

    public OrionGetUsageResponseData(string accountUid
      , decimal amount
      , string baseQuotaElementUid
      , decimal currentBaseQuotaAllowed
      , decimal currentExtendedQuotaAllowed
      , DateTime dateCreated
      , DateTime firstReportedUsage
      , DateTime lastReportedUsage
      , string measurementUnit
      , bool overageProtection
      , decimal totalUsage
      , string usageType
      , int resultCode
      , string error)
    {
      AccountUID = accountUid;
      Amount = amount;
      BaseQuotaElementUID = baseQuotaElementUid;
      CurrentBaseQuotaAllowed = currentBaseQuotaAllowed;
      CurrentExtendedQuotaAllowed = currentExtendedQuotaAllowed;
      DateCreated = dateCreated;
      FirstReportedUsage = firstReportedUsage;
      LastReportedUsage = lastReportedUsage;
      MeasurementUnit = measurementUnit;
      TotalUsage = totalUsage;
      UsageType = usageType;
      ResultCode = resultCode;
      Error = error;

      IsSuccess = true;
    }

    public OrionGetUsageResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
      IsSuccess = false;
    }

    public OrionGetUsageResponseData(RequestData requestData, Exception ex)
    {
      IsSuccess = false;
      _exception = new AtlantisException(requestData,
                                         "OrionGetUsageResponseData",
                                         ex.Message,
                                         requestData.ToXML());

    }

    #region IResponseData Members

    public string ToXML()
    {
      var xmlDoc = new XDocument(
        new XElement("GetUsageResponse",
                     new XElement("GetUsageResult", ResultCode),
                     new XElement("UsageReport",
                                  new XElement("Amount", Amount),
                                  new XElement("AccountUID", AccountUID),
                                  new XElement("UsageType", UsageType),
                                  new XElement("MeasurementUnit", MeasurementUnit),
                                  new XElement("BaseQuotaElementUID", BaseQuotaElementUID),
                                  new XElement("CurrentBaseQuotaAllowed", CurrentBaseQuotaAllowed),
                                  new XElement("CurrentExtendedQuotaAllowed", CurrentExtendedQuotaAllowed),
                                  new XElement("TotalUsage", TotalUsage),
                                  new XElement("FirstReportedUsage", FirstReportedUsage),
                                  new XElement("LastReportedUsage", LastReportedUsage),
                                  new XElement("DateCreated", DateCreated),
                                  new XElement("OverageProtection", OverageProtection)
                       ),
                     new XElement("Error", Error)
          )
        );

      return xmlDoc.ToString();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}
