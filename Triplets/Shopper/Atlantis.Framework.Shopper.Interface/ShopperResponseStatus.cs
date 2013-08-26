﻿using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class ShopperResponseStatus
  {
    public static ShopperResponseStatus Success { get; private set; }

    static ShopperResponseStatus()
    {
      Success = new ShopperResponseStatus();
    }

    public ShopperResponseStatusType Status { get; private set; }
    public string ErrorCode { get; private set; }
    public string ErrorMessage { get; private set; }

    private ShopperResponseStatus()
    {
      Status = ShopperResponseStatusType.Success;
      ErrorCode = string.Empty;
      ErrorMessage = string.Empty;
    }

    public static ShopperResponseStatus FromResponseElement(XElement responseElement)
    {
      if ("Status" != responseElement.Name)
      {
        return Success;
      }

      ShopperResponseStatus result = new ShopperResponseStatus();
      result.Status = ShopperResponseStatusType.UnknownError;

      var errorElement = responseElement.Element("Error");
      result.ErrorCode = (errorElement != null) ? errorElement.Value : "Unknown";

      var errorDescription = responseElement.Element("Description");
      result.ErrorMessage = (errorDescription != null) ? errorDescription.Value : string.Empty;

      switch (result.ErrorCode)
      {
        case "0xC0044A15":
          result.Status = ShopperResponseStatusType.ShopperNotFound;
          break;
        case "0xC0044A10":
          result.Status = ShopperResponseStatusType.InvalidRequestField;
          break;
      }

      return result;
    }
  }
}