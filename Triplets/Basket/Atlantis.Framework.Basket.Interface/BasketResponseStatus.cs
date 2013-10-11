using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;

namespace Atlantis.Framework.Basket.Interface
{
  public class BasketResponseStatus
  {
    // <RESPONSE><ERRORS><ERROR><NUMBER>-1073468912</NUMBER><DESC> The specified shopper is not valid.</DESC><SOURCE>WscgdBasketService::CWscgdBasketService::AddItem</SOURCE><LEVEL>ERROR</LEVEL></ERROR></ERRORS></RESPONSE>
    public static BasketResponseStatus Success { get; private set; }
    public static BasketResponseStatus NullResponseError { get; private set; }
    public static BasketResponseStatus UnknownError { get; private set; }

    static BasketResponseStatus()
    {
      Success = new BasketResponseStatus();
      
      NullResponseError = new BasketResponseStatus();
      NullResponseError.Status = BasketResponseStatusType.Errors;
      NullResponseError.AddError(new BasketError() { Description = "Null Response Element.", Level = "ERROR", Number = "0", Source = "BasketResponseStatus.FromResponseElement" });

      UnknownError = new BasketResponseStatus();
      UnknownError.Status = BasketResponseStatusType.Errors;
      UnknownError.AddError(new BasketError() { Description = "No Errors, but no success message", Level = "ERROR", Number = "0", Source = "BasketResponseStatus.FromResponseElement" });
    }

    private readonly List<BasketError> _errors;

    public BasketResponseStatusType Status { get; private set; }
    public IEnumerable<BasketError> Errors 
    {
      get { return _errors; }
    }

    public bool HasErrors
    {
      get { return _errors.Count > 0; }
    }
 
    private BasketResponseStatus()
    {
      _errors = new List<BasketError>();
      Status = BasketResponseStatusType.Success;
    }

    private void AddError(BasketError error)
    {
      _errors.Add(error);
    }

    public static BasketResponseStatus FromResponseElement(XElement responseElement)
    {
      if (responseElement == null)
      {
        return NullResponseError;
      }

      var messageElement = responseElement.Element("MESSAGE");
      if ((messageElement != null) && ("Success".Equals(messageElement.Value, StringComparison.OrdinalIgnoreCase)))
      {
        return Success;
      }

      var errorElements = responseElement.Descendants("ERROR");
      if (errorElements.Count() == 0)
      {
        return UnknownError;
      }

      var result = new BasketResponseStatus();
      result.Status = BasketResponseStatusType.Errors;

      foreach(var errorElement in errorElements)
      {
        result.AddError(BasketError.FromErrorXml(errorElement));
      }

      return result;
    }
  }
}
