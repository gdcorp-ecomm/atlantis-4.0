using Atlantis.Framework.Interface;
using System.Collections.Specialized;

namespace Atlantis.Framework.PurchaseEmail.Interface
{
  internal static class ErrorHelper
  {
    internal static void Log(string message, string data, string sourceFunction, string orderId, ISiteContext siteContext, IShopperContext shopperContext)
    {

      if (string.IsNullOrEmpty(message))
        return;

      if (data == null)
        return;

      if (string.IsNullOrEmpty(sourceFunction))
        return;

      if (orderId == null)
        return;

      string shopperId = "null";
      if (shopperContext != null)
        shopperId = shopperContext.ShopperId;

      string pathway = string.Empty;
      int pageCount = 0;
      if (siteContext != null)
      {
        pathway = siteContext.Pathway;
        pageCount = siteContext.PageCount;
      }

      //split up the message in 3000 character chunks due to db restriction
      StringCollection messages =
            new StringCollection();
      for (int i = 0; i < 10; i++)
      {
        if (message.Length > 3000)
        {
          messages.Add(message.Substring(0, 3000));
          message = message.Substring(3000);
        }
        else
        {
          messages.Add(message);
          break;
        }
      }

      foreach (string max3000CharMessage in messages)
      {
        if (siteContext != null && shopperContext != null)
        {
          AtlantisException aex = new AtlantisException(
            sourceFunction, string.Empty,
            "123", max3000CharMessage, data, shopperContext.ShopperId, orderId,
            string.Empty, siteContext.Pathway, siteContext.PageCount);
          Engine.Engine.LogAtlantisException(aex);
        }
        else
        {
          AtlantisException aex = new AtlantisException(
            sourceFunction, string.Empty,
            "123", max3000CharMessage, data, string.Empty, orderId,
            string.Empty, string.Empty, 0);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
    }

    internal static void LogSilent(string message, string data, string sourceFunction, string orderId, ISiteContext siteContext, IShopperContext shopperContext)
    {
      try
      {
        System.Diagnostics.Debug.WriteLine(message + ":" + data + ":" + sourceFunction);
        Log(message, data, sourceFunction, orderId, siteContext, shopperContext);
      }
      catch (System.Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }
    }

  }
}
