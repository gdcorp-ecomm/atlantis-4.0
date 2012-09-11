using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInstoreAccept.Interface;
using System.Data.SqlClient;
using System.Data;

namespace Atlantis.Framework.EcommInstoreAccept.Tests
{
  [TestClass]
  public class EcommInstoreAcceptTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInstoreAccept.Impl.dll")]
    public void NoPendingCreditToAccept()
    {
      EcommInstoreAcceptRequestData request = new EcommInstoreAcceptRequestData("855503", string.Empty, string.Empty, string.Empty, 0);
      EcommInstoreAcceptResponseData response = (EcommInstoreAcceptResponseData)Engine.Engine.ProcessRequest(request, 597);
      Assert.AreEqual(InstoreAcceptResult.NoCreditsToConsume, response.Result);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInstoreAccept.Impl.dll")]
    public void PendingCreditAccepted()
    {
      EnsurePendingCreditExists();
      EcommInstoreAcceptRequestData request = new EcommInstoreAcceptRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      EcommInstoreAcceptResponseData response = (EcommInstoreAcceptResponseData)Engine.Engine.ProcessRequest(request, 597);
      Assert.AreEqual(InstoreAcceptResult.Success, response.Result);
    }

    private const string _DEVSQL = "Server=g1dwpsql001;Database=godaddy;Trusted_Connection=Yes;";
    private const string _ADDCREDIT = "update comp_hostingInStoreCredit set isClaimed = 0 where shopper_id = '832652' and pkid = 708";

    private void EnsurePendingCreditExists()
    {
      // Requires that you have access to dev database for this to work
      using (SqlConnection connect = new SqlConnection(_DEVSQL))
      {
        using (SqlCommand command = new SqlCommand(_ADDCREDIT, connect))
        {
          command.CommandType = CommandType.Text;
          command.CommandTimeout = 1;
          connect.Open();
          command.ExecuteNonQuery();
        }
      }
    }

  }
}
