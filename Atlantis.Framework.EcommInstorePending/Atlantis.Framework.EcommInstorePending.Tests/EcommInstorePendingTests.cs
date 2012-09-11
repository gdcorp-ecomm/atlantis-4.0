using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommInstorePending.Interface;
using System.Data.SqlClient;
using System.Data;

namespace Atlantis.Framework.EcommInstorePending.Tests
{
  [TestClass]
  public class EcommInstorePendingTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInstorePending.Impl.dll")]
    public void NoPendingCredit()
    {
      EcommInstorePendingRequestData request = new EcommInstorePendingRequestData("855503", string.Empty, string.Empty, string.Empty, 0, "USD");
      EcommInstorePendingResponseData response = (EcommInstorePendingResponseData)Engine.Engine.ProcessRequest(request, 596);
      Assert.AreEqual(0, response.Amount);
      Assert.AreEqual(InstorePendingResult.NoCreditsToConsume, response.Result);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommInstorePending.Impl.dll")]
    public void HasPendingCredit()
    {
      EnsurePendingCreditExists();
      EcommInstorePendingRequestData request = new EcommInstorePendingRequestData("832652", string.Empty, string.Empty, string.Empty, 0, "USD");
      EcommInstorePendingResponseData response = (EcommInstorePendingResponseData)Engine.Engine.ProcessRequest(request, 596);
      Assert.IsTrue(response.Amount > 0);
      Assert.IsFalse(string.IsNullOrEmpty(response.TransactionalCurrencyType));
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
