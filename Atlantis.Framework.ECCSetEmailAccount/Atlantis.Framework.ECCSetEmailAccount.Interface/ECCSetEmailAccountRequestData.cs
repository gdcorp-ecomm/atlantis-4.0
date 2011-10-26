using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetEmailAccount.Interface
{
  public class EccSetEmailAccountRequestData : RequestData
  {
    public EccSetEmailAccountRequestData(string shopperId,
        string sourceUrl,
        string orderId,
        string pathway,
        int pageCount,
        int resellerId,
        TimeSpan requestTimeout,
        string accountUid,
        string emailAddress,
        string password,
        string ccList,
        string subaccount,
        float diskspace,
        bool catchAll,
        int smtpRelays,
        bool spamFilter,
        bool sendSingleResponse,
        string autoResponderMessage,
        string autoResponderSubject,
        string autoResponderFrom,
        DateTime? autoResponsderStart,
        DateTime? autoResponsderEnd,
        string autoResponderStatus
      )
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResellerId = resellerId;
      RequestTimeout = requestTimeout;
      AccountUid = accountUid;
      EmailAddress = emailAddress;
      Password = password;
      CCList = ccList;
      Subaccount = subaccount;
      DiskSpace = diskspace;
      IsCatchAll = catchAll;
      SmtpRelays = smtpRelays;
      HasSpamFilter = spamFilter;
      SendSingleResponse = sendSingleResponse;
      AutoResponderMessage = autoResponderMessage;
      AutoResponderSubject = autoResponderSubject;
      AutoResponderFrom = autoResponderFrom;
      AutoResponsderStart = autoResponsderStart;
      AutoResponsderEnd = autoResponsderEnd;
      AutoResponderStatus = autoResponderStatus;
    }

    public EccSetEmailAccountRequestData(string shopperId,
        string sourceUrl,
        string orderId,
        string pathway,
        int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _requestTimeout;
    }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);
    
    public int ResellerId { get; set; }
    public string AccountUid { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string CCList { get; set; }
    public string Subaccount { get; set; }
    public float DiskSpace { get; set; }
    public bool IsCatchAll { get; set; }
    public int SmtpRelays { get; set; }
    public bool HasSpamFilter { get; set; }
    public bool SendSingleResponse { get; set; }
    public string AutoResponderMessage { get; set; }
    public string AutoResponderSubject { get; set; }
    public string AutoResponderFrom { get; set; }
    public DateTime? AutoResponsderStart { get; set; }
    public DateTime? AutoResponsderEnd { get; set; }
    public string AutoResponderStatus { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      StringBuilder result = new StringBuilder();


      using (XmlTextWriter xmlWriter = new XmlTextWriter(new StringWriter(result)))
      {

        xmlWriter.WriteStartElement("requestInfo");
        xmlWriter.WriteStartElement("dictionary");

        xmlWriter.WriteStartElement("ShopperId");
        xmlWriter.WriteValue(base.ShopperID);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("SourceUrl");
        xmlWriter.WriteValue(base.SourceURL);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("OrderId");
        xmlWriter.WriteValue(base.OrderID);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("Pathway");
        xmlWriter.WriteValue(base.Pathway);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("PageCount");
        xmlWriter.WriteValue(base.PageCount);
        xmlWriter.WriteEndElement();


        xmlWriter.WriteStartElement("ResellerId");
        xmlWriter.WriteValue(this.ResellerId);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("RequestTimeout");
        xmlWriter.WriteValue(this.RequestTimeout);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }

  }
}
