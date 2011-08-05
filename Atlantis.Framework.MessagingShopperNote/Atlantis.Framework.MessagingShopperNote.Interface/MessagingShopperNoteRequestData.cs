using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MessagingShopperNote.Interface
{
  public class MessagingShopperNoteRequestData : RequestData
  {
    #region Properties

    //Constructor Properties
    public string Note { get; set; }
    public string EnteredBy { get; set; }
    public int NoteType { get; set; }
    public int PrivateLabelId {get; set; }

    //Optional Customer Note / Common Properties
    public string RequestingIp { get; set; }
    public string AccessRoleId { get; set; }
    public string ManagerUserId { get; set; }
    public string NoteTypeLookupId { get; set; }
    public string SessionId { get; set; }
    public string ShortDescription { get; set; }
    public string TaskActionTypeId { get; set; }

    //Optional Access Note Properties
    public string AccessReasonId { get; set; }
    public string MessageId { get; set; }
    public string TemplateId { get; set; }
    public string ValidationTypeId { get; set; }
    public string MachineName { get; set; }

    private DateTime TimeStamp { get; set; }
    #endregion

    public MessagingShopperNoteRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string note, 
                                  string enteredBy,
                                  int noteType,
                                  int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      TimeStamp = DateTime.Now;
      Note = note;
      EnteredBy = enteredBy;
      NoteType = noteType;
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in MessagingShopperNoteRequestData");
    }

    #region ToXML
    public override string ToXML()
    {
      XDocument msgDoc = new XDocument();
      XElement msgRoot = new XElement("messageXml");
      msgDoc.Add(msgRoot);

      switch (NoteType)
      {
        case NoteTypes.SHOPPER_NOTE:
          CreateShopperNoteXml(ref msgRoot);
          break;
        case NoteTypes.ACCESS_NOTE:
          CreateAccessNoteXml(ref msgRoot);
          break;
        case NoteTypes.CUSTOMER_NOTE:
          CreateCustomerNoteXml(ref msgRoot);
          break;
      }
      return msgDoc.ToString();
    }

    #region ToXML Helpers
    private void CreateShopperNoteXml(ref XElement msgRoot)
    {
      Dictionary<string, string> dictionaryItems = SetDictionaryItems();
      msgRoot.Add(new XAttribute("namespace", "Notes"));
      msgRoot.Add(new XAttribute("type", NoteTypes.GetNoteTypeString(NoteType)));

      XElement dictionaryElement = new XElement("dictionary");
      foreach (KeyValuePair<string, string> kvp in dictionaryItems)
      {
        if (!string.IsNullOrEmpty(kvp.Value))
        {
          XElement item = new XElement("item");
          item.Add(new XAttribute("name", kvp.Key));
          item.SetValue(kvp.Value);
          dictionaryElement.Add(item);
        }
      }

      msgRoot.Add(dictionaryElement);
    }

    private void CreateCustomerNoteXml(ref XElement msgRoot)
    {
      CreateShopperNoteXml(ref msgRoot);
      msgRoot.Add(new XElement("contactpoints"));
      msgRoot.Add(new XElement("resources",
        new XElement("resource",
          new XAttribute("type", "0"),
          new XAttribute("id", "0"))));
    }

    private void CreateAccessNoteXml(ref XElement msgRoot)
    {
      CreateCustomerNoteXml(ref msgRoot);
      Dictionary<string, string> metricAttributes = SetMetricAttributes();
      Dictionary<string, string> routerslipAttributes = SetRouterslipAttributes();

      XElement metricsElement = new XElement("metrics");
      foreach (KeyValuePair<string,string> kvp in metricAttributes)
      {
        if (!string.IsNullOrEmpty(kvp.Value))
        {
          XElement item = new XElement("attribute");
          item.Add(new XAttribute("name", kvp.Key));
          item.SetValue(kvp.Value);
          metricsElement.Add(item);
        }
      }
      XElement routerslipElement = new XElement("routerslip");
      foreach (KeyValuePair<string,string> kvp in routerslipAttributes)
      {
        if (!string.IsNullOrEmpty(kvp.Value))
        {
          XElement item = new XElement("attribute");
          item.Add(new XAttribute("name", kvp.Key));
          item.SetValue(kvp.Value);
          routerslipElement.Add(item);
        }
      }
      msgRoot.Add(metricsElement);
      msgRoot.Add(routerslipElement);
    }

    private Dictionary<string,string> SetDictionaryItems()
    {
      Dictionary<string,string> dictionaryItems = new Dictionary<string,string>();
      string cleanNote = PerformLuhnCheck(Note);

      switch (NoteType)
      {
        case NoteTypes.ACCESS_NOTE:
          dictionaryItems.Add("AccessReasonID", AccessReasonId);
          dictionaryItems.Add("ManagerUserID", ManagerUserId);
          dictionaryItems.Add("MessageID", MessageId);
          dictionaryItems.Add("NoteTypeLookupID", NoteTypeLookupId);
          dictionaryItems.Add("PrivateLabelID", PrivateLabelId.ToString());
          dictionaryItems.Add("SessionID", SessionId);
          dictionaryItems.Add("ShopperID", ShopperID);
          dictionaryItems.Add("TemplateID", TemplateId);
          dictionaryItems.Add("TimeStamp", TimeStamp.ToString());
          dictionaryItems.Add("ValidationTypeID", ValidationTypeId);
          break;
        case NoteTypes.CUSTOMER_NOTE:
          dictionaryItems.Add("PrivateLabelID", PrivateLabelId.ToString());
          dictionaryItems.Add("AccessRoleID", AccessRoleId);
          dictionaryItems.Add("CustomerNote", cleanNote);
          dictionaryItems.Add("ManagerUserID", ManagerUserId);
          dictionaryItems.Add("NoteTypeLookupID", NoteTypeLookupId);
          dictionaryItems.Add("RequestingIP", RequestingIp);
          dictionaryItems.Add("SessionID", SessionId);
          dictionaryItems.Add("ShopperID", ShopperID);
          dictionaryItems.Add("ShortDescription", ShortDescription);
          dictionaryItems.Add("TaskActionTypeID", TaskActionTypeId);
          dictionaryItems.Add("TimeStamp", TimeStamp.ToString());
          break;
        case NoteTypes.SHOPPER_NOTE:
          dictionaryItems.Add("PrivateLabelID", PrivateLabelId.ToString());
          dictionaryItems.Add("ShopperID", ShopperID);
          dictionaryItems.Add("EnteredBy", EnteredBy);
          dictionaryItems.Add("RequestingIP", RequestingIp);
          dictionaryItems.Add("ShopperNote", Note);
          dictionaryItems.Add("EnteredDate", TimeStamp.ToString());
          break;
      }
      
      return dictionaryItems;
    }
    
    private Dictionary<string,string> SetMetricAttributes()
    {
      Dictionary<string,string> metricAttributes = new Dictionary<string,string>();

      metricAttributes.Add("Entered System At", TimeStamp.ToString());
      metricAttributes.Add("entry_time", TimeStamp.ToString());
      
      return metricAttributes;
    }

    private Dictionary<string,string> SetRouterslipAttributes()
    {
      Dictionary<string,string> metricAttributes = new Dictionary<string,string>();

      metricAttributes.Add("ClientIP", RequestingIp);
      metricAttributes.Add("Machine", MachineName);
      
      return metricAttributes;
    }
    #endregion
    #endregion

    #region LuhnCheck

    public static string PerformLuhnCheck(string note)
    {
      Regex ccEx = new Regex(@"((4\d{3})|(5[1-5]\d{2})|(6011))-?\d{4}-?\d{4}-?\d{4}|3[4,7][\d\s-]{15}");
      string result = ccEx.Replace(note, "****");
      return result;
    }

    #endregion
  }
}
