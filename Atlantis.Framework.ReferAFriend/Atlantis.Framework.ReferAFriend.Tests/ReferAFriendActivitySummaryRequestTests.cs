using Atlantis.Framework.ReferAFriend.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.ReferAFriend.Tests
{
	[TestClass]
	public class ReferAFriendActivityRequestTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.ReferAFriend.Impl.dll")]
		public void ReferAFriendActivitySummaryRequest_Success()
		{
			var request = new ReferAFriendActivitySummaryRequestData("dlc1", DateTime.Now, DateTime.Now, "847503", "", "", "", 0);
			var response = (ReferAFriendActivitySummaryResponseData)Engine.Engine.ProcessRequest(request, 7);
			Assert.IsNotNull(response.ReferralCount == 0);
			Assert.IsTrue(response.IsSuccess);
		}
	}
}
