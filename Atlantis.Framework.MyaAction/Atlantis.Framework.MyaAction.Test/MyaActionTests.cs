using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MyaAction.Interface;


namespace Atlantis.Framework.MyaAction.Test
{
  [TestClass]
  public class GetMyaActionTests
  {

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MyaActionTest()
    {
      int customerId = 1274; // need to get from EEMCreateNewAccountRequest or use debugger in MYA4

      int resourceId = 5172;
      string shopperId = "855307";
      int privateLableId = 1;

      var actionNodeTemplate = @"<ACTION name=""CampaignBlazerActivate"" id=""{0}"" shopper_id=""{1}"" privatelabelid=""{2}""/>";
      var actionNode = string.Format(actionNodeTemplate, resourceId, shopperId, privateLableId);

      var eemNodeTemplate = @"<CAMPAIGNBLAZER external_customer_id=""{0}"" />";
      var eemNode = string.Format(eemNodeTemplate, customerId);

      var notesNode = sCreateNoteXml("Blah", resourceId, "CampaignBlazerActivate", "UnitTest");

      var actionRootTemplate = @"<ACTIONROOT>{0}{1}{2}</ACTIONROOT>";

      var actionRootXml = string.Format(actionRootTemplate, actionNode, eemNode, notesNode);
      string actionArgs = String.Format("{0}|{1}|{2}|{3}|{4}", Guid.NewGuid(), "CampBlazer", resourceId, "CampaignBlazerActivate", DateTime.Now);

      var request = new MyaActionRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, actionRootXml,
                                             actionArgs);
      var response = (MyaActionResponseData)Engine.Engine.ProcessRequest(request, 472);

    }

    protected static string sCreateNoteXml(string sShopperNote, int sResourceID, string sActionNote, string sEnteredBy)
    {

      XDocument doc = new XDocument(
        new XElement("NOTES",
                     new XElement("SHOPPERNOTE",
                                  new XAttribute("note", string.Format("{0}:{1}-{2}", "CampBlazer", sResourceID, sShopperNote)),
                                  new XAttribute("enteredby", sEnteredBy)
                       ),
                     new XElement("ACTIONNOTE",
                                  new XAttribute("note",
                                                 "REQUESTEDBY: " + sEnteredBy + " " + sShopperNote),
                                  new XAttribute("modifiedby", "14")
                       )
          )
        );
      return doc.ToString();
    }




  }
}
