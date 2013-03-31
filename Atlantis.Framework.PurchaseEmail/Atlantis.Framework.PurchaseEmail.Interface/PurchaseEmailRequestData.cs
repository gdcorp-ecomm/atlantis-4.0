﻿using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MessagingProcess.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.PurchaseEmail.Interface.Emails;

namespace Atlantis.Framework.PurchaseEmail.Interface
{
  public class PurchaseEmailRequestData : RequestData
  {
    private static HashSet<string> _validOptions;
    static PurchaseEmailRequestData()
    {
      _validOptions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
      _validOptions.Add("IsNewShopper");
      _validOptions.Add("IsFraudRefund");
      _validOptions.Add("IsDomainMeConfirmation");
      _validOptions.Add("IsAZHumane");
      _validOptions.Add("IsDevServer");
    }

    private string _orderXml;
    private string _localizationCode = "EN";

    private Dictionary<string, string> _options = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

    public PurchaseEmailRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string orderXml)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _orderXml = orderXml;
    }

    public PurchaseEmailRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string orderXml,string localizationCode)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _orderXml = orderXml;
      _localizationCode = localizationCode;
    }

    public PurchaseEmailRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string orderXml, IDictionary<string, string> options, string localizationCode)
      : this(shopperId, sourceUrl, orderId, pathway, pageCount, orderXml,localizationCode)
    {
      if (options != null)
      {
        foreach (string key in options.Keys)
        {
          AddOption(key, options[key]);
        }
      }
    }

    public void AddOption(string name, string value)
    {
      if (_validOptions.Contains(name))
      {
        _options[name] = value;
      }
      else
      {
        throw new ArgumentException(name + " is not a valid option.");
      }
    }

    private bool GetOption(string name, bool defaultValue)
    {
      bool result = defaultValue;
      string value;
      if (_options.TryGetValue(name, out value))
      {
        result = ((value == "1") || (value.ToLowerInvariant() == "true"));
      }
      return result;
    }

    private bool IsNewShopper
    {
      get 
      {
        return GetOption("IsNewShopper", false);
      }
    }

    private bool IsFraudRefund
    {
      get
      {
        return GetOption("IsFraudRefund", false);
      }
    }

    private bool IsDomainMeConfirmation
    {
      get
      {
        return GetOption("IsDomainMeConfirmation", false);
      }
    }

    private bool IsAZHumane
    {
      get
      {
        return GetOption("IsAZHumane", false);
      }
    }

    private bool IsDevServer
    {
      get
      {
        return GetOption("IsDevServer", false);
      }
    }

    private void ProcessConfirmationType(PurchaseConfirmationEmailBase confirmationEmail,Dictionary<string, MessagingProcessRequestData> uniqueRequests)
    {
      List<MessagingProcessRequestData> requestSet=confirmationEmail.GetMessageRequests();
      if (requestSet != null)
      {
        foreach (MessagingProcessRequestData currentRequest in requestSet)
        {
          if (!uniqueRequests.ContainsKey(currentRequest.TemplateType))
          {
            uniqueRequests.Add(currentRequest.TemplateType, currentRequest);
          }
        }
      }
    }   

    public List<MessagingProcessRequestData> AllPossibleMessageTypes()
    {
      List<MessagingProcessRequestData> result = new List<MessagingProcessRequestData>();
      ObjectProviderContainer _objectContainer = new ObjectProviderContainer();
      OrderData orderData = new OrderData(_orderXml, IsNewShopper, IsFraudRefund, _objectContainer, _localizationCode);
      EmailRequired emailRequired = new EmailRequired(orderData);

      Dictionary<string, MessagingProcessRequestData> uniqueRequests = new Dictionary<string, MessagingProcessRequestData>();
      PurchaseConfirmationEmailBase confirmationEmail = new DBPAdminFeesConfirmationEmail(orderData, emailRequired, _objectContainer);
      ProcessConfirmationType(confirmationEmail, uniqueRequests);

      confirmationEmail = new AdminFeeConfirmationEmail(orderData, emailRequired, _objectContainer);
      ProcessConfirmationType(confirmationEmail, uniqueRequests);

      confirmationEmail = new GDConfirmationEmail(orderData, emailRequired, IsAZHumane,IsDevServer,this,_objectContainer);
      ProcessConfirmationType(confirmationEmail, uniqueRequests);

      confirmationEmail = new BRConfirmationEmail(orderData, emailRequired, IsDevServer, _objectContainer);
      ProcessConfirmationType(confirmationEmail, uniqueRequests);

      confirmationEmail = new PLConfirmationEmail(orderData, emailRequired, _objectContainer);
      ProcessConfirmationType(confirmationEmail, uniqueRequests);

      foreach (KeyValuePair<string, MessagingProcessRequestData> currentItem in uniqueRequests)
      {
        result.Add(currentItem.Value);
      }
      return result;
    }

    public List<MessagingProcessRequestData> GetPurchaseConfirmationEmailRequests()
    {
      AtlantisException exception;
      List<MessagingProcessRequestData> result = GetPurchaseConfirmationEmailRequests(out exception);
      return result;
    }

    public List<MessagingProcessRequestData> GetPurchaseConfirmationEmailRequests(out AtlantisException exception)
    {
      exception = null;
      List<MessagingProcessRequestData> result = null;
      ObjectProviderContainer _objectContainer = new ObjectProviderContainer();

      OrderData orderData = new OrderData(_orderXml, IsNewShopper, IsFraudRefund,_objectContainer,_localizationCode);

      try
      {
        XmlNodeList itemNodes = orderData.OrderXmlDoc.SelectNodes("/ORDER/ITEMS/ITEM");
        if (itemNodes.Count > 0)
        {
          EmailRequired emailRequired = new EmailRequired(orderData);
          PurchaseConfirmationEmailBase confirmationEmail = null;

          if (emailRequired.AdminFee && !emailRequired.ProcessFee)
          {
            if (emailRequired.DBPAdminFee)
            {
              confirmationEmail = new DBPAdminFeesConfirmationEmail(orderData, emailRequired, _objectContainer);
            }
            else
            {
              confirmationEmail = new AdminFeeConfirmationEmail(orderData, emailRequired, _objectContainer);
            }
          }
          else
          {
            if (IsDomainMeConfirmation)
            {
              confirmationEmail = new MEConfirmationEmail(orderData, emailRequired, _objectContainer);
            }
            else
            {
              if (orderData.PrivateLabelId == 1)
              {
                if (orderData.Detail.GetAttribute("basket_type") == "marketplace")
                {
                  confirmationEmail = new GDShopsConfirmationEmail(orderData, emailRequired,_objectContainer);
                }
                else
                {
                  confirmationEmail = new GDConfirmationEmail(orderData, emailRequired, IsAZHumane, IsDevServer, this, _objectContainer);
                }
              }
              else
              {
                if (orderData.PrivateLabelId == 2)
                {
                  confirmationEmail = new BRConfirmationEmail(orderData, emailRequired, IsDevServer, _objectContainer);
                }
                else
                {
                  confirmationEmail = new PLConfirmationEmail(orderData, emailRequired, _objectContainer);
                }
              }
            }
          }

          if (confirmationEmail != null)
          {
            result = confirmationEmail.GetMessageRequests();            
          }
        }
      }
      catch (Exception ex)
      {
        string shopperId = string.Empty;
        string orderId = string.Empty;

        if (orderData != null)
        {
          shopperId = orderData.ShopperId;
          orderId = orderData.OrderId;
        }

        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException("GetPurchaseConfirmationEmailRequests", base.SourceURL, "0", message, string.Empty,
          shopperId, orderId, string.Empty, base.Pathway, base.PageCount);
        exception = aex;
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;

    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("PurchaseEmail is not a cacheable request.");
    }
  }
}
