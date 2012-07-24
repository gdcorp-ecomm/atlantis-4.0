using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.FastballCICreate.Interface;
using Fastball.Common.DataTransfer.Dtos;
using Fastball.Common.DataTransfer.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FastballCICreate.Tests
{
  [TestClass]
  public class FastballCICreateTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void Can_Create_CI_Codes()
    {

      var items = new CreateCICodesForItems()
      {
        CICodeItems = new List<CICodeItem>
        {
          new CICodeItem()
          {
            CIName = "232342323",
            Description = "Landing Page",
            Page = "Landing Page",
            Type = "link",
            Company = "GoDaddy",
            Product = "Domain",
            Destination = "Landing Page",
            Configuration = "Default"
          },
          new CICodeItem()
          {
            CIName = "565756567",
            Description = "Landing Page",
            Page = "Landing Page",
            Type = "link",
            Company = "GoDaddy",
            Product = "Domain",
            Destination = "Landing Page",
            Configuration = "Default"
          }
        }
      };

      string inputXml = XMLHelper.ToXml(items, true);

      System.Diagnostics.Debug.WriteLine(inputXml);

      //Arange
      string shopperId = "860316";
      int requestType = 568;
      FastballCICreateRequestData request = new FastballCICreateRequestData(shopperId,
        "http://www.yuck.com", string.Empty, string.Empty, 1, inputXml);

      //Act
      FastballCICreateResponseData response = (FastballCICreateResponseData)Engine.Engine.ProcessRequest(request, requestType);

      //Assert
      Assert.IsTrue(response.IsSuccess);

      System.Diagnostics.Debug.WriteLine(response);

      var transferResultDto = XMLHelper.FromXml<TransferResult>(response.CICodeXMLOutput, typeof(TransferResult));

      if (transferResultDto.Successful)
      {
        System.Diagnostics.Debug.WriteLine(transferResultDto.Message);
        System.Diagnostics.Debug.WriteLine("Count: {0}", transferResultDto.ResultItems.Count());

        foreach (var item in transferResultDto.ResultItems)
        {
          System.Diagnostics.Debug.WriteLine("{0} - {1}", item.CIName, item.CICID);
        }
      }

      //Assert.IsNotNull(response.CICodeXMLOutput, "The service returned XML content.");

      //Assert.IsTrue(response.CICodeXMLOutput.IndexOf("<Successful>true</Successful>", StringComparison.OrdinalIgnoreCase) >= 0);

      Assert.IsTrue(transferResultDto.Successful);

      Assert.IsTrue(transferResultDto.ResultItems.Count > 0, "At least 1 or more CI Codes was generated and returned.");

    }
  }
}