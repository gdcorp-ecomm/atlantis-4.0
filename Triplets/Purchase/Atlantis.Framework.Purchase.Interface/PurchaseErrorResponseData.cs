using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Purchase.Interface
{
  public class PurchaseErrorResponseData : IResponseData
  {
    private const string DEFAULT_TERMINAL_ERROR_MESSAGE = "There was a problem processing your transaction. Please verify your payment information or use an alternate form of payment.";
    public string CustomerFriendlyErrorText { get; private set; }
    public bool Success { get; private set; }

    private AtlantisException _exception = null;

    public static PurchaseErrorResponseData FromXML(string data, PurchaseErrorRequestData requestedData)
    {
      return new PurchaseErrorResponseData(data,requestedData);
    }

    public static PurchaseErrorResponseData FromException(AtlantisException exception, PurchaseErrorRequestData requestedData)
    {
      return new PurchaseErrorResponseData(exception,requestedData);
    }


    private PurchaseErrorResponseData(string xmlResults, PurchaseErrorRequestData requestedData)
    {
      try
      {
        CustomerFriendlyErrorText = string.Empty;
        XElement data = XElement.Parse(xmlResults);
        if (data.HasAttributes && data.Attribute("errorText") != null)
        {
          Success = true;
          CustomerFriendlyErrorText = data.Attribute("errorText").Value;
        }
        else
        {
          Success = false;
          CustomerFriendlyErrorText = DEFAULT_TERMINAL_ERROR_MESSAGE;
          _exception = new AtlantisException((RequestData)requestedData, "PurchaseErrors Retrieval", string.Empty, string.Empty, new System.Exception("No ErrorText found"));
        }
      }
      catch (Exception ex)
      {
        Success = false;
        CustomerFriendlyErrorText = DEFAULT_TERMINAL_ERROR_MESSAGE;
        _exception = new AtlantisException((RequestData)requestedData, "PurchaseErrors Retrieval", string.Empty, string.Empty, ex);
      }     
    }

    private PurchaseErrorResponseData(Exception ex, PurchaseErrorRequestData requestedData)
    {
      CustomerFriendlyErrorText = DEFAULT_TERMINAL_ERROR_MESSAGE;
      _exception = new AtlantisException((RequestData)requestedData, "PurchaseErrors Retrieval", string.Empty, string.Empty, ex);
    }

    public string ToXML()
    {
      XElement element = new XElement("PurchaseErrorResponseData");
      element.Add(
        new XAttribute("CustomerFriendlyErrorText", CustomerFriendlyErrorText));
      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
