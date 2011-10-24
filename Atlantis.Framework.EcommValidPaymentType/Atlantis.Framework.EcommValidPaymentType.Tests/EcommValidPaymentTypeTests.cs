using System;
using Atlantis.Framework.EcommValidPaymentType.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommValidPaymentType.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class EcommValidPaymentTypeTests
  {
    [TestMethod]
    public void GetValidPaymentTypesForUSD()
    {
      EcommValidPaymentTypeRequestData requestData = new EcommValidPaymentTypeRequestData("847235",
                                                                                          "http://atlantis.framework.ecommvalidpaymenttype.tests/",
                                                                                          string.Empty,
                                                                                          Guid.NewGuid().ToString(),
                                                                                          0,
                                                                                          "gdshop",
                                                                                          "USD");

      try
      {
        EcommValidPaymentTypeResponseData responseData = (EcommValidPaymentTypeResponseData)Engine.Engine.ProcessRequest(requestData, 362);

        if(responseData.IsSuccess)
        {
          if(responseData.IsPaymentTypeAllowed(PaymentTypes.CreditCard, "Discover"))
          {
            Console.Write("Credit card payment is allowed for USD");
          }
          else
          {
            Console.Write("Credit card payment is NOT allowed for USD");
          }
        }
        else
        {
          Assert.Fail();
        }
      }
      catch(Exception ex)
      {
        Console.Write(ex.Message);   
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetValidPaymentTypesForGBP()
    {
      EcommValidPaymentTypeRequestData requestData = new EcommValidPaymentTypeRequestData("847235",
                                                                                          "http://atlantis.framework.ecommvalidpaymenttype.tests/",
                                                                                          string.Empty,
                                                                                          Guid.NewGuid().ToString(),
                                                                                          0,
                                                                                          "gdshop",
                                                                                          "GBP");

      try
      {
        EcommValidPaymentTypeResponseData responseData = (EcommValidPaymentTypeResponseData)Engine.Engine.ProcessRequest(requestData, 362);

        if (responseData.IsSuccess)
        {
          if (responseData.IsPaymentTypeAllowed(PaymentTypes.CreditCard, "Visa"))
          {
            Console.Write("Credit card payment is allowed for GBP");
          }
          else
          {
            Console.Write("Credit card payment is NOT allowed for GBP");
          }
        }
        else
        {
          Assert.Fail();
        }
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetValidPaymentTypesForEUR()
    {
      EcommValidPaymentTypeRequestData requestData = new EcommValidPaymentTypeRequestData("847235",
                                                                                          "http://atlantis.framework.ecommvalidpaymenttype.tests/",
                                                                                          string.Empty,
                                                                                          Guid.NewGuid().ToString(),
                                                                                          0,
                                                                                          "gdshop",
                                                                                          "EUR");

      try
      {
        EcommValidPaymentTypeResponseData responseData = (EcommValidPaymentTypeResponseData)Engine.Engine.ProcessRequest(requestData, 362);

        if (responseData.IsSuccess)
        {
          if (responseData.IsPaymentTypeAllowed(PaymentTypes.CreditCard, "AMEX"))
          {
            Console.Write("Credit card payment is allowed for EUR");
          }
          else
          {
            Console.Write("Credit card payment is NOT allowed for EUR");
          }
        }
        else
        {
          Assert.Fail();
        }
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
        Assert.Fail();
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetValidPaymentTypesForBadCurrency()
    {
      EcommValidPaymentTypeRequestData requestData = new EcommValidPaymentTypeRequestData("847235",
                                                                                          "http://atlantis.framework.ecommvalidpaymenttype.tests/",
                                                                                          string.Empty,
                                                                                          Guid.NewGuid().ToString(),
                                                                                          0,
                                                                                          "gdshop",
                                                                                          "AAA");

      try
      {
        EcommValidPaymentTypeResponseData responseData = (EcommValidPaymentTypeResponseData)Engine.Engine.ProcessRequest(requestData, 362);

        if (responseData.IsSuccess)
        {
          if (responseData.IsPaymentTypeAllowed(PaymentTypes.CreditCard, "JCB"))
          {
            Console.Write("Credit card payment is allowed for AAA");
          }
          else
          {
            Console.Write("Credit card payment is NOT allowed for AAA");
          }
        }
        else
        {
          Assert.Fail();
        }
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
        Assert.Fail();
      }
    }
  }
}
