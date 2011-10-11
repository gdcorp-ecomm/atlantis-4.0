using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCModifyDNS.Interface;

namespace Atlantis.Framework.DCCModifyDNS.Tests
{
  [TestClass]
  public class DCCModifyDNSTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ModifyDNSValid()
    {
      DCCModifyDNSRequestData request = new DCCModifyDNSRequestData("9865", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");

      DnsRecordType record = new DnsRecordType();

      record.Type = "a";
      record.AttributeUid = "f7144121-0fbc-4b02-ba88-a6b319b448b2";
      record.Data = "172.19.67.186";
      record.Name = "test2";
      record.TTL = 3600;
      request.addRecord(record);

      DCCModifyDNSResponseData response = (DCCModifyDNSResponseData)Engine.Engine.ProcessRequest(request, 109);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ModifyDNSForDomainThatShopperDoesNotOwn()
    {
      DCCModifyDNSRequestData request = new DCCModifyDNSRequestData("847235", string.Empty, string.Empty, string.Empty, 0, 1, false, "000123123ASD.BIZ");

      DnsRecordType record = new DnsRecordType();

      record.Type = "a";
      record.AttributeUid = "f7144121-0fbc-4b02-ba88-a6b319b448b2";
      record.Data = "172.19.67.186";
      record.Name = "test2";
      record.TTL = 3600;
      request.addRecord(record);

      try
      {
        DCCModifyDNSResponseData response = (DCCModifyDNSResponseData)Engine.Engine.ProcessRequest(request, 109);

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
