using System;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCDeleteDNS.Interface;

namespace Atlantis.Framework.DCCDeleteDNS.Tests
{
  [TestClass]
  public class DCCDeleteDNSTests
  {
    private void WriteErrors(DCCDeleteDNSResponseData response)
    {
      foreach (string error in response.ErrorList)
      {
        Console.WriteLine(error);
      }  
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCDeleteDNSValid()
    {
      DCCDeleteDNSRequestData request = new DCCDeleteDNSRequestData("9865", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");

      DnsRecordType record = new DnsRecordType();

      record.Type = "a";
      record.AttributeUid = "37d113bf-c085-44fd-bcd2-5e95d6fc3f39";
      request.addRecord(record);

      DCCDeleteDNSResponseData response = (DCCDeleteDNSResponseData)Engine.Engine.ProcessRequest(request, 108);
      WriteErrors(response);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCDeleteDNSForDomainShopperDoesNotOwn()
    {
      DCCDeleteDNSRequestData request = new DCCDeleteDNSRequestData("847235", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");

      DnsRecordType record = new DnsRecordType();

      record.Type = "a";
      record.AttributeUid = "37d113bf-c085-44fd-bcd2-5e95d6fc3f39";
      request.addRecord(record);

      try
      {
        DCCDeleteDNSResponseData response = (DCCDeleteDNSResponseData)Engine.Engine.ProcessRequest(request, 108);
        WriteErrors(response);
        if (response.GetException() != null)
        {
          throw response.GetException();
        }
      }
      catch (AtlantisException ex)
      {
        Assert.IsTrue(ex.Message.StartsWith("Could not find DNS zonefile for"));
      }
    }
  }
}
