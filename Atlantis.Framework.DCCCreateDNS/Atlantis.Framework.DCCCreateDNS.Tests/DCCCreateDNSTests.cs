using System;
using Atlantis.Framework.DCCCreateDNS.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCCreateDNS.Tests
{
  [TestClass]
  public class DCCCreateDNSTests
  {
    private void WriteErrors(DCCCreateDNSResponseData responseData)
    {
      foreach (string error in responseData.ErrorList)
      {
        Console.WriteLine(error);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCCreateDNSValid()
    {
      DCCCreateDNSRequestData request = new DCCCreateDNSRequestData("9865", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");

      DnsRecordType record = new DnsRecordType();
      
      record.Type = "a";
      record.Data = "172.19.67.185";
      record.Name = "test";
      record.TTL = 3600;
      request.addRecord(record);

      DCCCreateDNSResponseData response = (DCCCreateDNSResponseData)Engine.Engine.ProcessRequest(request, 107);

      WriteErrors(response);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCCreateDNSForDomainThatShopperDoesNotOwn()
    {
      DCCCreateDNSRequestData request = new DCCCreateDNSRequestData("847235", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");

      DnsRecordType record = new DnsRecordType();

      record.Type = "a";
      record.Data = "172.19.67.185";
      record.Name = "invalid";
      record.TTL = 3600;
      request.addRecord(record);

      try
      {
        DCCCreateDNSResponseData response = (DCCCreateDNSResponseData)Engine.Engine.ProcessRequest(request, 107);
        WriteErrors(response);

        if(response.GetException() != null)
        {
          throw response.GetException();
        }
      }
      catch (AtlantisException ex)
      {
        Assert.IsTrue(ex.Message.StartsWith("Could not find DNS zonefile for"));
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCCreateCNameValid()
    {
      DCCCreateDNSRequestData request = new DCCCreateDNSRequestData("9865", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");

      DnsRecordType record = new DnsRecordType();

      record.Type = "cname";
      record.Data = "test.godaddymobile.com";
      record.Name = "newcname";
      record.TTL = 3600;
      request.addRecord(record);

      DCCCreateDNSResponseData response = (DCCCreateDNSResponseData)Engine.Engine.ProcessRequest(request, 107);
      WriteErrors(response);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCCreateMXRecordValid()
    {
      DCCCreateDNSRequestData request = new DCCCreateDNSRequestData("9865", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");

      DnsRecordType record = new DnsRecordType();

      record.Type = "mx";
      record.Data = "test.godaddymobile.com";
      record.Name = "blah.com";
      record.TTL = 3600;
      record.Priority = 3600;
      request.addRecord(record);

      DCCCreateDNSResponseData response = (DCCCreateDNSResponseData)Engine.Engine.ProcessRequest(request, 107);
      WriteErrors(response);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
